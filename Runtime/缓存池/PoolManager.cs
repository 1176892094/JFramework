using System;
using System.Collections.Generic;
using JFramework.Core;
using JFramework.Interface;
using UnityEngine;

namespace JFramework
{
    public class PoolManager : Singleton<PoolManager>
    {
        /// <summary>
        /// 存储所有池的字典
        /// </summary>
        internal Dictionary<string, IPool> poolDict;

        /// <summary>
        /// 对象池控制器
        /// </summary>
        internal GameObject manager;

        /// <summary>
        /// 对象池管理器初始化
        /// </summary>
        public override void Awake()
        {
            poolDict = new Dictionary<string, IPool>();
        }

        /// <summary>
        /// 对象池管理器拉取对象
        /// </summary>
        /// <param name="key">拉取对象的名称</param>
        /// <param name="action">拉取对象的回调</param>
        public void Pop(string key, Action<GameObject> action)
        {
            if (poolDict == null)
            {
                Debug.Log("对象池管理器没有初始化!");
                return;
            }
            
            if (poolDict.ContainsKey(key) && poolDict[key].Count > 0)
            {
                var obj = (GameObject)poolDict[key].Pop();
                if (obj != null)
                {
                    action?.Invoke(obj);
                }
                else
                {
                    poolDict.Remove(key);
                    AssetManager.Instance.LoadAsync<GameObject>(key, o =>
                    {
                        o.name = key;
                        action?.Invoke(o);
                    });
                }
            }
            else
            {
                AssetManager.Instance.LoadAsync<GameObject>(key, o =>
                {
                    o.name = key;
                    action?.Invoke(o);
                });
            }
        }

        /// <summary>
        /// 对象池管理器推入对象
        /// </summary>
        /// <param name="obj">对象的实例</param>
        public void Push(GameObject obj)
        {
            if (poolDict == null)
            {
                Debug.Log("对象池管理器没有初始化!");
                return;
            }
            
            if (obj == null) return;
            string key = obj.name;
            if (manager == null)
            {
                manager = new GameObject(nameof(PoolManager));
            }

            if (poolDict.ContainsKey(key))
            {
                if (obj == null)
                {
                    Debug.LogWarning(key + "已被销毁,无法推入对象！");
                    poolDict[key].Pop();
                    return;
                }

                poolDict[key].Push(obj);
            }
            else
            {
                var pool = new PoolData();
                pool.Awake(obj, manager);
                poolDict.Add(key, pool);
            }
        }

        public override void Destroy()
        {
            base.Destroy();
            manager = null;
            poolDict = null;
        }
    }
}