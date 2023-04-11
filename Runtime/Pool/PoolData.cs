using System;
using System.Collections.Generic;
using JFramework.Core;
using Sirenix.OdinInspector;
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
        [ShowInInspector, LabelText("对象池组")] private readonly Transform pool;

        /// <summary>
        /// 游戏物体栈
        /// </summary>
        [ShowInInspector, LabelText("对象池栈")] private readonly Stack<GameObject> stack;

        /// <summary>
        /// 栈中对象数量
        /// </summary>
        public override int Count => stack.Count;

        /// <summary>
        /// 构造函数初始化数据
        /// </summary>
        /// <param name="pool">推入的游戏对象</param>
        /// <param name="parent">池中的游戏对象栈</param>
        public PoolData(GameObject pool, Transform parent)
        {
            stack = new Stack<GameObject>();
            this.pool = new GameObject(pool.name + "Pool").transform;
            this.pool.SetParent(parent);
            Push(pool);
        }

        /// <summary>
        /// 对象池拉取对象
        /// </summary>
        /// <returns>返回拉取的游戏物体</returns>
        protected override GameObject Pop()
        {
            GameObject obj = stack.Pop();
            if (obj == null) return null;
            obj.transform.SetParent(null);
            obj.SetActive(true);
            return obj;
        }

        /// <summary>
        /// 对象池推入对象
        /// </summary>
        /// <param name="obj">推出的游戏物体</param>
        protected override void Push(GameObject obj)
        {
            stack.Push(obj);
            if (pool != null) obj.transform.SetParent(pool.transform);
            obj.SetActive(false);
        }
    }
}