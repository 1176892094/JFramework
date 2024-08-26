// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-02-04  16:14
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;

namespace JFramework.Core
{
    public static class BundleManager
    {
        private static readonly List<string> updateBundles = new();
        private static Dictionary<string, BundleData> clientBundles = new();
        private static Dictionary<string, BundleData> remoteBundles = new();

        public static async void UpdateAssetBundles()
        {
            if (GlobalManager.Instance)
            {
                var remote = await GetRemoteBundle();
                if (string.IsNullOrEmpty(remote))
                {
                    Debug.Log("更新失败。");
                    EventManager.Invoke(new OnBundleComplete(false));
                    return;
                }

                var bundles = JsonManager.Read<List<BundleData>>(remote);
                remoteBundles = bundles.ToDictionary(bundle => bundle.name);

                var client = await GetClientBundle();
                if (!string.IsNullOrEmpty(client))
                {
                    bundles = JsonManager.Read<List<BundleData>>(client);
                    clientBundles = bundles.ToDictionary(bundle => bundle.name);
                }

                updateBundles.Clear();
                foreach (var key in remoteBundles.Keys)
                {
                    if (clientBundles.TryGetValue(key, out var bundle))
                    {
                        if (bundle != remoteBundles[key])
                        {
                            updateBundles.Add(key);
                        }

                        clientBundles.Remove(key);
                    }
                    else
                    {
                        updateBundles.Add(key);
                    }
                }

                foreach (var key in clientBundles.Keys)
                {
                    var path = GlobalSetting.GetPersistentPath(key);
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }

                if (await GetAssetBundles())
                {
                    await File.WriteAllTextAsync(GlobalSetting.clientInfoPath, remote);
                    EventManager.Invoke(new OnBundleComplete(true));
                }
            }
        }

        private static async Task<string> GetRemoteBundle()
        {
            var reloads = 5;
            while (reloads-- > 0)
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

                    return request.downloadHandler.text;
                }
            }

            return null;
        }

        private static async Task<string> GetClientBundle()
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

        private static async Task<bool> GetAssetBundles()
        {
            var reloads = 5;
            var bundles = updateBundles.ToList();
            while (updateBundles.Count > 0 && reloads-- > 0)
            {
                var sizes = new List<long>();
                foreach (var bundle in bundles)
                {
                    var fileUri = GlobalSetting.GetRemoteFilePath(bundle);
                    using (var request = UnityWebRequest.Head(fileUri))
                    {
                        request.timeout = 1;
                        await request.SendWebRequest();
                        if (request.result != UnityWebRequest.Result.Success)
                        {
                            Debug.Log($"获取 {bundle} 文件失败\n");
                            continue;
                        }

                        sizes.Add(long.Parse(request.GetResponseHeader("Content-Length")));
                    }
                }

                EventManager.Invoke(new OnBundleEntry(sizes));
                foreach (var bundle in bundles)
                {
                    var fileUri = GlobalSetting.GetRemoteFilePath(bundle);
                    using (var request = UnityWebRequest.Get(fileUri))
                    {
                        var result = request.SendWebRequest();
                        while (!result.isDone && GlobalManager.Instance)
                        {
                            EventManager.Invoke(new OnBundleUpdate(bundle, request.downloadProgress));
                            await Task.Yield();
                        }
                        
                        EventManager.Invoke(new OnBundleUpdate(bundle, 1));
                        if (request.result != UnityWebRequest.Result.Success)
                        {
                            Debug.Log($"下载 {bundle} 文件失败\n");
                            continue;
                        }
                        
                        await File.WriteAllBytesAsync(GlobalSetting.GetPersistentPath(bundle), request.downloadHandler.data);
                        if (updateBundles.Contains(bundle))
                        {
                            updateBundles.Remove(bundle);
                        }
                    }
                }
            }

            return updateBundles.Count == 0;
        }

        internal static void UnRegister()
        {
            clientBundles.Clear();
            remoteBundles.Clear();
        }
    }
}