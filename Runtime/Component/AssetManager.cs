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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
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
        /// 资源数据
        /// </summary>
        [Serializable]
        internal struct AssetData
        {
            /// <summary>
            /// 所在资源包名称
            /// </summary>
            public string bundleName;

            /// <summary>
            /// 资源名称
            /// </summary>
            public string assetName;

            /// <summary>
            /// 分割包名和资源名
            /// </summary>
            /// <param name="path"></param>
            public AssetData(string path)
            {
                var array = path.Split('/');
                bundleName = array[0].ToLower();
                assetName = array[1];
                assets.Add(path, this);
            }
        }

        /// <summary>
        /// 存储AB包的字典
        /// </summary>
        internal static readonly Dictionary<string, AssetBundle> bundles = new Dictionary<string, AssetBundle>();

        /// <summary>
        /// 存储AB包的名称和资源
        /// </summary>
        internal static readonly Dictionary<string, AssetData> assets = new Dictionary<string, AssetData>();

        /// <summary>
        /// 主包
        /// </summary>
        private static AssetBundle mainAsset;

        /// <summary>
        /// 声明文件
        /// </summary>
        private static AssetBundleManifest manifest;

        /// <summary>
        /// 加载指定包的依赖包
        /// </summary>
        /// <param name="bundleName"></param>
        private static void LoadDependencies(string bundleName)
        {
            if (mainAsset == null)
            {
                mainAsset = LoadFromFile(GlobalSetting.Instance.platform.ToString());
                manifest = mainAsset.LoadAsset<AssetBundleManifest>(nameof(AssetBundleManifest));
            }
            
            var dependencies = manifest.GetAllDependencies(bundleName);
            foreach (var dependency in dependencies)
            {
                if (!bundles.ContainsKey(dependency))
                {
                    bundles.Add(dependency, LoadFromFile(dependency));
                }
            }
        }

        /// <summary>
        /// 根据路径加载
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Load<T>(string path) where T : Object
        {
            if (!GlobalManager.Runtime) return null;
#if UNITY_EDITOR
            if (!AssetSetting.Instance.isRemote)
            {
                return AssetSetting.Instance.Load<T>(path);
            }
#endif
            if (!assets.TryGetValue(path, out var assetData))
            {
                assetData = new AssetData(path);
            }

            return Load<T>(assetData.bundleName, assetData.assetName);
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
            if (!AssetSetting.Instance.isRemote)
            {
                AssetSetting.Instance.LoadAsync(path, action);
                return;
            }
#endif
            if (!assets.TryGetValue(path, out var assetData))
            {
                assetData = new AssetData(path);
            }

            var asset = await LoadAsync<T>(assetData.bundleName, assetData.assetName);
            action?.Invoke(asset);
        }

        /// <summary>
        /// 根据路径加载
        /// </summary>
        /// <param name="path"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        internal static async void LoadSceneAsync(string path, Action<AsyncOperation> action = null)
        {
            if (!GlobalManager.Runtime) return;
#if UNITY_EDITOR
            if (!AssetSetting.Instance.isRemote)
            {
                AssetSetting.Instance.LoadSceneAsync(path, action);
                return;
            }
#endif
            if (!assets.TryGetValue(path, out var assetData))
            {
                assetData = new AssetData(path);
            }

            var asset = await LoadSceneAsync(assetData.bundleName, assetData.assetName);
            action?.Invoke(asset);
        }

        /// <summary>
        /// 泛型资源同步加载
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static T Load<T>(string bundleName, string assetName) where T : Object
        {
            LoadDependencies(bundleName);
            if (!bundles.TryGetValue(bundleName, out var assetBundle))
            {
                assetBundle = LoadFromFile(bundleName);
                if (assetBundle == null)
                {
                    Debug.LogWarning($"加载 {bundleName.Red()} 资源失败");
                    return null;
                }

                bundles[bundleName] = assetBundle;
            }

            if (typeof(T).IsSubclassOf(typeof(Component)))
            {
                var obj = assetBundle.LoadAsset<GameObject>(assetName);
                return Object.Instantiate(obj).GetComponent<T>();
            }
            else
            {
                var obj = assetBundle.LoadAsset<T>(assetName);
                return obj is GameObject ? Object.Instantiate(obj) : obj;
            }
        }

        /// <summary>
        /// 泛型异步加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bundleName"></param>
        /// <param name="assetName"></param>
        private static async Task<T> LoadAsync<T>(string bundleName, string assetName) where T : Object
        {
            LoadDependencies(bundleName);
            if (!bundles.TryGetValue(bundleName, out var assetBundle))
            {
                bundles.Add(bundleName, null);
                assetBundle = await LoadFromFileAsync(bundleName);
                if (assetBundle == null)
                {
                    bundles.Remove(bundleName);
                    Debug.LogWarning($"加载 {bundleName.Red()} 资源失败");
                    return null;
                }

                bundles[bundleName] = assetBundle;
            }
            
            if (typeof(T).IsSubclassOf(typeof(Component)))
            {
                var obj = assetBundle.LoadAssetAsync<GameObject>(assetName);
                return ((GameObject)Object.Instantiate(obj.asset)).GetComponent<T>();
            }
            else
            {
                var obj = assetBundle.LoadAssetAsync<T>(assetName);
                return obj.asset is GameObject ? (T)Object.Instantiate(obj.asset) : (T)obj.asset;
            }
        }

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="assetName"></param>
        private static async Task<AsyncOperation> LoadSceneAsync(string bundleName, string assetName)
        {
            LoadDependencies(bundleName);
            if (!bundles.TryGetValue(bundleName, out var assetBundle))
            {
                bundles.Add(bundleName, null);
                assetBundle = await LoadFromFileAsync(bundleName);
                if (assetBundle == null)
                {
                    bundles.Remove(bundleName);
                    Debug.LogWarning($"加载 {bundleName.Red()} 资源失败");
                    return null;
                }

                bundles[bundleName] = assetBundle;
            }

            return UnitySceneManager.LoadSceneAsync(assetName, LoadSceneMode.Single);
        }

        /// <summary>
        /// 同步加载AB包
        /// </summary>
        /// <param name="assetBundle"></param>
        /// <returns></returns>
        private static AssetBundle LoadFromFile(string assetBundle)
        {
            if (File.Exists(GlobalSetting.GetPersistentPath(assetBundle)))
            {
                return AssetBundle.LoadFromFile(GlobalSetting.GetPersistentPath(assetBundle));
            }

            if (File.Exists(GlobalSetting.GetStreamingPath(assetBundle)))
            {
                return AssetBundle.LoadFromFile(GlobalSetting.GetStreamingPath(assetBundle));
            }

            return null;
        }

        /// <summary>
        /// 异步加载AB包路径
        /// </summary>
        /// <param name="assetBundle"></param>
        /// <returns></returns>
        private static async Task<AssetBundle> LoadFromFileAsync(string assetBundle)
        {
            if (File.Exists(GlobalSetting.GetPersistentPath(assetBundle)))
            {
                var request = AssetBundle.LoadFromFileAsync(GlobalSetting.GetPersistentPath(assetBundle));
                while (!request.isDone && GlobalManager.Runtime)
                {
                    await Task.Yield();
                }

                return request.assetBundle;
            }

            if (File.Exists(GlobalSetting.GetStreamingPath(assetBundle)))
            {
                var request = AssetBundle.LoadFromFileAsync(GlobalSetting.GetStreamingPath(assetBundle));
                while (!request.isDone && GlobalManager.Runtime)
                {
                    await Task.Yield();
                }

                return request.assetBundle;
            }

            return null;
        }

        /// <summary>
        /// 卸载AB包的方法
        /// </summary>
        /// <param name="bundleName"></param>
        public static void Unload(string bundleName)
        {
            if (bundles.TryGetValue(bundleName, out var assetBundle))
            {
                assetBundle.Unload(false);
                foreach (var asset in assets.Values.Where(asset => asset.bundleName == bundleName))
                {
                    assets.Remove($"{asset.bundleName}/{asset.assetName}");
                }

                bundles.Remove(bundleName);
            }
        }

        /// <summary>
        /// 清空AB包的方法
        /// </summary>
        internal static void Clear()
        {
            AssetBundle.UnloadAllAssetBundles(true);
            assets.Clear();
            bundles.Clear();
            manifest = null;
            mainAsset = null;
        }
    }
}