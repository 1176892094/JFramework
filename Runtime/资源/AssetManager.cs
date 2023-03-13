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
    public sealed class AssetManager : Singleton<AssetManager>
    {
        /// <summary>
        /// 资源存储字典
        /// </summary>
        private Dictionary<string, IEnumerator> assetDict;

        /// <summary>
        /// 资源管理器初始化
        /// </summary>
        internal override void Awake()
        {
            base.Awake();
            assetDict = new Dictionary<string, IEnumerator>();
        }

        /// <summary>
        /// 通过资源管理器加载资源 (同步)
        /// </summary>
        /// <param name="name">资源的名称</param>
        /// <typeparam name="T">可以使用任何继承Object的对象</typeparam>
        public T Load<T>(string name) where T : Object
        {
            if (assetDict == null)
            {
                Debug.Log("资源管理器没有初始化!");
                return null;
            }
            
            var result = Addressables.LoadAssetAsync<T>(name).WaitForCompletion();
            return result is GameObject ? Object.Instantiate(result) : result;
        }
        
        /// <summary>
        /// 通过资源加载管理器异步加载资源
        /// </summary>
        /// <param name="name">资源的名称</param>
        /// <param name="action">资源的回调函数</param>
        /// <typeparam name="T">可以使用任何继承Object的对象</typeparam>
        public void LoadAsync<T>(string name, Action<T> action) where T : Object
        {
            if (assetDict == null)
            {
                Debug.Log("资源管理器没有初始化!");
                return;
            }
            
            AsyncOperationHandle<T> handle;
            if (assetDict.ContainsKey(name))
            {
                handle = (AsyncOperationHandle<T>)assetDict[name];
                if (handle.IsDone)
                {
                    action(handle.Result is GameObject ? Object.Instantiate(handle.Result) : handle.Result);
                    if(DebugManager.Instance.isShowAsset)Debug.Log($"AssetManager创建 {name.Green()} 资源成功！");
                }
                else
                {
                    handle.Completed += obj =>
                    {
                        if (obj.Status == AsyncOperationStatus.Succeeded)
                        {
                            action(obj.Result is GameObject ? Object.Instantiate(obj.Result) : obj.Result);
                            if(DebugManager.Instance.isShowAsset)Debug.Log($"AssetManager创建 {name.Green()} 资源成功！");
                        }
                    };
                }

                return;
            }

            handle = Addressables.LoadAssetAsync<T>(name);
            handle.Completed += obj =>
            {
                if (obj.Status == AsyncOperationStatus.Succeeded)
                {
                    action(obj.Result is GameObject ? Object.Instantiate(obj.Result) : obj.Result);
                    if(DebugManager.Instance.isShowAsset)Debug.Log($"AssetManager加载 {name.Green()} 资源成功！");
                }
                else
                {
                    Debug.LogWarning($"资源 {name.Red()} 未获取到！");
                    if (assetDict.ContainsKey(name))
                    {
                        assetDict.Remove(name);
                    }
                }
            };
            assetDict.Add(name, handle);
        }

        /// <summary>
        /// 通过资源加载管理器释放资源
        /// </summary>
        /// <param name="name">资源的名称</param>
        /// <typeparam name="T">可以使用任何继承Object的对象</typeparam>
        public void Dispose<T>(string name)
        {
            if (assetDict == null)
            {
                Debug.Log("资源管理器没有初始化!");
                return;
            }
            
            if (!assetDict.ContainsKey(name)) return;
            var handle = (AsyncOperationHandle<T>)assetDict[name];
            Addressables.Release(handle);
            assetDict.Remove(name);
        }

        internal override void Destroy()
        {
            base.Destroy();
            assetDict = null;
        }
    }
}