using System;
using System.Collections.Generic;
using JFramework.Interface;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JFramework
{
    /// <summary>
    /// 游戏物体对象池
    /// </summary>
    [Serializable]
    internal sealed class PoolData : IPool<GameObject>
    {
        /// <summary>
        /// 对象池容器
        /// </summary>
        [ShowInInspector] private Stack<GameObject> stackPool;
        
        /// <summary>
        /// 游戏物体组
        /// </summary>
        private readonly Transform transform;

        /// <summary>
        /// 对象池物体数量
        /// </summary>
        public int Count => stackPool.Count;

        /// <summary>
        /// 构造函数初始化数据
        /// </summary>
        /// <param name="pool">推入的游戏对象</param>
        /// <param name="parent">池中的游戏对象栈</param>
        public PoolData(GameObject pool, Transform parent)
        {
            stackPool = new Stack<GameObject>();
            transform = new GameObject(pool.name + "-Pool").transform;
            transform.SetParent(parent);
            Push(pool);
        }

        /// <summary>
        /// 对象池弹出对象
        /// </summary>
        /// <returns>返回拉取的游戏物体</returns>
        public GameObject Pop()
        {
            var gameObject = stackPool.Count > 0 ? stackPool.Pop() : null;
            if (gameObject == null) return null;
            gameObject.transform.SetParent(null);
            gameObject.SetActive(true);
            return gameObject;
        }

        /// <summary>
        /// 对象池推入对象
        /// </summary>
        /// <param name="obj">推出的游戏物体</param>
        public void Push(GameObject obj)
        {
            obj.SetActive(false);
            if (transform == null) return;
            obj.transform.SetParent(transform);
            if (stackPool.Contains(obj)) return;
            stackPool.Push(obj);
        }

        /// <summary>
        /// 清空对象池
        /// </summary>
        public void Clear()
        {
            foreach (var obj in stackPool)
            {
                Push(obj);
            }
            
            stackPool.Clear();
        }
    }
}