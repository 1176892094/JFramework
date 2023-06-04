using System;
using System.Collections;
using System.Collections.Generic;
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
        /// 管理器名称
        /// </summary>
        private static string Name => nameof(AssetManager);

        /// <summary>
        /// 资源管理器初始化
        /// </summary>
        internal static void Awake() => assetDict = new Dictionary<string, IEnumerator>();

        /// <summary>
        /// 通过资源管理器加载资源 (同步)
        /// </summary>
        /// <param name="name">资源的名称</param>
        /// <typeparam name="T">可以使用任何继承Object的对象</typeparam>
        public static T Load<T>(string name) where T : Object
        {
            if (!GlobalManager.Runtime) return null;
            var result = Addressables.LoadAssetAsync<T>(name).WaitForCompletion();
            if (result == null)
            {
                Debug.Log($"{Name.Sky()} 加载 => {name.Red()} 资源失败");
                return null;
            }

            GlobalManager.Logger(DebugOption.Asset, $"加载 => {result.name.Green()} 资源成功");
            return result is GameObject ? Object.Instantiate(result) : result;
        }

        /// <summary>
        /// 通过资源加载管理器异步加载资源
        /// </summary>
        /// <param name="name">资源的名称</param>
        /// <param name="action">资源的回调函数</param>
        /// <typeparam name="T">可以使用任何继承Object的对象</typeparam>
        public static void LoadAsync<T>(string name, Action<T> action) where T : Object
        {
            if (!GlobalManager.Runtime) return;
            AsyncOperationHandle<T> handle;
            if (assetDict.ContainsKey(name))
            {
                handle = (AsyncOperationHandle<T>)assetDict[name];
                if (handle.IsDone)
                {
                    LoadSuccess(handle.Result, action);
                }
                else
                {
                    handle.Completed -= LoadCompleted;
                    handle.Completed += LoadCompleted;
                }

                return;
            }

            handle = Addressables.LoadAssetAsync<T>(name);
            handle.Completed += LoadCompleted;
            assetDict.Add(name, handle);
            
            void LoadCompleted(AsyncOperationHandle<T> obj)
            {
                if (obj.Status == AsyncOperationStatus.Succeeded)
                {
                    LoadSuccess(obj.Result, action);
                }
                else
                {
                    if (assetDict.ContainsKey(name))
                    {
                        assetDict.Remove(name);
                    }

                    Debug.Log($"{Name.Sky()} 加载 => {name.Red()} 资源失败");
                }
            }
            
            void LoadSuccess(T result, Action<T> callback)
            {
                GlobalManager.Logger(DebugOption.Asset, $"加载 => {result.name.Green()} 资源成功");
                callback(result is GameObject ? Object.Instantiate(result) : result);
            }
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