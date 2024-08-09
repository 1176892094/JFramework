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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace JFramework.Core
{
    public static class AssetManager
    {
        internal const string AES_KEY = "ABCDEFGHIJKLMNOP";
        private static AssetBundle mainAsset;
        private static AssetBundleManifest manifest;
        private static readonly Dictionary<string, AssetData> assets = new();
        private static readonly Dictionary<string, AssetBundle> bundles = new();
        private static readonly Dictionary<string, Task<AssetBundle>> requests = new();

        public static async Task<T> Load<T>(string path) where T : Object
        {
            try
            {
                if (!GlobalManager.Instance) return null;
                var asset = await LoadAsset<T>(path, GlobalManager.mode);
                if (asset == null)
                {
                    Debug.LogWarning($"加载 {path} 资源为空！");
                    return null;
                }

                return asset;
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
                return null;
            }
        }

        public static async void Load<T>(string path, Action<T> action) where T : Object
        {
            try
            {
                if (!GlobalManager.Instance) return;
                var asset = await LoadAsset<T>(path, GlobalManager.mode);
                if (asset == null)
                {
                    Debug.LogWarning($"加载 {path} 资源为空！");
                    return;
                }

                action?.Invoke(asset);
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
        }

        public static async void LoadAssetBundles(Action action)
        {
            if (mainAsset == null)
            {
                mainAsset = await LoadAssetBundle(SettingManager.Instance.platform.ToString());
                manifest = mainAsset.LoadAsset<AssetBundleManifest>(nameof(AssetBundleManifest));
            }

            var assetBundles = manifest.GetAllAssetBundles();
            foreach (var assetBundle in assetBundles)
            {
                await LoadAssetBundle(assetBundle);
            }

            action?.Invoke();
        }

        private static async Task<T> LoadAsset<T>(string path, AssetMode mode) where T : Object
        {
            if (mode == AssetMode.AssetBundle)
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
                    var scenes = bundle.GetAllScenePaths();
                    if (scenes.Any(scene => Path.GetFileNameWithoutExtension(scene) == data.asset))
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
                mainAsset = await LoadAssetBundle(SettingManager.Instance.platform.ToString());
                manifest = mainAsset.LoadAsset<AssetBundleManifest>(nameof(AssetBundleManifest));
            }

            var dependencies = manifest.GetAllDependencies(assetData.bundle);
            foreach (var dependency in dependencies)
            {
                await LoadAssetBundle(dependency);
            }

            return assetData;
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
            Debug.Log("解密AB包：" + bundle);
            var path = SettingManager.GetPersistentPath(bundle);
            if (File.Exists(path))
            {
                var bytes = await File.ReadAllBytesAsync(path);
                bytes = await Obfuscator.DecryptAsync(bytes, AES_KEY);
                var result = await AssetBundle.LoadFromMemoryAsync(bytes);
                bundles.Add(bundle, result.assetBundle);
                return result.assetBundle;
            }

            path = SettingManager.GetStreamingPath(bundle);
#if UNITY_ANDROID && !UNITY_EDITOR
            using (var request = UnityWebRequestAssetBundle.GetAssetBundle(path))
            {
                await request.SendWebRequest();
                if (request.result == UnityWebRequest.Result.Success)
                {
                    var bytes = await Obfuscator.DecryptAsync(request.downloadHandler.data, AES_KEY);
                    var result = await AssetBundle.LoadFromMemoryAsync(bytes);
                    bundles.Add(bundle, result.assetBundle);
                    return result.assetBundle;
                }
            }
#else
            if (File.Exists(path))
            {
                var bytes = await File.ReadAllBytesAsync(path);
                bytes = await Obfuscator.DecryptAsync(bytes, AES_KEY);
                var result = await AssetBundle.LoadFromMemoryAsync(bytes);
                bundles.Add(bundle, result.assetBundle);
                return result.assetBundle;
            }

#endif
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
        }

        private static class AssetBundleLoader
        {
            public static T LoadAsync<T>(AssetBundle assetBundle, string assetName) where T : Object
            {
                if (assetBundle != null)
                {
                    if (typeof(T).IsSubclassOf(typeof(MonoBehaviour)))
                    {
                        var request = assetBundle.LoadAssetAsync<GameObject>(assetName);
                        return ((GameObject)Object.Instantiate(request.asset)).GetComponent<T>();
                    }
                    else
                    {
                        var request = assetBundle.LoadAssetAsync<T>(assetName);
                        return request.asset is GameObject ? (T)Object.Instantiate(request.asset) : (T)request.asset;
                    }
                }

                return null;
            }
        }

        private static class ResourcesLoader
        {
            public static async Task<T> LoadAsync<T>(string assetPath) where T : Object
            {
                if (typeof(T).IsSubclassOf(typeof(MonoBehaviour)))
                {
                    var request = await Resources.LoadAsync<GameObject>(assetPath);
                    return ((GameObject)Object.Instantiate(request.asset)).GetComponent<T>();
                }
                else
                {
                    var request = await Resources.LoadAsync<T>(assetPath);
                    return request.asset is GameObject ? (T)Object.Instantiate(request.asset) : (T)request.asset;
                }
            }
        }

#if UNITY_EDITOR
        private static class SimulateLoader
        {
            public static T LoadAsync<T>(string assetPath) where T : Object
            {
                if (SettingManager.objects.TryGetValue(char.ToUpper(assetPath[0]) + assetPath.Substring(1), out var editorPath))
                {
                    if (typeof(T).IsSubclassOf(typeof(MonoBehaviour)))
                    {
                        var request = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(editorPath);
                        return Object.Instantiate(request).GetComponent<T>();
                    }
                    else
                    {
                        var request = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(editorPath);
                        return request is GameObject ? Object.Instantiate(request) : request;
                    }
                }

                return null;
            }
        }
#endif

        [Serializable]
        private struct AssetData
        {
            public string asset;
            public string bundle;

            public AssetData(string path)
            {
                var index = path.LastIndexOf('/');
                if (index < 0)
                {
                    bundle = "";
                    asset = path;
                    return;
                }

                bundle = path.Substring(0, index).ToLower();
                asset = path.Substring(index + 1);
            }

            public override string ToString()
            {
                return $"资源包：{bundle} 资源名称：{asset}";
            }
        }
    }
}