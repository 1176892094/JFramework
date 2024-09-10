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
using System.Linq;
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

        public static async Task<T> Load<T>(string assetPath) where T : Object
        {
            try
            {
                if (GlobalManager.Instance)
                {
                    var assetData = await LoadAsset<T>(assetPath);
                    return assetData;
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"加载资源 {assetPath} 失败!\n" + e);
            }

            return null;
        }

        public static async void Load<T>(string assetPath, Action<T> action) where T : Object
        {
            try
            {
                if (GlobalManager.Instance)
                {
                    var assetData = await LoadAsset<T>(assetPath);
                    action?.Invoke(assetData);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"加载资源 {assetPath} 失败!\n" + e);
            }
        }

        private static async Task<T> LoadAsset<T>(string assetPath) where T : Object
        {
            if (GlobalManager.mode == AssetMode.AssetBundle)
            {
                var assetInfo = await LoadDependency(assetPath);
                var assetBundle = await LoadAssetBundle(assetInfo.bundle);
                var assetData = AssetBundleLoader.LoadAsync<T>(assetBundle, assetInfo.asset);
                assetData ??= await ResourcesLoader.LoadAsync<T>(assetPath);
                return assetData;
            }
            else
            {
#if UNITY_EDITOR
                var assetData = SimulateLoader.LoadAsync<T>(assetPath);
                assetData ??= await ResourcesLoader.LoadAsync<T>(assetPath);
#else
                var assetData = await ResourcesLoader.LoadAsync<T>(assetPath);
#endif
                return assetData;
            }
        }

        internal static async Task<string> LoadScene(string assetPath)
        {
            if (GlobalManager.mode == AssetMode.AssetBundle)
            {
                var assetInfo = await LoadDependency(assetPath);
                var assetBundle = await LoadAssetBundle(assetInfo.bundle);
                if (assetBundle.GetAllScenePaths().Any(sceneData => sceneData == assetInfo.asset))
                {
                    return assetInfo.asset;
                }
            }

            return assetPath.Substring(assetPath.LastIndexOf('/') + 1);
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

        private static async Task<AssetData> LoadDependency(string assetPath)
        {
            if (mainAsset == null)
            {
                mainAsset = await LoadAssetBundle(GlobalSetting.Instance.platform.ToString());
                manifest = mainAsset.LoadAsset<AssetBundleManifest>(nameof(AssetBundleManifest));
                OnLoadEntry?.Invoke(manifest.GetAllAssetBundles());
            }

            if (!assets.TryGetValue(assetPath, out var assetData))
            {
                assetData = new AssetData(assetPath);
                assets.Add(assetPath, assetData);
            }

            var dependencies = manifest.GetAllDependencies(assetData.bundle);
            foreach (var dependency in dependencies)
            {
                _ = LoadAssetBundle(dependency);
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
            foreach (var assetBundle in assetBundles)
            {
                _ = LoadAssetBundle(assetBundle);
            }

            await Task.WhenAll(requests.Values);
            OnLoadComplete?.Invoke();
        }

        private static async Task<AssetBundle> LoadAssetRequest(string bundle)
        {
            var fileInfo = await BundleManager.GetRequest(bundle);
            if (fileInfo.Key == 0)
            {
                var bytes = await Task.Run(() => Obfuscator.Decrypt(File.ReadAllBytes(fileInfo.Value)));
                var assetBundle = AssetBundle.LoadFromMemory(bytes);
                Debug.Log("解密AB包：" + bundle);
                bundles.Add(bundle, assetBundle);
                OnLoadUpdate?.Invoke(bundle);
                return assetBundle;
            }

            if (fileInfo.Key == 1)
            {
                using var request = UnityWebRequest.Get(fileInfo.Value);
                await request.SendWebRequest();
                if (request.result == UnityWebRequest.Result.Success)
                {
                    var bytes = request.downloadHandler.data;
                    bytes = await Task.Run(() => Obfuscator.Decrypt(bytes));
                    var assetBundle = AssetBundle.LoadFromMemory(bytes);
                    Debug.Log("解密AB包：" + bundle);
                    bundles.Add(bundle, assetBundle);
                    OnLoadUpdate?.Invoke(bundle);
                    return assetBundle;
                }
            }

            return null;
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