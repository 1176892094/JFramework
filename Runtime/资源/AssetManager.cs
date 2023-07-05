using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace JFramework.Core
{
    public static class AssetManager
    {
        internal static readonly Dictionary<string, AsyncOperationHandle> assetDict = new Dictionary<string, AsyncOperationHandle>();

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

            return LoadCompleted(result);
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
                handle = assetDict[path].Convert<T>();
                if (!handle.IsDone) await handle.Task;
                return LoadCompleted(handle.Result);
            }

            handle = Addressables.LoadAssetAsync<T>(path);
            assetDict.Add(path, handle);
            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                return LoadCompleted(handle.Result);
            }

            if (assetDict.ContainsKey(path)) //资源加载失败
            {
                assetDict.Remove(path);
            }

            Debug.Log($"{nameof(AssetManager).Sky()} 加载 => {path.Red()} 资源失败");
            return null;
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
            var handle = assetDict[name].Convert<T>();
            Addressables.Release(handle);
            assetDict.Remove(name);
        }

        /// <summary>
        /// 释放所有资源，并开启垃圾回收
        /// </summary>
        public static void Clear()
        {
            foreach (var handle in assetDict.Values)
            {
                Addressables.Release(handle);
            }
            
            assetDict.Clear();
            AssetBundle.UnloadAllAssetBundles(true);
            Resources.UnloadUnusedAssets();
            GC.Collect();
        }
        
        /// <summary>
        /// 资源加载完成
        /// </summary>
        /// <param name="result">传入返回的结果</param>
        /// <typeparam name="T">可以使用任何继承Object的对象</typeparam>
        /// <returns>返回资源的类型</returns>
        private static T LoadCompleted<T>(T result) where T : Object
        {
            Log.Info(DebugOption.Asset, $"加载 => {result.name.Green()} 资源成功");
            return result is GameObject ? Object.Instantiate(result) : result;
        }
        
        /// <summary>
        /// 管理器销毁
        /// </summary>
        internal static void Destroy() => assetDict.Clear();
    }
}