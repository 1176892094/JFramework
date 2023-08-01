using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using JFramework.Core;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Networking;

namespace JFramework
{
    public static class AssetHelper
    {
        private static readonly Dictionary<string, AssetData> clientDataList = new Dictionary<string, AssetData>();
        private static readonly Dictionary<string, AssetData> serverDataList = new Dictionary<string, AssetData>();
        private static readonly List<string> assetDataList = new List<string>();
        private static readonly string clientInfoName = $"{AssetSetting.Info}.txt";
        private static readonly string serverInfoName = $"{AssetSetting.Info}_TMP.txt";
        private static readonly string clientInfoPath = $"{Application.persistentDataPath}/{clientInfoName}";
        private static readonly string serverInfoPath = $"{Application.persistentDataPath}/{serverInfoName}";
        private static readonly string localBuildPath = $"{Application.streamingAssetsPath}/{AssetSetting.Platform.ToString()}";

        /// <summary>
        /// 用于检测热更新的函数
        /// </summary>
        internal static async Task<bool> UpdateAsync()
        {
            assetDataList.Clear();
            clientDataList.Clear();
            serverDataList.Clear();
            if (await LoadServerAssetBundleInfo())
            {
                var remoteInfo = await File.ReadAllTextAsync(serverInfoPath);
                GetAssetBundleInfo(remoteInfo, serverDataList);
                Debug.Log("解析远端对比文件完成");
                if (await LoadClientAssetBundleInfo())
                {
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
                    var enumerable = clientDataList.Keys.Where(name => File.Exists($"{Application.persistentDataPath}/{name}"));
                    foreach (var fileName in enumerable)
                    {
                        Debug.Log("Delete:" + fileName);
                        File.Delete($"{Application.persistentDataPath}/{fileName}");
                    }

                    Debug.Log("下载和更新AB包文件");
                    if (await LoadAssetBundles())
                    {
                        Debug.Log("更新本地AB包对比文件为最新");
                        await File.WriteAllTextAsync($"{clientInfoPath}", remoteInfo);
                        return true;
                    }

                    return false;
                }
            }

            Debug.Log("更新失败。");
            return false;
        }

        /// <summary>
        /// 下载AB包的TMP信息文件
        /// </summary>
        private static async Task<bool> LoadServerAssetBundleInfo()
        {
            var reloads = 5;
            var success = false;
            var localPath = $"{serverInfoPath}";
            while (!success && reloads-- > 0)
            {
                Debug.Log("从远端下载对比文件: " + localPath);
                success = await Download($"{clientInfoName}", localPath);
            }

            return success;
        }

        /// <summary>
        /// 本地AB包对比文件加载 解析信息
        /// </summary>
        private static async Task<bool> LoadClientAssetBundleInfo()
        {
            if (File.Exists($"{clientInfoPath}"))
            {
                Debug.Log("解析本地文件: " + clientInfoPath);
                return await GetClientAssetBundleInfo($"file://{clientInfoPath}");
            }

            if (File.Exists($"{Application.streamingAssetsPath}/{AssetSetting.Platform}/{clientInfoName}"))
            {
#if UNITY_ANDROID
                string path = Application.streamingAssetsPath;
#else
                string path = $"file://{Application.streamingAssetsPath}";
#endif
                return await GetClientAssetBundleInfo($"{path}/{AssetSetting.Platform}/{clientInfoName}");
            }

            return true;
        }

        /// <summary>
        /// 协同程序 加载本地信息 并且解析存入字典
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private static async Task<bool> GetClientAssetBundleInfo(string filePath)
        {
            using var request = UnityWebRequest.Get(filePath);
            var operation = request.SendWebRequest();
            while (!operation.isDone)
            {
                await Task.Yield();
            }

            if (request.result == UnityWebRequest.Result.Success)
            {
                GetAssetBundleInfo(request.downloadHandler.text, clientDataList);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取下载下来的AB包中的信息
        /// </summary>
        private static void GetAssetBundleInfo(string info, Dictionary<string, AssetData> assetInfos)
        {
            var infoArray = info.Split('\n');
            foreach (var array in infoArray)
            {
                var infos = array.Split(" ");
                assetInfos.Add(infos[0], new AssetData(infos[0], infos[1], infos[2]));
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
                    var success = await Download(fileName, $"{Application.persistentDataPath}/{fileName}");
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
            using var request = UnityWebRequest.Get($"{AssetSetting.LoadPath}/{AssetSetting.Platform}/{fileName}");
            try
            {
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
            catch (Exception e)
            {
                Debug.LogWarning($"下载 {fileName} 文件失败\n{e}");
            }

            return false;
        }

#if UNITY_EDITOR
        /// <summary>
        /// 上传所有文件到服务器
        /// </summary>
        public static async void Uploads()
        {
            DirectoryInfo directoryInfo = Directory.CreateDirectory($"{localBuildPath}");
            var fileInfos = directoryInfo.GetFiles();
            foreach (var info in fileInfos)
            {
                if (info.Extension is "" or ".txt")
                {
                    await Upload(info.FullName, info.Name);
                }
            }
        }

        /// <summary>
        /// 上传指定文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        private static async Task Upload(string filePath, string fileName)
        {
            var request = new UnityWebRequest($"{AssetSetting.LoadPath}/{AssetSetting.Platform}/{fileName}", "POST");
            var fileData = await File.ReadAllBytesAsync(filePath);
            request.uploadHandler = new UploadHandlerRaw(fileData);
            request.SetRequestHeader("Content-Type", "application/octet-stream");
            var operation = request.SendWebRequest();
            if (!operation.isDone)
            {
                await Task.Yield();
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("文件上传失败： " + request.error);
            }
            else
            {
                Debug.Log("文件上传成功");
            }
        }

        /// <summary>
        /// 创建AB校验文件
        /// </summary>
        public static void CreateAssetBundleInfo()
        {
            BuildPipeline.BuildAssetBundles($"{AssetSetting.BuildPath}/{AssetSetting.Platform}", (BuildAssetBundleOptions)AssetSetting.Options, (BuildTarget)AssetSetting.Platform);
            var directory = Directory.CreateDirectory($"{localBuildPath}");
            var fileInfos = directory.GetFiles();
            var builder = PoolManager.Pop<StringBuilder>();
            foreach (var info in fileInfos)
            {
                if (info.Extension == "")
                {
                    builder.AppendFormat("{0} {1} {2}\n", info.Name, info.Length, GetMD5(info.FullName));
                }
            }

            var result = builder.ToString()[..^1];
            File.WriteAllText($"{localBuildPath}/{clientInfoName}", result);
            PoolManager.Push(builder);
            builder.Clear();
        }

        /// <summary>
        /// 获取MD5码
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private static string GetMD5(string filePath)
        {
            using var file = new FileStream(filePath, FileMode.Open);
            var md5 = new MD5CryptoServiceProvider();
            var md5Info = md5.ComputeHash(file);
            var builder = PoolManager.Pop<StringBuilder>();
            foreach (var info in md5Info)
            {
                builder.Append(info.ToString("X2"));
            }

            var result = builder.ToString();
            PoolManager.Push(builder);
            builder.Clear();
            return result;
        }
#endif
    }
}