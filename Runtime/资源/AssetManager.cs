using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace JFramework.Core
{
    /// <summary>
    /// 资源管理器
    /// </summary>
    public static class AssetManager
    {
        /// <summary>
        /// 资源存储字典
        /// </summary>
        internal static Dictionary<string, IEnumerator> assetDict;

        /// <summary>
        /// 资源管理器初始化
        /// </summary>
        internal static void Awake() => assetDict = new Dictionary<string, IEnumerator>();

        /// <summary>
        /// 通过资源管理器加载资源 (同步)
        /// </summary>
        /// <param name="path">资源的路径</param>
        /// <typeparam name="T">可以使用任何继承Object的对象</typeparam>
        public static T Load<T>(string path) where T : Object
        {
            if (!GlobalManager.Runtime) return null;
            var result = Addressables.LoadAssetAsync<T>(path).WaitForCompletion();
            if (result == null)
            {
                Debug.Log($"{nameof(AssetManager).Sky()} 加载 => {path.Red()} 资源失败");
                return null;
            }

            return LoadSuccess(result);
        }

        /// <summary>
        /// 通过资源加载管理器异步加载资源
        /// </summary>
        /// <param name="path">资源的路径</param>
        /// <typeparam name="T">可以使用任何继承Object的对象</typeparam>
        /// <returns>返回资源的任务</returns>
        public static async Task<T> LoadAsync<T>(string path) where T : Object
        {
            if (!GlobalManager.Runtime) return null;
            AsyncOperationHandle<T> handle;
            if (assetDict.ContainsKey(path))
            {
                handle = (AsyncOperationHandle<T>)assetDict[path];
                if (!handle.IsDone) await handle.Task;
                return LoadSuccess(handle.Result);
            }

            handle = Addressables.LoadAssetAsync<T>(path);
            assetDict.Add(path, handle);
            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                return LoadSuccess(handle.Result);
            }

            if (assetDict.ContainsKey(path)) //资源加载失败
            {
                assetDict.Remove(path);
            }

            Debug.Log($"{nameof(AssetManager).Sky()} 加载 => {path.Red()} 资源失败");
            return null;
        }

        private static T LoadSuccess<T>(T result) where T : Object
        {
            GlobalManager.Logger(DebugOption.Asset, $"加载 => {result.name.Green()} 资源成功");
            return result is GameObject ? Object.Instantiate(result) : result;
        }

        /// <summary>
        /// 通过资源加载管理器释放资源
        /// </summary>
        /// <param name="name">资源的名称</param>
        /// <typeparam name="T">可以使用任何继承Object的对象</typeparam>
        public static void Dispose<T>(string name)
        {
            if (!GlobalManager.Runtime) return;
            if (!assetDict.ContainsKey(name)) return;
            var handle = (AsyncOperationHandle<T>)assetDict[name];
            Addressables.Release(handle);
            assetDict.Remove(name);
        }

        /// <summary>
        /// 清空管理器
        /// </summary>
        internal static void Destroy() => assetDict = null;
    }
}