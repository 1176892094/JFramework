// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-02-04  16:14
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;

namespace JFramework
{
    public static class BundleManager
    {
        private static readonly HashSet<string> updateBundles = new();
        private static Dictionary<string, BundleData> clientBundles = new();
        private static Dictionary<string, BundleData> serverBundles = new();
        public static event Action<List<long>> OnLoadEntry;
        public static event Action<string, float> OnLoadUpdate;
        public static event Action<bool> OnLoadComplete;

        public static async void UpdateAssetBundles()
        {
            if (GlobalManager.Instance)
            {
                if (!Directory.Exists(GlobalSetting.assetBundlePath))
                {
                    Directory.CreateDirectory(GlobalSetting.assetBundlePath);
                }

                var serverFile = await GetServerRequest();
                if (!string.IsNullOrEmpty(serverFile))
                {
                    var assetBundles = JsonManager.Read<List<BundleData>>(serverFile);
                    serverBundles = assetBundles.ToDictionary(bundle => bundle.name);
                    OnLoadEntry?.Invoke(assetBundles.Select(bundle => long.Parse(bundle.size)).ToList());
                }

                if (string.IsNullOrEmpty(serverFile))
                {
                    OnLoadComplete?.Invoke(false);
                    Debug.Log("更新失败。");
                    return;
                }

                var clientFile = await GetClientRequest();
                if (!string.IsNullOrEmpty(clientFile))
                {
                    var assetBundles = JsonManager.Read<List<BundleData>>(clientFile);
                    clientBundles = assetBundles.ToDictionary(bundle => bundle.name);
                }

                updateBundles.Clear();
                foreach (var fileName in serverBundles.Keys)
                {
                    if (clientBundles.TryGetValue(fileName, out var assetBundle))
                    {
                        if (assetBundle != serverBundles[fileName])
                        {
                            updateBundles.Add(fileName);
                        }

                        clientBundles.Remove(fileName);
                    }
                    else
                    {
                        updateBundles.Add(fileName);
                    }
                }

                foreach (var filePath in clientBundles.Keys.Select(GlobalSetting.GetAssetBundles).Where(File.Exists))
                {
                    File.Delete(filePath);
                }

                var completed = await GetAssetBundles();
                if (completed)
                {
                    var filePath = GlobalSetting.GetAssetBundles(GlobalSetting.clientInfoName);
                    await File.WriteAllTextAsync(filePath, serverFile);
                }
                else
                {
                    Debug.Log("更新失败。");
                }

                OnLoadComplete?.Invoke(completed);
            }
        }

        private static async Task<string> GetServerRequest()
        {
            for (int i = 0; i < 5; i++)
            {
                var fileUri = GlobalSetting.GetRemoteFilePath(GlobalSetting.clientInfoName);
                using (var request = UnityWebRequest.Head(fileUri))
                {
                    request.timeout = 1;
                    await request.SendWebRequest();
                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        Debug.Log($"校验 {GlobalSetting.Instance.assetBundle} 失败\n");
                        continue;
                    }
                }

                using (var request = UnityWebRequest.Get(fileUri))
                {
                    await request.SendWebRequest();
                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        Debug.Log($"下载 {GlobalSetting.Instance.assetBundle} 失败\n");
                        continue;
                    }

                    return request.downloadHandler.text;
                }
            }

            return null;
        }

        private static async Task<string> GetClientRequest()
        {
            var fileInfo = await GetRequest(GlobalSetting.clientInfoName);
            if (fileInfo.Key == BundlePlatform.Default)
            {
                return await File.ReadAllTextAsync(fileInfo.Value);
            }

            if (fileInfo.Key == BundlePlatform.Android)
            {
                using var request = UnityWebRequest.Get(fileInfo.Value);
                await request.SendWebRequest();
                if (request.result == UnityWebRequest.Result.Success)
                {
                    return request.downloadHandler.text;
                }
            }

            return null;
        }

        internal static async Task<KeyValuePair<BundlePlatform, string>> GetRequest(string fileName)
        {
            var filePath = GlobalSetting.GetAssetBundles(fileName);
            if (File.Exists(filePath))
            {
                return new KeyValuePair<BundlePlatform, string>(BundlePlatform.Default, filePath);
            }

            filePath = GlobalSetting.GetStreamingPath(fileName);
#if UNITY_ANDROID && !UNITY_EDITOR
            using var request = UnityWebRequest.Head(filePath);
            await request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                 return new KeyValuePair<BundlePlatform, string>(BundlePlatform.Android, filePath);
            }
#else
            if (File.Exists(filePath))
            {
                return new KeyValuePair<BundlePlatform, string>(BundlePlatform.Default, filePath);
            }

#endif
            await Task.CompletedTask;
            return new KeyValuePair<BundlePlatform, string>(BundlePlatform.Unknown, filePath);
        }

        private static async Task<bool> GetAssetBundles()
        {
            var assetBundles = updateBundles.ToList();
            for (int i = 0; i < 5; i++)
            {
                foreach (var assetBundle in assetBundles)
                {
                    var fileUri = GlobalSetting.GetRemoteFilePath(assetBundle);
                    using (var request = UnityWebRequest.Head(fileUri))
                    {
                        request.timeout = 1;
                        await request.SendWebRequest();
                        if (request.result != UnityWebRequest.Result.Success)
                        {
                            Debug.Log($"获取 {assetBundle} 文件失败\n");
                        }
                    }

                    using (var request = UnityWebRequest.Get(fileUri))
                    {
                        var result = request.SendWebRequest();
                        while (!result.isDone && GlobalManager.Instance)
                        {
                            OnLoadUpdate?.Invoke(assetBundle, request.downloadProgress);
                            await Task.Yield();
                        }

                        OnLoadUpdate?.Invoke(assetBundle, 1);
                        if (request.result != UnityWebRequest.Result.Success)
                        {
                            Debug.Log($"下载 {assetBundle} 文件失败\n");
                            continue;
                        }

                        var filePath = GlobalSetting.GetAssetBundles(assetBundle);
                        await File.WriteAllBytesAsync(filePath, request.downloadHandler.data);
                        if (updateBundles.Contains(assetBundle))
                        {
                            updateBundles.Remove(assetBundle);
                        }
                    }
                }

                if (updateBundles.Count == 0)
                {
                    break;
                }
            }

            return updateBundles.Count == 0;
        }

        internal static void UnRegister()
        {
            OnLoadEntry = null;
            OnLoadUpdate = null;
            OnLoadComplete = null;
            clientBundles.Clear();
            serverBundles.Clear();
        }
    }
}