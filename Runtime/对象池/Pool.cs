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
    internal sealed class Pool : IPool<GameObject>
    {
        /// <summary>
        /// 对象池容器
        /// </summary>
        [ShowInInspector] private readonly HashSet<GameObject> pool = new HashSet<GameObject>();

        /// <summary>
        /// 游戏物体组
        /// </summary>
        private GameObject parent;

        /// <summary>
        /// 对象池物体数量
        /// </summary>
        public int Count => pool.Count;

        /// <summary>
        /// 构造函数初始化数据
        /// </summary>
        /// <param name="object">推入的游戏对象</param>
        public Pool(GameObject @object)
        {
            parent = new GameObject(@object.name + "-Pool");
            parent.transform.SetParent(PoolManager.poolManager);
            Push(@object);
        }

        /// <summary>
        /// 对象池弹出对象
        /// </summary>
        /// <returns>返回拉取的游戏物体</returns>
        public GameObject Pop()
        {
            if (!pool.TryPop(out var @object)) return null;
            @object.SetActive(true);
            @object.transform.SetParent(null);
            @object.GetComponent<IPop>()?.OnPop();
            return @object;

        }

        /// <summary>
        /// 对象池推入对象
        /// </summary>
        /// <param name="object">推出的游戏物体</param>
        public bool Push(GameObject @object)
        {
            if (!pool.Add(@object)) return false;
            @object.SetActive(false);
            @object.transform.SetParent(parent.transform);
            @object.GetComponent<IPush>()?.OnPush();
            return true;
        }

        /// <summary>
        /// 清空对象池
        /// </summary>
        public void Clear()
        {
            foreach (var @object in pool)
            {
                Push(@object);
            }

            pool.Clear();
        }
    }
}