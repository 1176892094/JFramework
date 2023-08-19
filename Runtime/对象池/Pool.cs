using System;
using System.Collections.Generic;
using JFramework.Core;
using JFramework.Interface;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JFramework
{
    /// <summary>
    /// 游戏物体对象池
    /// </summary>
    [Serializable]
    internal readonly struct Pool : IPool<GameObject>
    {
        /// <summary>
        /// 对象池容器
        /// </summary>
        [ShowInInspector] private readonly HashSet<GameObject> pool;

        /// <summary>
        /// 游戏物体组
        /// </summary>
        private readonly GameObject parent;

        /// <summary>
        /// 对象池物体数量
        /// </summary>
        public int Count => pool.Count;

        /// <summary>
        /// 构造函数初始化数据
        /// </summary>
        /// <param name="obj">推入的游戏对象</param>
        public Pool(GameObject obj)
        {
            pool = new HashSet<GameObject>();
            parent = new GameObject(obj.name + "-Pool");
            parent.transform.SetParent(PoolManager.poolManager);
            Push(obj);
        }

        /// <summary>
        /// 对象池弹出对象
        /// </summary>
        /// <returns>返回拉取的游戏物体</returns>
        public GameObject Pop()
        {
            if (!pool.TryPop(out var obj)) return null;
            obj.SetActive(true);
            obj.transform.SetParent(null);
            obj.GetComponent<IPop>()?.OnPop();
            return obj;

        }

        /// <summary>
        /// 对象池推入对象
        /// </summary>
        /// <param name="obj">推出的游戏物体</param>
        public bool Push(GameObject obj)
        {
            if (!pool.Add(obj)) return false;
            obj.SetActive(false);
            obj.transform.SetParent(parent.transform);
            obj.GetComponent<IPush>()?.OnPush();
            return true;
        }

        /// <summary>
        /// 清空对象池
        /// </summary>
        public void Clear()
        {
            foreach (var obj in pool)
            {
                Push(obj);
            }

            pool.Clear();
        }
    }
}