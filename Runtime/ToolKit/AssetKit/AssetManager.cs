// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-10-24  23:54
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;
#if UNITY_EDITOR
using JFramework.Editor;
#endif

namespace JFramework.Core
{
    public static class AssetManager
    {
        /// <summary>
        /// 存储AB包的名称和资源
        /// </summary>
        internal static readonly Dictionary<string, Asset> assets = new Dictionary<string, Asset>();

        /// <summary>
        /// 存储AB包的字典
        /// </summary>
        private static readonly Dictionary<string, AssetBundle> bundles = new Dictionary<string, AssetBundle>();

        /// <summary>
        /// 异步加载AB包的任务
        /// </summary>
        private static readonly Dictionary<string, Task<AssetBundle>> tasks = new Dictionary<string, Task<AssetBundle>>();

        /// <summary>
        /// 主包
        /// </summary>
        private static AssetBundle mainAsset;

        /// <summary>
        /// 声明文件
        /// </summary>
        private static AssetBundleManifest manifest;

        /// <summary>
        /// 异步加载AB依赖包
        /// </summary>
        /// <param name="bundle"></param>
        private static async Task LoadDependency(string bundle)
        {
            if (mainAsset == null)
            {
                mainAsset = await LoadAssetBundleTask(GlobalSetting.Instance.platform.ToString());
                manifest = mainAsset.LoadAsset<AssetBundleManifest>(nameof(AssetBundleManifest));
            }

            var dependencies = manifest.GetAllDependencies(bundle);
            foreach (var dependency in dependencies)
            {
                if (!bundles.ContainsKey(dependency))
                {
                    await LoadAssetBundleTask(dependency);
                }
            }
        }

        /// <summary>
        /// 异步加载AB包任务
        /// </summary>
        /// <param name="bundle"></param>
        /// <returns></returns>
        private static async Task<AssetBundle> LoadAssetBundleTask(string bundle)
        {
            if (tasks.TryGetValue(bundle, out var task))
            {
                return await task;
            }

            var newTask = LoadAssetBundleAsync(bundle);
            tasks[bundle] = newTask;

            try
            {
                return await newTask;
            }
            finally
            {
                tasks.Remove(bundle);
            }
        }

        /// <summary>
        /// 异步加载AB包
        /// </summary>
        /// <param name="bundle"></param>
        /// <returns></returns>
        private static async Task<AssetBundle> LoadAssetBundleAsync(string bundle)
        {
            var path = GlobalSetting.GetPersistentPath(bundle);
#if UNITY_EDITOR
            path = "file://" + path;
#endif
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

            path = GlobalSetting.GetStreamingPath(bundle);
#if UNITY_EDITOR
            path = "file://" + path;
#endif
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

            Debug.LogWarning($"加载 {bundle} 资源包失败");
            return null;
        }

        /// <summary>
        /// 根据路径加载
        /// </summary>
        /// <param name="path"></param>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async void LoadAsync<T>(string path, Action<T> action = null) where T : Object
        {
            if (!GlobalManager.Runtime) return;
#if UNITY_EDITOR
            if (!BuildSetting.Instance.isRemote)
            {
                BuildSetting.Instance.LoadAsync(path, action);
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
                assetBundle = await LoadAssetBundleTask(assetData.bundle);
                if (assetBundle == null)
                {
                    return;
                }
            }

            if (typeof(T).IsSubclassOf(typeof(Component)))
            {
                var obj = assetBundle.LoadAssetAsync<GameObject>(assetData.asset);
                var asset = ((GameObject)Object.Instantiate(obj.asset)).GetComponent<T>();
                action?.Invoke(asset);
            }
            else
            {
                var obj = assetBundle.LoadAssetAsync<T>(assetData.asset);
                var asset = obj.asset is GameObject ? (T)Object.Instantiate(obj.asset) : (T)obj.asset;
                action?.Invoke(asset);
            }
        }

        /// <summary>
        /// 根据路径加载
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal static async Task<AsyncOperation> LoadSceneAsync(string path)
        {
            if (!GlobalManager.Runtime) return null;
#if UNITY_EDITOR
            if (!BuildSetting.Instance.isRemote)
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
                assetBundle = await LoadAssetBundleTask(assetData.bundle);
                if (assetBundle == null)
                {
                    return null;
                }
            }

            return UnitySceneManager.LoadSceneAsync(assetData.asset, LoadSceneMode.Single);
        }

        /// <summary>
        /// 卸载AB包的方法
        /// </summary>
        /// <param name="labelName"></param>
        public static void Unload(string labelName)
        {
            if (bundles.TryGetValue(labelName, out var assetBundle))
            {
                assetBundle.Unload(false);
                foreach (var asset in assets.Values.Where(asset => asset.bundle == labelName))
                {
                    assets.Remove($"{asset.bundle}/{asset.asset}");
                }

                bundles.Remove(labelName);
            }
        }

        /// <summary>
        /// 清空AB包的方法
        /// </summary>
        internal static void Dispose()
        {
            AssetBundle.UnloadAllAssetBundles(true);
            tasks.Clear();
            assets.Clear();
            bundles.Clear();
            manifest = null;
            mainAsset = null;
        }
    }
}