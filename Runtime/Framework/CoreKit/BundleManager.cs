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

namespace JFramework.Core
{
    public static class BundleManager
    {
        private static readonly List<string> updateBundles = new();
        private static Dictionary<string, BundleData> clientBundles = new();
        private static Dictionary<string, BundleData> remoteBundles = new();
        public static event Action<List<long>> OnLoadStart;
        public static event Action<string, float> OnLoadUpdate;
        public static event Action<bool> OnLoadComplete;

        public static async void UpdateAssetBundles()
        {
            if (!GlobalManager.Instance) return;
            if (!await GetRemoteBundle())
            {
                Debug.Log("更新失败。");
                OnLoadComplete?.Invoke(false);
                return;
            }

            var remote = await File.ReadAllTextAsync(SettingManager.remoteInfoPath);
            var bundles = JsonManager.Reader<List<BundleData>>(remote);
            remoteBundles = bundles.ToDictionary(bundle => bundle.name);

            var client = await GetClientBundle();
            if (client != null)
            {
                bundles = JsonManager.Reader<List<BundleData>>(client);
                clientBundles = bundles.ToDictionary(bundle => bundle.name);
            }

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
                var path = SettingManager.GetPersistentPath(key);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }

            if (await GetAssetBundles())
            {
                await File.WriteAllTextAsync(SettingManager.clientInfoPath, remote);
                OnLoadComplete?.Invoke(true);
            }
        }

        private static async Task<bool> GetRemoteBundle()
        {
            var reloads = 5;
            var success = false;
            while (!success && reloads-- > 0)
            {
                var fileUri = SettingManager.GetRemoteFilePath(SettingManager.clientInfoName);
                using (var request = UnityWebRequest.Head(fileUri))
                {
                    request.timeout = 1;
                    await request.SendWebRequest();
                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        Debug.Log($"校验 {SettingManager.Instance.assetInfo} 失败\n");
                        continue;
                    }
                }

                using (var request = UnityWebRequest.Get(fileUri))
                {
                    await request.SendWebRequest();
                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        Debug.Log($"下载 {SettingManager.Instance.assetInfo} 失败\n");
                        continue;
                    }

                    var contents = request.downloadHandler.text;
                    await File.WriteAllTextAsync(SettingManager.remoteInfoPath, contents);
                }

                success = true;
            }

            return success;
        }

        private static async Task<string> GetClientBundle()
        {
            if (File.Exists(SettingManager.clientInfoPath))
            {
                return await File.ReadAllTextAsync(SettingManager.clientInfoPath);
            }

#if UNITY_ANDROID && !UNITY_EDITOR
            using var request = UnityWebRequest.Get(SettingManager.streamingInfoPath);
            await request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                return request.downloadHandler.text;
            }
#else
            if (File.Exists(SettingManager.streamingInfoPath))
            {
                return await File.ReadAllTextAsync(SettingManager.streamingInfoPath);
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
                var sizeList = new List<long>();
                foreach (var bundle in bundles)
                {
                    var fileUri = SettingManager.GetRemoteFilePath(bundle);
                    using (var request = UnityWebRequest.Head(fileUri))
                    {
                        request.timeout = 1;
                        await request.SendWebRequest();
                        if (request.result != UnityWebRequest.Result.Success)
                        {
                            Debug.Log($"获取 {bundle} 文件失败\n");
                            continue;
                        }

                        sizeList.Add(long.Parse(request.GetResponseHeader("Content-Length")));
                    }
                }

                OnLoadStart?.Invoke(sizeList);
                foreach (var bundle in bundles)
                {
                    var fileUri = SettingManager.GetRemoteFilePath(bundle);
                    using (var request = UnityWebRequest.Get(fileUri))
                    {
                        var result = request.SendWebRequest();
                        while (!result.isDone && GlobalManager.Instance)
                        {
                            OnLoadUpdate?.Invoke(bundle, request.downloadProgress);
                            await Task.Yield();
                        }

                        OnLoadUpdate?.Invoke(bundle, 1);
                        if (request.result != UnityWebRequest.Result.Success)
                        {
                            Debug.Log($"下载 {bundle} 文件失败\n");
                            continue;
                        }

                        var contents = request.downloadHandler.data;
                        await File.WriteAllBytesAsync(SettingManager.GetPersistentPath(bundle), contents);
                    }

                    if (updateBundles.Contains(bundle))
                    {
                        updateBundles.Remove(bundle);
                    }
                }
            }

            return updateBundles.Count == 0;
        }

        internal static void UnRegister()
        {
            OnLoadStart = null;
            OnLoadUpdate = null;
            OnLoadComplete = null;
            clientBundles.Clear();
            remoteBundles.Clear();
            updateBundles.Clear();
        }

        [Serializable]
        internal struct BundleData
        {
            public string code;
            public string name;
            public string size;

            public BundleData(string code, string name, string size)
            {
                this.code = code;
                this.name = name;
                this.size = size;
            }

            public static bool operator ==(BundleData a, BundleData b) => a.code == b.code;

            public static bool operator !=(BundleData a, BundleData b) => a.code != b.code;

            private bool Equals(BundleData other) => size == other.size && code == other.code && name == other.name;

            public override bool Equals(object obj) => obj is BundleData other && Equals(other);

            public override int GetHashCode() => HashCode.Combine(size, code, name);
        }
    }
}