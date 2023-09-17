using System;
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
        /// 资源加载进度
        /// </summary>
        public static event Action<AssetProgress> OnLoadProgress; 
        
#if UNITY_EDITOR
        /// <summary>
        /// 是否为远端加载
        /// </summary>
        public static bool isRemote => GlobalManager.Instance.isRemote;
#endif
        
        /// <summary>
        /// 加载主包 和 配置文件
        /// </summary>
        private static void LoadMainAssetBundle()
        {
            if (mainAsset != null) return;
            mainAsset = LoadFromFile($"{GlobalSetting.PLATFORM}");
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
        /// 从服务器下载资源包
        /// </summary>
        public static async void LoadAssetBundle() => await AssetHelper.UpdateAssetBundles();

        /// <summary>
        /// 下载资源包进度
        /// </summary>
        /// <param name="progress"></param>
        internal static void LoadProgress(AssetProgress progress) => OnLoadProgress?.Invoke(progress);

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
            if (!isRemote)
            {
                if (GlobalEditor.objects.TryGetValue(path, out var obj))
                {
                    if (obj is Texture2D texture)
                    {
                        obj = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                        return (T)obj;
                    }
                    
                    return obj is GameObject ? Object.Instantiate((T)obj) : (T)obj;
                }

                return null;
            }
#endif
            if (assets.ContainsKey(path))
            {
                return Load<T>(assets[path].Item1.ToLower(), assets[path].Item2);
            }

            var array = path.Split('/');
            assets.Add(path, (array[0].ToLower(), array[1]));
            return Load<T>(array[0].ToLower(), array[1]);
        }

        /// <summary>
        /// 泛型资源同步加载
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static T Load<T>(string bundleName, string assetName) where T : Object
        {
            LoadDependencies(bundleName);
            if (!depends.ContainsKey(bundleName))
            {
                var assetBundle = LoadFromFile(bundleName);
                if (assetBundle == null)
                {
                    Debug.LogWarning($"加载 {bundleName.Red()} 资源失败");
                    return null;
                }

                depends.Add(bundleName, assetBundle);
            }

            var obj = depends[bundleName].LoadAsset<T>(assetName);
            return obj is GameObject ? Object.Instantiate(obj) : obj;
        }

        /// <summary>
        /// 同步加载AB包
        /// </summary>
        /// <param name="assetBundle"></param>
        /// <returns></returns>
        private static AssetBundle LoadFromFile(string assetBundle)
        {
            var path = GlobalSetting.GetPerFile(assetBundle);
            if (File.Exists(path))
            {
                return AssetBundle.LoadFromFile(path);
            }

            path = GlobalSetting.GetStrFile(assetBundle);
            if (File.Exists(path))
            {
                return AssetBundle.LoadFromFile(path);
            }

            return null;
        }

        /// <summary>
        /// 根据路径加载
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Task<T> LoadAsync<T>(string path) where T : Object
        {
#if UNITY_EDITOR
            if (!isRemote)
            {
                return LoadAsyncYield();

                async Task<T> LoadAsyncYield()
                {
                    await Task.Yield();
                    return Load<T>(path);
                }
            }
#endif
            if (assets.ContainsKey(path))
            {
                return LoadAsync<T>(assets[path].Item1.ToLower(), assets[path].Item2);
            }

            var array = path.Split('/');
            assets.Add(path, (array[0].ToLower(), array[1]));
            return LoadAsync<T>(array[0].ToLower(), array[1]);
        }

        /// <summary>
        /// 泛型异步加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bundleName"></param>
        /// <param name="assetName"></param>
        private static async Task<T> LoadAsync<T>(string bundleName, string assetName) where T : Object
        {
            if (!GlobalManager.Runtime) return null;
            LoadDependencies(bundleName);
            if (!depends.ContainsKey(bundleName))
            {
                var assetBundle = await LoadFromFileAsync(bundleName);
                if (assetBundle == null)
                {
                    Debug.LogWarning($"加载 {bundleName.Red()} 资源失败");
                    return null;
                }

                depends.Add(bundleName, assetBundle);
            }

            var request = depends[bundleName].LoadAssetAsync<T>(assetName);
            if (!request.isDone && GlobalManager.Runtime)
            {
                await Task.Yield();
            }

            if (request.asset == null) return null;
            return request.asset is GameObject ? (T)Object.Instantiate(request.asset) : (T)request.asset;
        }

        /// <summary>
        /// 异步加载AB包路径
        /// </summary>
        /// <param name="assetBundle"></param>
        /// <returns></returns>
        private static async Task<AssetBundle> LoadFromFileAsync(string assetBundle)
        {
            var path = GlobalSetting.GetPerFile(assetBundle);
            if (File.Exists(path))
            {
                var request = AssetBundle.LoadFromFileAsync(path);
                if (!request.isDone && GlobalManager.Runtime)
                {
                    await Task.Yield();
                }

                return request.assetBundle;
            }

            path = GlobalSetting.GetStrFile(assetBundle);
            if (File.Exists(path))
            {
                var request = AssetBundle.LoadFromFileAsync(path);
                if (!request.isDone && GlobalManager.Runtime)
                {
                    await Task.Yield();
                }

                return request.assetBundle;
            }

            return null;
        }
        
        /// <summary>
        /// 根据路径加载
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal static Task<AsyncOperation> LoadSceneAsync(string path)
        {
            if (assets.ContainsKey(path))
            {
                return LoadSceneAsync(assets[path].Item1.ToLower(), assets[path].Item2);
            }

            var array = path.Split('/');
            assets.Add(path, (array[0].ToLower(), array[1]));
            return LoadSceneAsync(array[0].ToLower(), array[1]);
        }

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="sceneName"></param>
        private static async Task<AsyncOperation> LoadSceneAsync(string bundleName, string sceneName)
        {
            if (!GlobalManager.Runtime) return null;
#if UNITY_EDITOR
            if (!isRemote)
            {
                if (!GlobalManager.Runtime) return null;
                return UnitySceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            }
#endif
            LoadDependencies(bundleName);
            if (!depends.ContainsKey(bundleName))
            {
                var assetBundle = await LoadFromFileAsync(bundleName);
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
        /// 卸载AB包的方法
        /// </summary>
        /// <param name="assetBundle"></param>
        public static void Unload(string assetBundle)
        {
            if (depends.ContainsKey(assetBundle))
            {
                depends[assetBundle].Unload(false);
                foreach (var (bundleName, assetName) in assets.Values)
                {
                    if (bundleName == assetBundle)
                    {
                        assets.Remove($"{assetBundle}/{assetName}");
                    }
                }

                depends.Remove(assetBundle);
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
            OnLoadProgress = null;
        }
    }
}