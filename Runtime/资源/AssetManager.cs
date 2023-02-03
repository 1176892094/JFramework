using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace JFramework.Core
{
    /// <summary>
    /// 资源管理器
    /// </summary>
    public class AssetManager : Singleton<AssetManager>
    {
        /// <summary>
        /// 存储资源的字典
        /// </summary>
        [ShowInInspector, ReadOnly, LabelText("资源管理器"), FoldoutGroup("资源加载视图")]
        private Dictionary<string, IEnumerator> assetDict;

        /// <summary>
        /// 资源管理器初始化
        /// </summary>
        protected override void OnInit(params object[] args)
        {
            assetDict = new Dictionary<string, IEnumerator>();
        }


        /// <summary>
        /// 通过资源加载管理器异步加载资源
        /// </summary>
        /// <param name="name">资源的名称</param>
        /// <param name="action">资源的回调函数</param>
        /// <typeparam name="T">可以使用任何继承Object的对象</typeparam>
        public void LoadAsync<T>(string name, Action<T> action) where T : Object
        {
            AsyncOperationHandle<T> handle;
            if (assetDict.ContainsKey(name))
            {
                handle = (AsyncOperationHandle<T>)assetDict[name];
                if (handle.IsDone)
                {
                    action(handle.Result is GameObject ? Instantiate(handle.Result) : handle.Result);
                }
                else
                {
                    handle.Completed += obj =>
                    {
                        if (obj.Status == AsyncOperationStatus.Succeeded)
                        {
                            action(obj.Result is GameObject ? Instantiate(obj.Result) : obj.Result);
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
                    action(obj.Result is GameObject ? Instantiate(obj.Result) : obj.Result);
                }
                else
                {
                    Debug.LogWarning(name + "未获取到！");
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
            if (!assetDict.ContainsKey(name)) return;
            var handle = (AsyncOperationHandle<T>)assetDict[name];
            Addressables.Release(handle);
            assetDict.Remove(name);
        }
    }
}