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
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;

namespace JFramework.Core
{
    /// <summary>
    /// AB包更新相关
    /// </summary>
    public class RequestManager : Controller<GlobalManager>
    {
        /// <summary>
        /// 本地数据列表
        /// </summary>
        [ShowInInspector] private Dictionary<string, AssetData> clientAssets = new Dictionary<string, AssetData>();

        /// <summary>
        /// 远端数据列表
        /// </summary>
        [ShowInInspector] private Dictionary<string, AssetData> remoteAssets = new Dictionary<string, AssetData>();

        /// <summary>
        /// 资源数据列表
        /// </summary>
        private readonly List<string> updateAssets = new List<string>();

        /// <summary>
        /// 当开始下载资源(资源数量)
        /// </summary>
        public event Action<int> OnLoadStart;

        /// <summary>
        /// 资源加载进度(资源名称，加载进度)
        /// </summary>
        public event Action<string, float> OnLoadUpdate;

        /// <summary>
        /// 当资源下载完成
        /// </summary>
        public event Action OnLoadComplete;

        /// <summary>
        /// 检测是否需要更新
        /// </summary>
        /// <returns></returns>
        public async Task<bool> UpdateAssetBundles()
        {
            if (!GlobalManager.Runtime) return false;
            var success = await GetRemoteInfo();
            if (success)
            {
                Debug.Log("解析远端对比文件完成");
                var remoteInfo = await File.ReadAllTextAsync(GlobalSetting.remoteInfoPath);
                var jsonData = JsonUtility.FromJson<Variables<AssetData>>(remoteInfo);
                remoteAssets = jsonData.value.ToDictionary(data => data.name);

                var clientInfo = await GetClientInfo();
                if (clientInfo != null)
                {
                    jsonData = JsonUtility.FromJson<Variables<AssetData>>(clientInfo);
                    clientAssets = jsonData.value.ToDictionary(data => data.name);
                }

                Debug.Log("解析本地对比文件完成");
                foreach (var fileName in remoteAssets.Keys)
                {
                    if (!clientAssets.ContainsKey(fileName))
                    {
                        updateAssets.Add(fileName);
                    }
                    else
                    {
                        if (clientAssets[fileName] != remoteAssets[fileName])
                        {
                            updateAssets.Add(fileName);
                        }

                        clientAssets.Remove(fileName);
                    }
                }

                Debug.Log("删除弃用的资源文件");
                foreach (var path in clientAssets.Keys.Select(GlobalSetting.GetPersistentPath).Where(File.Exists))
                {
                    File.Delete(path);
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
        private async Task<bool> GetRemoteInfo()
        {
            var reloads = 5;
            var success = false;
            while (!success && reloads-- > 0)
            {
                var fileUri = GlobalSetting.GetRemoteFilePath(GlobalSetting.clientInfoName);
                using (var request = UnityWebRequest.Head(fileUri))
                {
                    request.timeout = 1;
                    await request.SendWebRequest();
                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        Debug.Log($"校验 {GlobalSetting.Instance.assetInfo} 失败\n");
                        continue;
                    }
                }

                using (var request = UnityWebRequest.Get(fileUri))
                {
                    await request.SendWebRequest();
                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        Debug.Log($"下载 {GlobalSetting.Instance.assetInfo} 失败\n");
                        continue;
                    }

                    var contents = request.downloadHandler.text;
                    await File.WriteAllTextAsync(GlobalSetting.remoteInfoPath, contents);
                }

                success = true;
            }

            return success;
        }

        /// <summary>
        /// 异步读取文件
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetClientInfo()
        {
            if (File.Exists(GlobalSetting.clientInfoPath))
            {
                return await File.ReadAllTextAsync(GlobalSetting.clientInfoPath);
            }

#if UNITY_ANDROID && !UNITY_EDITOR
            using var request = UnityWebRequest.Get(GlobalSetting.streamingInfoPath);
            await request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                return request.downloadHandler.text;
            }
#else
            if (File.Exists(GlobalSetting.streamingInfoPath))
            {
                return await File.ReadAllTextAsync(GlobalSetting.streamingInfoPath);
            }

#endif
            return null;
        }

        /// <summary>
        /// 下载待下载列表中的AB包文件
        /// </summary>
        private async Task<bool> GetAssetBundles()
        {
            var reloads = 5;
            var copyList = updateAssets.ToList();
            OnLoadStart?.Invoke(updateAssets.Count);
            while (updateAssets.Count > 0 && reloads-- > 0)
            {
                foreach (var fileName in copyList)
                {
                    var fileUri = GlobalSetting.GetRemoteFilePath(fileName);
                    using (var request = UnityWebRequest.Get(fileUri))
                    {
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
                            continue;
                        }

                        var contents = request.downloadHandler.data;
                        await File.WriteAllBytesAsync(GlobalSetting.GetPersistentPath(fileName), contents);
                    }

                    if (updateAssets.Contains(fileName))
                    {
                        updateAssets.Remove(fileName);
                    }
                }
            }

            return updateAssets.Count == 0;
        }

        private void OnDestroy()
        {
            OnLoadStart = null;
            OnLoadUpdate = null;
            OnLoadComplete = null;
            updateAssets.Clear();
            clientAssets.Clear();
            remoteAssets.Clear();
        }
    }
}