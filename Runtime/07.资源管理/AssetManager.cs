// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-02-04  15:28
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.IO;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace JFramework
{
    public static partial class AssetManager
    {
        private static AssetBundle mainAsset;
        private static AssetBundleManifest manifest;
        private static readonly Dictionary<string, AssetData> assets = new();
        private static readonly Dictionary<string, AssetBundle> bundles = new();
        private static readonly Dictionary<string, Task<AssetBundle>> requests = new();
        public static event Action<string[]> OnLoadEntry;
        public static event Action<string> OnLoadUpdate;
        public static event Action OnLoadComplete;

        public static async Task<T> Load<T>(string path) where T : Object
        {
            try
            {
                if (GlobalManager.Instance)
                {
                    var asset = await LoadAsset<T>(path);
                    if (asset == null)
                    {
                        Debug.LogWarning($"加载 {path} 资源为空！");
                        return null;
                    }

                    return asset;
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }

            return null;
        }

        public static async void Load<T>(string path, Action<T> action) where T : Object
        {
            try
            {
                if (GlobalManager.Instance)
                {
                    var asset = await LoadAsset<T>(path);
                    if (asset == null)
                    {
                        Debug.LogWarning($"加载 {path} 资源为空！");
                        return;
                    }

                    action?.Invoke(asset);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
        }

        private static async Task<T> LoadAsset<T>(string path) where T : Object
        {
            if (GlobalManager.mode == AssetMode.AssetBundle)
            {
                var data = await LoadAssetData(path);
                var bundle = await LoadAssetBundle(data.bundle);
                var asset = AssetBundleLoader.LoadAsync<T>(bundle, data.asset);
                asset ??= await ResourcesLoader.LoadAsync<T>(path);
                return asset;
            }
            else
            {
#if UNITY_EDITOR
                var asset = SimulateLoader.LoadAsync<T>(path);
                asset ??= await ResourcesLoader.LoadAsync<T>(path);
#else
                var asset = await ResourcesLoader.LoadAsync<T>(path);
#endif
                return asset;
            }
        }

        internal static async Task<string> LoadScene(string path)
        {
            try
            {
                if (!GlobalManager.Instance) return null;
                if (GlobalManager.mode == AssetMode.AssetBundle)
                {
                    var data = await LoadAssetData(path);
                    var bundle = await LoadAssetBundle(data.bundle);
                    if (bundle.GetAllScenePaths().Length > 0)
                    {
                        return data.asset;
                    }
                }

                return path.Split('/')[1];
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }

            return null;
        }

        private static async Task<AssetData> LoadAssetData(string path)
        {
            if (!assets.TryGetValue(path, out var assetData))
            {
                assetData = new AssetData(path);
                assets.Add(path, assetData);
            }

            if (mainAsset == null)
            {
                mainAsset = await LoadAssetBundle(GlobalSetting.Instance.platform.ToString());
                manifest = mainAsset.LoadAsset<AssetBundleManifest>(nameof(AssetBundleManifest));
                OnLoadEntry?.Invoke(manifest.GetAllAssetBundles());
            }

            var dependencies = manifest.GetAllDependencies(assetData.bundle);
            foreach (var dependency in dependencies)
            {
                await LoadAssetBundle(dependency);
            }

            return assetData;
        }

        public static async Task LoadAssetBundles()
        {
            if (mainAsset == null)
            {
                mainAsset = await LoadAssetBundle(GlobalSetting.Instance.platform.ToString());
                manifest = mainAsset.LoadAsset<AssetBundleManifest>(nameof(AssetBundleManifest));
                OnLoadEntry?.Invoke(manifest.GetAllAssetBundles());
            }

            var assetBundles = manifest.GetAllAssetBundles();
            var writeTasks = new List<Task>();
            foreach (var assetBundle in assetBundles)
            {
                writeTasks.Add(LoadAssetBundleTask(assetBundle));
            }

            await Task.WhenAll(writeTasks);
            await Task.Yield();
            OnLoadComplete?.Invoke();
        }

        private static async Task<AssetBundle> LoadAssetBundle(string bundle)
        {
            if (string.IsNullOrEmpty(bundle))
            {
                return null;
            }

            if (bundles.TryGetValue(bundle, out var result))
            {
                return result;
            }

            if (requests.TryGetValue(bundle, out var request))
            {
                return await request;
            }

            request = LoadAssetRequest(bundle);
            requests.Add(bundle, request);
            try
            {
                return await request;
            }
            finally
            {
                requests.Remove(bundle);
            }
        }

        private static async Task<AssetBundle> LoadAssetRequest(string bundle)
        {
            var (code, path) = await GetBundlePath(bundle);
            if (code == 0)
            {
                var bytes = await File.ReadAllBytesAsync(path);
                return await LoadAssetRequest(bundle, bytes);
            }

            if (code == 1)
            {
                using var request = UnityWebRequest.Get(path);
                await request.SendWebRequest();
                if (request.result == UnityWebRequest.Result.Success)
                {
                    return await LoadAssetRequest(bundle, request.downloadHandler.data);
                }
            }

            return null;
        }

        private static async Task<(int, string)> GetBundlePath(string bundle)
        {
            var path = GlobalSetting.GetPersistentPath(bundle);
            if (File.Exists(path))
            {
                return (0, path);
            }

            path = GlobalSetting.GetStreamingPath(bundle);
#if UNITY_ANDROID && !UNITY_EDITOR
            using var request = UnityWebRequest.Head(path);
            await request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                return (1, path);
            }
#else
            if (File.Exists(path))
            {
                return (0, path);
            }

#endif
            return (2, null);
        }

        private static Task<AssetBundle> LoadAssetRequest(string bundle, byte[] bytes)
        {
            bytes = Obfuscator.Decrypt(bytes);
            if (GlobalManager.Instance)
            {
                OnLoadUpdate?.Invoke(bundle);
                Debug.Log("解密AB包：" + bundle);
                var result = AssetBundle.LoadFromMemory(bytes);
                bundles.Add(bundle, result);
                return Task.FromResult(result);
            }

            return null;
        }

        private static async Task<Task> LoadAssetBundleTask(string bundle)
        {
            var (code, path) = await GetBundlePath(bundle);
            if (code == 0)
            {
                var writeTask = Task.Run(() => Obfuscator.Decrypt(File.ReadAllBytes(path)));
                WriteTask(writeTask);
                return writeTask;
            }

            if (code == 1)
            {
                using var request = UnityWebRequest.Get(path);
                await request.SendWebRequest();
                if (request.result == UnityWebRequest.Result.Success)
                {
                    var bytes = request.downloadHandler.data;
                    var writeTask = Task.Run(() => Obfuscator.Decrypt(bytes));
                    WriteTask(writeTask);
                    return writeTask;
                }
            }

            return Task.CompletedTask;

            async void WriteTask(Task<byte[]> writeTask)
            {
                var bytes = await writeTask;
                if (GlobalManager.Instance)
                {
                    OnLoadUpdate?.Invoke(bundle);
                    Debug.Log("解密AB包：" + bundle);
                    var result = AssetBundle.LoadFromMemory(bytes);
                    bundles.Add(bundle, result);
                }
            }
        }

        internal static void UnRegister()
        {
            AssetBundle.UnloadAllAssetBundles(true);
            assets.Clear();
            bundles.Clear();
            requests.Clear();
            manifest = null;
            mainAsset = null;
            OnLoadEntry = null;
            OnLoadUpdate = null;
            OnLoadComplete = null;
        }
    }
}