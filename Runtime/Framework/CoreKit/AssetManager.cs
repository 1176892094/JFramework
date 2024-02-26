// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-02-04  15:28
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************
// ReSharper disable All

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace JFramework.Core
{
    using AssetTask = Task<AssetBundle>;

    public sealed class AssetManager : ScriptableObject
    {
        [ShowInInspector, LabelText("加载资源")] private Dictionary<string, Asset> assets = new();
        [ShowInInspector, LabelText("等待列表")] private Dictionary<string, AssetTask> awaits = new();
        [ShowInInspector, LabelText("依赖列表")] private Dictionary<string, AssetBundle> bundles = new();
        private AssetBundle mainAsset;
        private AssetBundleManifest manifest;

        private async Task LoadDependency(string bundle)
        {
            if (mainAsset == null)
            {
                mainAsset = await LoadAssetTask(SettingManager.Instance.platform.ToString());
                manifest = mainAsset.LoadAsset<AssetBundleManifest>(nameof(AssetBundleManifest));
            }

            var dependencies = manifest.GetAllDependencies(bundle);
            foreach (var dependency in dependencies)
            {
                if (!bundles.ContainsKey(dependency))
                {
                    await LoadAssetTask(dependency);
                }
            }
        }

        private async Task<AssetBundle> LoadAssetTask(string bundle)
        {
            if (awaits.TryGetValue(bundle, out var task))
            {
                return await task;
            }

            var newTask = LoadAssetBundle(bundle);
            awaits[bundle] = newTask;

            try
            {
                return await newTask;
            }
            finally
            {
                awaits.Remove(bundle);
            }
        }

        private async Task<AssetBundle> LoadAssetBundle(string bundle)
        {
            var path = SettingManager.GetPersistentPath(bundle);
            if (File.Exists(path))
            {
                var request = await AssetBundle.LoadFromFileAsync(path);
                var assetBundle = request.assetBundle;
                bundles.Add(bundle, assetBundle);
                return assetBundle;
            }

            path = SettingManager.GetStreamingPath(bundle);
#if UNITY_ANDROID && !UNITY_EDITOR
            using (var request = UnityWebRequestAssetBundle.GetAssetBundle(path))
            {
                await request.SendWebRequest();
                if (request.result == UnityWebRequest.Result.Success)
                {
                    var assetBundle = DownloadHandlerAssetBundle.GetContent(request);
                    bundles.Add(bundle, assetBundle);
                    return assetBundle;
                }
            }
#else
            if (File.Exists(path))
            {
                var request = await AssetBundle.LoadFromFileAsync(path);
                var assetBundle = request.assetBundle;
                bundles.Add(bundle, assetBundle);
                return assetBundle;
            }

#endif
            Debug.LogWarning($"加载 {bundle} 资源包失败");
            return null;
        }

        public async Task<T> Load<T>(string path) where T : Object
        {
#if UNITY_EDITOR
            if (!SettingManager.Instance.remoteLoad)
            {
                return SettingManager.Instance.Load<T>(path);
            }
#endif
            if (!assets.TryGetValue(path, out var assetData))
            {
                assetData = new Asset(path);
                assets.Add(path, assetData);
            }

            await LoadDependency(assetData.bundle);
            if (!bundles.TryGetValue(assetData.bundle, out var assetBundle))
            {
                assetBundle = await LoadAssetTask(assetData.bundle);
                if (assetBundle == null)
                {
                    return null;
                }
            }

            if (typeof(T).IsSubclassOf(typeof(Component)))
            {
                var obj = assetBundle.LoadAssetAsync<GameObject>(assetData.asset);
                return ((GameObject)Instantiate(obj.asset)).GetComponent<T>();
            }
            else
            {
                var obj = assetBundle.LoadAssetAsync<T>(assetData.asset);
                return obj.asset is GameObject ? (T)Instantiate(obj.asset) : (T)obj.asset;
            }
        }

        public async void LoadAsync<T>(string path, Action<T> action = null) where T : Object
        {
#if UNITY_EDITOR
            if (!SettingManager.Instance.remoteLoad)
            {
                var asset = SettingManager.Instance.Load<T>(path);
                action?.Invoke(asset);
                return;
            }
#endif
            if (!assets.TryGetValue(path, out var assetData))
            {
                assetData = new Asset(path);
                assets.Add(path, assetData);
            }

            await LoadDependency(assetData.bundle);
            if (!bundles.TryGetValue(assetData.bundle, out var assetBundle))
            {
                assetBundle = await LoadAssetTask(assetData.bundle);
                if (assetBundle == null)
                {
                    return;
                }
            }

            if (typeof(T).IsSubclassOf(typeof(Component)))
            {
                var obj = assetBundle.LoadAssetAsync<GameObject>(assetData.asset);
                var asset = ((GameObject)Instantiate(obj.asset)).GetComponent<T>();
                action?.Invoke(asset);
            }
            else
            {
                var obj = assetBundle.LoadAssetAsync<T>(assetData.asset);
                var asset = obj.asset is GameObject ? (T)Instantiate(obj.asset) : (T)obj.asset;
                action?.Invoke(asset);
            }
        }

        internal async Task<AsyncOperation> LoadSceneAsync(string path)
        {
#if UNITY_EDITOR
            if (!SettingManager.Instance.remoteLoad)
            {
                return UnitySceneManager.LoadSceneAsync(path.Split('/')[1], LoadSceneMode.Single);
            }
#endif
            if (!assets.TryGetValue(path, out var assetData))
            {
                assetData = new Asset(path);
                assets.Add(path, assetData);
            }

            await LoadDependency(assetData.bundle);
            if (!bundles.TryGetValue(assetData.bundle, out var assetBundle))
            {
                assetBundle = await LoadAssetTask(assetData.bundle);
                if (assetBundle == null)
                {
                    return null;
                }
            }

            return UnitySceneManager.LoadSceneAsync(assetData.asset, LoadSceneMode.Single);
        }

        public void Unload(string bundle)
        {
            if (bundles.TryGetValue(bundle, out var assetBundle))
            {
                assetBundle.Unload(false);
                foreach (var asset in assets.Values.Where(asset => asset.bundle == bundle))
                {
                    assets.Remove($"{asset.bundle}/{asset.asset}");
                }

                bundles.Remove(bundle);
            }
        }

        internal void OnDisable()
        {
            AssetBundle.UnloadAllAssetBundles(true);
            awaits.Clear();
            assets.Clear();
            bundles.Clear();
            manifest = null;
            mainAsset = null;
        }
    }
}