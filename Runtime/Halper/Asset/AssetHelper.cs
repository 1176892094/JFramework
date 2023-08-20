using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JFramework.Core;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace JFramework
{
    /// <summary>
    /// AB包更新相关
    /// </summary>
    internal static class AssetHelper
    {
        /// <summary>
        /// 本地数据列表
        /// </summary>
        private static Dictionary<string, AssetData> clientDataList = new Dictionary<string, AssetData>();

        /// <summary>
        /// 远端数据列表
        /// </summary>
        private static Dictionary<string, AssetData> serverDataList = new Dictionary<string, AssetData>();

        /// <summary>
        /// 资源数据列表
        /// </summary>
        private static readonly List<string> assetDataList = new List<string>();

        /// <summary>
        /// 检测是否需要更新
        /// </summary>
        /// <returns></returns>
        internal static async Task UpdateVersion()
        {
            assetDataList.Clear();
            clientDataList.Clear();
            serverDataList.Clear();
            var success = await LoadServerAssetBundleInfo();
            if (success)
            {
                var serverInfo = await File.ReadAllTextAsync(GlobalSetting.serverInfoPath);
                var dataList = JsonConvert.DeserializeObject<List<AssetData>>(serverInfo);
                serverDataList = dataList.ToDictionary(data => data.name);
                Debug.Log("解析远端对比文件完成");
                await LoadClientAssetBundleInfo();
                Debug.Log("解析本地对比文件完成");
                foreach (var fileName in serverDataList.Keys)
                {
                    if (!clientDataList.ContainsKey(fileName))
                    {
                        assetDataList.Add(fileName);
                        Debug.Log("更新文件: " + fileName);
                    }
                    else
                    {
                        if (clientDataList[fileName] != serverDataList[fileName])
                        {
                            assetDataList.Add(fileName);
                            Debug.Log("更新文件: " + fileName);
                        }

                        clientDataList.Remove(fileName);
                    }
                }

                Debug.Log("删除无用的AB包文件");
                var fileList = clientDataList.Keys.Where(file => File.Exists(GlobalSetting.GetPerFile(file)));
                foreach (var fileName in fileList)
                {
                    File.Delete(GlobalSetting.GetPerFile(fileName));
                }

                Debug.Log("下载和更新AB包文件");
                success = await LoadAssetBundles();
                if (success)
                {
                    Debug.Log("更新本地AB包对比文件为最新");
                    await File.WriteAllTextAsync(GlobalSetting.clientInfoPath, serverInfo);
                    return;
                }
            }
            
            Debug.Log("更新失败。");
        }

        /// <summary>
        /// 从服务器下载校对文件
        /// </summary>
        private static async Task<bool> LoadServerAssetBundleInfo()
        {
            var reloads = 5;
            var success = false;
            while (!success && reloads-- > 0)
            {
                success = await Download(GlobalSetting.clientInfoName, GlobalSetting.serverInfoPath);
            }

            return success;
        }

        /// <summary>
        /// 本地AB包对比文件加载 解析信息
        /// </summary>
        private static async Task LoadClientAssetBundleInfo()
        {
            if (File.Exists(GlobalSetting.clientInfoPath))
            {
                var saveJson = await File.ReadAllTextAsync(GlobalSetting.clientInfoPath);
                var dataList = JsonConvert.DeserializeObject<List<AssetData>>(saveJson);
                clientDataList = dataList.ToDictionary(data => data.name);
                return;
            }

            if (File.Exists(GlobalSetting.localInfoPath))
            {
                var saveJson = await File.ReadAllTextAsync(GlobalSetting.localInfoPath);
                var dataList = JsonConvert.DeserializeObject<List<AssetData>>(saveJson);
                clientDataList = dataList.ToDictionary(data => data.name);
            }
        }

        /// <summary>
        /// 下载待下载列表中的AB包文件
        /// </summary>
        private static async Task<bool> LoadAssetBundles()
        {
            int reloads = 5;
            int curProgress = 0;
            int maxProgress = assetDataList.Count;
            var copies = assetDataList.ToList();
            while (assetDataList.Count > 0 && reloads-- > 0)
            {
                foreach (var fileName in copies)
                {
                   
                    var success = await Download(fileName,  GlobalSetting.GetPerFile(fileName));
                    if (!success) continue;
                    if (assetDataList.Contains(fileName))
                    {
                        assetDataList.Remove(fileName);
                    }

                    Debug.Log($"{++curProgress}/{maxProgress}");
                }
            }

            return assetDataList.Count == 0;
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="localPath"></param>
        /// <returns></returns>
        private static async Task<bool> Download(string fileName, string localPath)
        {
            using var request = UnityWebRequest.Get(GlobalSetting.GetRemoteFile(fileName));
            var operation = request.SendWebRequest();
            while (!operation.isDone)
            {
                await Task.Yield();
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogWarning($"下载 {fileName} 文件失败\n");
                return false;
            }

            await File.WriteAllBytesAsync(localPath, request.downloadHandler.data);
            Debug.Log($"下载 {fileName} 文件成功");
            return true;
        }
    }
}