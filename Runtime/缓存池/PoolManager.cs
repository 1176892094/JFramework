using System;
using System.Collections.Generic;
using JFramework.Core;
using JFramework.Interface;
using UnityEngine;

namespace JFramework
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
        internal static GameObject manager;

        /// <summary>
        /// 对象池管理器初始化
        /// </summary>
        internal static void Awake() => poolDict = new Dictionary<string, IPool>();

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
                    action?.Invoke(obj);
                    return;
                }

                poolDict.Remove(key);
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
            if (manager == null)
            {
                manager = new GameObject(Name);
            }
            var key = obj.name;
            if (poolDict.ContainsKey(key))
            {
                if (obj == null)
                {
                    Debug.Log($"{Name.Sky()} <= Push => {key} Failure");
                    poolDict[key].Pop();
                    return;
                }

                poolDict[key].Push(obj);
            }
            else
            {
                poolDict.Add(key, new PoolData(obj, manager));
            }
        }
        
        internal static void Destroy()
        {
            manager = null;
            poolDict = null;
        }
    }
}