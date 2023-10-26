// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-10-24  23:54
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        internal static readonly Dictionary<string, (string, string)> assets = new Dictionary<string, (string, string)>();

        /// <summary>
        /// 存储字典
        /// </summary>
        internal static readonly Dictionary<string, AssetBundle> depends = new Dictionary<string, AssetBundle>();

        /// <summary>
        /// 主包
        /// </summary>
        private static AssetBundle mainAsset;

        /// <summary>
        /// 声明文件
        /// </summary>
        private static AssetBundleManifest manifest;

        /// <summary>
        /// 加载主包 和 配置文件
        /// </summary>
        private static void LoadMainAssetBundle()
        {
            if (mainAsset != null) return;
            mainAsset = LoadFromFile($"{GlobalSetting.Instance.platform}");
            manifest = mainAsset.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }

        /// <summary>
        /// 加载指定包的依赖包
        /// </summary>
        /// <param name="bundleName"></param>
        private static void LoadDependencies(string bundleName)
        {
            LoadMainAssetBundle();
            var dependencies = manifest.GetAllDependencies(bundleName);
            foreach (var dependency in dependencies)
            {
                if (depends.ContainsKey(dependency)) continue;
                var assetBundle = LoadFromFile(dependency);
                depends.Add(dependency, assetBundle);
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
            if (!AssetSetting.Instance.isRemoteLoad) return AssetSetting.Instance.Load<T>(path);
#endif
            if (assets.TryGetValue(path, out var asset))
            {
                return Load<T>(asset.Item1, asset.Item2);
            }

            var array = path.Split('/');
            asset = (array[0].ToLower(), array[1]);
            assets.Add(path, asset);
            return Load<T>(asset.Item1, asset.Item2);
        }

        /// <summary>
        /// 根据路径加载
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Task<T> LoadAsync<T>(string path) where T : Object
        {
            if (!GlobalManager.Runtime) return null;
#if UNITY_EDITOR
            if (!AssetSetting.Instance.isRemoteLoad) return AssetSetting.Instance.LoadAsync<T>(path);
#endif
            if (assets.TryGetValue(path, out var asset))
            {
                return LoadAsync<T>(asset.Item1, asset.Item2);
            }

            var array = path.Split('/');
            asset = (array[0].ToLower(), array[1]);
            assets.Add(path, asset);
            return LoadAsync<T>(asset.Item1, asset.Item2);
        }

        /// <summary>
        /// 根据路径加载
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal static Task<AsyncOperation> LoadSceneAsync(string path)
        {
            if (!GlobalManager.Runtime) return null;
#if UNITY_EDITOR
            if (!AssetSetting.Instance.isRemoteLoad) return AssetSetting.Instance.LoadSceneAsync(path);
#endif
            if (assets.TryGetValue(path, out var asset))
            {
                return LoadSceneAsync(asset.Item1, asset.Item2);
            }

            var array = path.Split('/');
            asset = (array[0].ToLower(), array[1]);
            assets.Add(path, asset);
            return LoadSceneAsync(asset.Item1, asset.Item2);
        }

        /// <summary>
        /// 泛型资源同步加载
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static T Load<T>(string bundleName, string assetName) where T : Object
        {
            LoadDependencies(bundleName);
            if (!depends.TryGetValue(bundleName, out var assetBundle))
            {
                assetBundle = LoadFromFile(bundleName);
                if (assetBundle == null)
                {
                    Debug.LogWarning($"加载 {bundleName.Red()} 资源失败");
                    return null;
                }

                depends.Add(bundleName, assetBundle);
            }

            var obj = assetBundle.LoadAsset<T>(assetName);
            return obj is GameObject ? Object.Instantiate(obj) : obj;
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
            if (!depends.TryGetValue(bundleName, out var assetBundle))
            {
                assetBundle = await LoadFromFileAsync(bundleName);
                if (assetBundle == null)
                {
                    Debug.LogWarning($"加载 {bundleName.Red()} 资源失败");
                    return null;
                }

                depends.Add(bundleName, assetBundle);
            }

            var obj = await assetBundle.LoadAssetAsync<T>(assetName);
            return obj is GameObject ? (T)Object.Instantiate(obj) : (T)obj;
        }

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="sceneName"></param>
        private static async Task<AsyncOperation> LoadSceneAsync(string bundleName, string sceneName)
        {
            LoadDependencies(bundleName);
            if (!depends.TryGetValue(bundleName, out var assetBundle))
            {
                assetBundle = await LoadFromFileAsync(bundleName);
                if (assetBundle == null)
                {
                    Debug.LogWarning($"加载 {bundleName.Red()} 资源失败");
                    return null;
                }

                depends.Add(bundleName, assetBundle);
            }

            if (!GlobalManager.Runtime) return null;
            return UnitySceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
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
                return await AssetBundle.LoadFromFileAsync(GlobalSetting.GetPersistentPath(assetBundle));
            }

            if (File.Exists(GlobalSetting.GetStreamingPath(assetBundle)))
            {
                return await AssetBundle.LoadFromFileAsync(GlobalSetting.GetStreamingPath(assetBundle));
            }

            return null;
        }

        /// <summary>
        /// 卸载AB包的方法
        /// </summary>
        /// <param name="bundleName"></param>
        public static void Unload(string bundleName)
        {
            if (depends.TryGetValue(bundleName, out var assetBundle))
            {
                assetBundle.Unload(false);
                foreach (var (bundle, asset) in assets.Values)
                {
                    if (bundle == bundleName)
                    {
                        assets.Remove($"{bundle}/{asset}");
                    }
                }

                depends.Remove(bundleName);
            }
        }

        /// <summary>
        /// 清空AB包的方法
        /// </summary>
        internal static void Clear()
        {
            AssetBundle.UnloadAllAssetBundles(true);
            assets.Clear();
            depends.Clear();
            manifest = null;
            mainAsset = null;
        }
    }
}