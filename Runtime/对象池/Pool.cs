using System;
using System.Collections.Generic;
using JFramework.Interface;
using Sirenix.OdinInspector;

namespace JFramework
{
    [Serializable]
    internal sealed class Pool<T> : IPool<T> where T : new()
    {
        /// <summary>
        /// 静态对象池
        /// </summary>
        [ShowInInspector] internal readonly Stack<T> stackPool = new Stack<T>();

        /// <summary>
        /// 对象数量
        /// </summary>
        public int Count => stackPool.Count;

        /// <summary>
        /// 创建时推入对象
        /// </summary>
        /// <param name="obj">传入泛型对象</param>
        public Pool(T obj) => Push(obj);

        /// <summary>
        /// 对象弹出
        /// </summary>
        /// <returns>返回对象</returns>
        public T Pop()
        {
            return stackPool.Count > 0 ? stackPool.Pop() : new T();
        }

        /// <summary>
        /// 对象推入
        /// </summary>
        /// <param name="obj">推入对象</param>
        public void Push(T obj)
        {
            if (!stackPool.Contains(obj))
            {
                stackPool.Push(obj);
            }
        }

        /// <summary>
        /// 清空对象池
        /// </summary>
        public void Clear() => stackPool.Clear();
    }
}