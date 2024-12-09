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
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace JFramework
{
    public static partial class AssetManager
    {
        private static AssetBundle mainAsset;
        private static AssetBundleManifest manifest;
        private static readonly Dictionary<string, AssetBundle> bundles = new();
        private static readonly Dictionary<string, Task<AssetBundle>> requests = new();
        private static readonly Dictionary<string, KeyValuePair<string, string>> assets = new();
        public static event Action<string[]> OnLoadEntry;
        public static event Action<string> OnLoadUpdate;
        public static event Action OnLoadComplete;

        public static async Task<T> Load<T>(string assetPath) where T : Object
        {
            try
            {
                if (GlobalManager.Instance)
                {
                    var assetData = await LoadAsset(assetPath, typeof(T));
                    return (T)assetData;
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"加载资源 {assetPath.Red()} 失败!\n" + e);
            }

            return null;
        }

        public static async void Load<T>(string assetPath, Action<T> action) where T : Object
        {
            try
            {
                if (GlobalManager.Instance)
                {
                    var assetData = await LoadAsset(assetPath, typeof(T));
                    action?.Invoke((T)assetData);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"加载资源 {assetPath.Red()} 失败!\n" + e);
            }
        }

        private static async Task<Object> LoadAsset(string assetPath, Type assetType)
        {
            if (GlobalManager.mode == AssetMode.AssetBundle)
            {
                var dependency = await LoadDependency(assetPath);
                var assetBundle = await LoadAssetBundle(dependency.Key);
                var assetData = AssetBundleLoader.Load(assetBundle, dependency.Value, assetType);
                assetData ??= ResourcesLoader.Load(assetPath, assetType);
                return assetData;
            }
            else
            {
#if UNITY_EDITOR
                var assetData = SimulateLoader.Load(assetPath, assetType);
                assetData ??= ResourcesLoader.Load(assetPath, assetType);
                return assetData;
#else
                var assetData = ResourcesLoader.LoadAsync<T>(assetPath, assetType);
                return assetData;
#endif
            }
        }

        private static async Task LoadBundleManifest()
        {
            if (mainAsset != null) return;
            mainAsset = await LoadAssetBundle(GlobalSetting.Instance.platform.ToString());
            manifest = mainAsset.LoadAsset<AssetBundleManifest>(nameof(AssetBundleManifest));
            OnLoadEntry?.Invoke(manifest.GetAllAssetBundles());
        }

        private static async Task<KeyValuePair<string, string>> LoadDependency(string assetPath)
        {
            await LoadBundleManifest();
            if (!assets.TryGetValue(assetPath, out var assetData))
            {
                var index = assetPath.LastIndexOf('/');
                if (index < 0)
                {
                    assetData = new KeyValuePair<string, string>(string.Empty, assetPath);
                }
                else
                {
                    var assetBundle = assetPath.Substring(0, index).ToLower();
                    assetData = new KeyValuePair<string, string>(assetBundle, assetPath.Substring(index + 1));
                }

                assets.Add(assetPath, assetData);
            }

            var dependencies = manifest.GetAllDependencies(assetData.Key);
            foreach (var dependency in dependencies)
            {
                _ = LoadAssetBundle(dependency);
            }

            return assetData;
        }

        public static async void LoadAssetBundles()
        {
            await LoadBundleManifest();
            var assetBundles = manifest.GetAllAssetBundles();
            foreach (var assetBundle in assetBundles)
            {
                _ = LoadAssetBundle(assetBundle);
            }

            await Task.WhenAll(requests.Values);
            OnLoadComplete?.Invoke();
        }

        private static async Task<AssetBundle> LoadAssetBundle(string assetPath)
        {
            if (string.IsNullOrEmpty(assetPath))
            {
                return null;
            }

            if (bundles.TryGetValue(assetPath, out var assetBundle))
            {
                return assetBundle;
            }

            if (requests.TryGetValue(assetPath, out var request))
            {
                return await request;
            }

            request = LoadAssetRequest(assetPath);
            requests.Add(assetPath, request);
            try
            {
                return await request;
            }
            finally
            {
                requests.Remove(assetPath);
            }
        }

        private static async Task<AssetBundle> LoadAssetRequest(string assetPath)
        {
            var assetData = await BundleManager.GetRequest(assetPath);
            if (assetData.Key == BundlePlatform.Default)
            {
                var bytes = await Task.Run(() => Obfuscator.Decrypt(File.ReadAllBytes(assetData.Value)));
                if (GlobalManager.Instance)
                {
                    var assetBundle = AssetBundle.LoadFromMemory(bytes);
                    Debug.Log("解密AB包：" + assetPath);
                    bundles.Add(assetPath, assetBundle);
                    OnLoadUpdate?.Invoke(assetPath);
                    return assetBundle;
                }
            }

            if (assetData.Key == BundlePlatform.Android)
            {
                using var request = UnityWebRequest.Get(assetData.Value);
                await request.SendWebRequest();
                if (request.result == UnityWebRequest.Result.Success)
                {
                    var bytes = request.downloadHandler.data;
                    bytes = await Task.Run(() => Obfuscator.Decrypt(bytes));
                    if (GlobalManager.Instance)
                    {
                        var assetBundle = AssetBundle.LoadFromMemory(bytes);
                        Debug.Log("解密AB包：" + assetPath);
                        bundles.Add(assetPath, assetBundle);
                        OnLoadUpdate?.Invoke(assetPath);
                        return assetBundle;
                    }
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