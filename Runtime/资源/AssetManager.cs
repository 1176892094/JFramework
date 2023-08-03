using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

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
        /// 从服务器下载资源包
        /// </summary>
        internal static async Task Awake()
        {
            await AssetHelper.UpdateAsync();
        }

        /// <summary>
        /// 加载主包 和 配置文件
        /// </summary>
        private static void LoadMainAssetBundle()
        {
            if (mainAsset != null) return;
            mainAsset = LoadFromFile($"{AssetSetting.PLATFORM}");
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
            if (!AssetEditor.isRemote)
            {
                var obj = AssetEditor.objects[path];
                return obj is GameObject ? Object.Instantiate((T)obj) : (T)obj;
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
                    Debug.Log($"{nameof(AssetManager).Sky()} 加载 => {bundleName.Red()} 资源失败");
                    return null;
                }

                depends.Add(bundleName, assetBundle);
            }

            var obj = depends[bundleName].LoadAsset<T>(assetName);
            Log.Info(DebugOption.Asset, $"加载 => {obj.name.Green()} 资源成功");
            return obj is GameObject ? Object.Instantiate(obj) : obj;
        }

        /// <summary>
        /// 同步加载AB包
        /// </summary>
        /// <param name="assetBundle"></param>
        /// <returns></returns>
        private static AssetBundle LoadFromFile(string assetBundle)
        {
            var path = AssetSetting.GetPerFile(assetBundle);
            if (File.Exists(path))
            {
                return AssetBundle.LoadFromFile(path);
            }

            path = AssetSetting.GetStrFile(assetBundle);
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
            if (!AssetEditor.isRemote)
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
                    Debug.Log($"{nameof(AssetManager).Sky()} 加载 => {bundleName.Red()} 资源失败");
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
            Log.Info(DebugOption.Asset, $"加载 => {request.asset.name.Green()} 资源成功");
            return request.asset is GameObject ? (T)Object.Instantiate(request.asset) : (T)request.asset;
        }

        /// <summary>
        /// 异步加载AB包路径
        /// </summary>
        /// <param name="assetBundle"></param>
        /// <returns></returns>
        private static async Task<AssetBundle> LoadFromFileAsync(string assetBundle)
        {
            var path = AssetSetting.GetPerFile(assetBundle);
            if (File.Exists(path))
            {
                return await LoadFromFilePath(path);
            }

            path = AssetSetting.GetStrFile(assetBundle);
            if (File.Exists(path))
            {
                return await LoadFromFilePath(path);
            }

            return null;
        }

        /// <summary>
        /// 异步加载AB包
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        private static async Task<AssetBundle> LoadFromFilePath(string assetPath)
        {
            var request = AssetBundle.LoadFromFileAsync(assetPath);
            if (!request.isDone && GlobalManager.Runtime)
            {
                await Task.Yield();
            }

            return request.assetBundle;
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
            if (!AssetEditor.isRemote)
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
                    Debug.Log($"{nameof(AssetManager).Sky()} 加载 => {bundleName.Red()} 资源失败");
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
        internal static void RuntimeInitializeOnLoad()
        {
            AssetBundle.UnloadAllAssetBundles(true);
            depends.Clear();
            assets.Clear();
            manifest = null;
            mainAsset = null;
        }
    }
}