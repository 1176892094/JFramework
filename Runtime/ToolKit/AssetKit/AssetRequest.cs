// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-10-24  23:53
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JFramework.Core;
using UnityEngine;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;

namespace JFramework
{
    /// <summary>
    /// AB包更新相关
    /// </summary>
    public static class AssetRequest
    {
        /// <summary>
        /// 本地数据列表
        /// </summary>
        private static Dictionary<string, AssetData> clientAssets = new Dictionary<string, AssetData>();

        /// <summary>
        /// 远端数据列表
        /// </summary>
        private static Dictionary<string, AssetData> remoteAssets = new Dictionary<string, AssetData>();

        /// <summary>
        /// 资源数据列表
        /// </summary>
        private static readonly List<string> changeAssets = new List<string>();

        /// <summary>
        /// 当开始下载资源(资源数量)
        /// </summary>
        public static event Action<int> OnLoadStart;

        /// <summary>
        /// 资源加载进度(资源名称，加载进度)
        /// </summary>
        public static event Action<string, float> OnLoadUpdate;

        /// <summary>
        /// 当资源下载完成
        /// </summary>
        public static event Action OnLoadComplete;

        /// <summary>
        /// 检测是否需要更新
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> UpdateAssetBundles()
        {
            if (!GlobalManager.Runtime) return false;
            changeAssets.Clear();
            clientAssets.Clear();
            remoteAssets.Clear();
            var success = await GetRemoteInfo();
            if (success)
            {
                Debug.Log("解析远端对比文件完成");
                var remoteInfo = await File.ReadAllTextAsync(GlobalSetting.remoteInfoPath);
                var jsonData = JsonUtility.FromJson<Variables<AssetData>>(remoteInfo);
                remoteAssets = jsonData.value.ToDictionary(data => data.name);
                
                if (await HeadRequest(GlobalSetting.clientInfoPath))
                {
                    var clientInfo = await GetRequest(GlobalSetting.clientInfoPath);
                    jsonData = JsonUtility.FromJson<Variables<AssetData>>(clientInfo);
                    clientAssets = jsonData.value.ToDictionary(data => data.name);
                }
                else if (await HeadRequest(GlobalSetting.streamingInfoPath))
                {
                    var clientInfo = await GetRequest(GlobalSetting.streamingInfoPath);
                    jsonData = JsonUtility.FromJson<Variables<AssetData>>(clientInfo);
                    clientAssets = jsonData.value.ToDictionary(data => data.name);
                }

                Debug.Log("解析本地对比文件完成");
                foreach (var fileName in remoteAssets.Keys)
                {
                    if (!clientAssets.ContainsKey(fileName))
                    {
                        changeAssets.Add(fileName);
                    }
                    else
                    {
                        if (clientAssets[fileName] != remoteAssets[fileName])
                        {
                            changeAssets.Add(fileName);
                        }

                        clientAssets.Remove(fileName);
                    }
                }

                Debug.Log("删除弃用的资源文件");
                var files = clientAssets.Keys.Where(file => File.Exists(GlobalSetting.GetPersistentPath(file)));
                foreach (var file in files)
                {
                    File.Delete(GlobalSetting.GetPersistentPath(file));
                }

                success = await GetAssetBundles();
                if (success)
                {
                    await File.WriteAllTextAsync(GlobalSetting.clientInfoPath, remoteInfo);
                    OnLoadComplete?.Invoke();
                    return true;
                }
            }

            Debug.Log("更新失败。");
            return false;
        }

        /// <summary>
        /// 从服务器下载校对文件
        /// </summary>
        private static async Task<bool> GetRemoteInfo()
        {
            var reloads = 5;
            var success = false;
            while (!success && reloads-- > 0)
            {
                var fileUri = GlobalSetting.GetRemoteFilePath(GlobalSetting.clientInfoName);
                if (!await HeadRequest(fileUri)) continue;
                var contents = await GetRequest(fileUri);
                await File.WriteAllTextAsync(GlobalSetting.remoteInfoPath, contents);
                success = true;
            }

            return success;
        }

        /// <summary>
        /// 下载待下载列表中的AB包文件
        /// </summary>
        private static async Task<bool> GetAssetBundles()
        {
            var reloads = 5;
            var copyList = changeAssets.ToList();
            OnLoadStart?.Invoke(changeAssets.Count);
            while (changeAssets.Count > 0 && reloads-- > 0)
            {
                foreach (var fileName in copyList)
                {
                    var fileUri = GlobalSetting.GetRemoteFilePath(fileName);
                    if (!await HeadRequest(fileUri)) continue;
                    var contents = await GetRequest(fileUri, fileName);
                    if (contents == null) continue;
                    var path = GlobalSetting.GetPersistentPath(fileName);
                    await File.WriteAllBytesAsync(path, contents);
                    if (changeAssets.Contains(fileName))
                    {
                        changeAssets.Remove(fileName);
                    }
                }
            }

            return changeAssets.Count == 0;
        }

        /// <summary>
        /// 获取Head请求
        /// </summary>
        /// <param name="fileUri"></param>
        /// <returns></returns>
        private static async Task<bool> HeadRequest(string fileUri)
        {
            using var request = UnityWebRequest.Head(fileUri);
            request.timeout = 1;
            await request.SendWebRequest();
            return request.result == UnityWebRequest.Result.Success;
        }

        /// <summary>
        /// 获取对比文件请求
        /// </summary>
        /// <param name="fileUri"></param>
        /// <returns></returns>
        private static async Task<string> GetRequest(string fileUri)
        {
            using var request = UnityWebRequest.Get(fileUri);
            await request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log($"下载 对比文件 失败\n");
                return null;
            }

            return request.downloadHandler.text;
        }

        /// <summary>
        /// 下载文件请求
        /// </summary>
        /// <param name="fileUri"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static async Task<byte[]> GetRequest(string fileUri, string fileName)
        {
            using var request = UnityWebRequest.Get(fileUri);
            var result = request.SendWebRequest();
            while (!result.isDone && GlobalManager.Runtime)
            {
                OnLoadUpdate?.Invoke(fileName, request.downloadProgress);
                await Task.Yield();
            }

            OnLoadUpdate?.Invoke(fileName, 1);
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log($"下载 {fileName} 文件失败\n");
                return null;
            }

            return request.downloadHandler.data;
        }
    }
}