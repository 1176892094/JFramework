using System;
using System.Collections.Generic;
using JFramework.Interface;
using JFramework;
using UnityEngine;

namespace JFramework.Core
{
    public static class PoolManager
    {
        /// <summary>
        /// 存储所有池的字典
        /// </summary>
        internal static Dictionary<string, IPool> poolDict;

        /// <summary>
        /// 对象池管理器
        /// </summary>
        private static string Name => nameof(PoolManager);

        /// <summary>
        /// 对象池控制器
        /// </summary>
        internal static GameObject poolManager;

        /// <summary>
        /// 对象池管理器初始化
        /// </summary>
        internal static void Awake()
        {
            poolDict = new Dictionary<string, IPool>();
            var transform = GlobalManager.Instance.transform;
            poolManager = transform.Find("PoolManager").gameObject;
        }

        /// <summary>
        /// 对象池管理器拉取对象
        /// </summary>
        /// <param name="key">拉取对象的名称</param>
        /// <param name="action">拉取对象的回调</param>
        public static void Pop(string key, Action<GameObject> action)
        {
            if (poolDict == null)
            {
                Debug.Log($"{Name.Red()} 没有初始化");
                return;
            }

            if (poolDict.ContainsKey(key) && poolDict[key].Count > 0)
            {
                var obj = (GameObject)poolDict[key].Pop();
                if (obj != null)
                {
                    if (GlobalManager.Instance.IsDebugPool)
                    {
                        Debug.Log($"{Name.Sky()} 取出 => {key.Pink()} 对象成功");
                    }
                    
                    action?.Invoke(obj);
                    return;
                }
                
                if (GlobalManager.Instance.IsDebugPool)
                {
                    Debug.Log($"{Name.Sky()} 移除已销毁对象 : {key.Red()}");
                }
            }

            if (GlobalManager.Instance.IsDebugPool)
            {
                Debug.Log($"{Name.Sky()} 创建 => {key.Green()} 对象成功");
            }

            AssetManager.LoadAsync<GameObject>(key, o =>
            {
                o.name = key;
                action?.Invoke(o);
            });
        }

        /// <summary>
        /// 对象池管理器推入对象
        /// </summary>
        /// <param name="obj">对象的实例</param>
        public static void Push(GameObject obj)
        {
            if (poolDict == null)
            {
                Debug.Log($"{Name.Red()} 没有初始化");
                return;
            }

            if (obj == null) return;
            var key = obj.name;

            if (poolDict.ContainsKey(key))
            {
                if (obj == null)
                {
                    Debug.Log($"{Name.Sky()} 移除已销毁对象 : {key.Red()}");
                    poolDict[key].Pop();
                    return;
                }

                if (GlobalManager.Instance.IsDebugPool)
                {
                    Debug.Log($"{Name.Sky()} 存入 => {key.Pink()} 对象成功");
                }

                poolDict[key].Push(obj);
            }
            else
            {
                if (GlobalManager.Instance.IsDebugPool)
                {
                    Debug.Log($"{Name.Sky()} => 创建对象池 : {key.Green()}");
                }
                
                poolDict.Add(key, new PoolData(obj, poolManager));
            }
        }

        internal static void Destroy()
        {
            poolManager = null;
            poolDict = null;
        }
    }
}