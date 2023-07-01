using System;
using System.Collections.Generic;
using UnityEngine;

namespace JFramework
{
    /// <summary>
    /// 游戏物体对象池
    /// </summary>
    [Serializable]
    internal sealed class PoolData : Pool<GameObject>
    {
        /// <summary>
        /// 游戏物体组
        /// </summary>
        private readonly Transform transform;

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
        protected override GameObject Pop()
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
        /// <param name="gameObject">推出的游戏物体</param>
        protected override void Push(GameObject gameObject)
        {
            gameObject.SetActive(false);
            if (transform == null) return;
            gameObject.transform.SetParent(transform);
            if (stackPool.Contains(gameObject)) return;
            stackPool.Push(gameObject);
        }
    }
}