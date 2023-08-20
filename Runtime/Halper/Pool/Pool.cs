using System;
using System.Collections.Generic;
using JFramework.Interface;
using Sirenix.OdinInspector;

namespace JFramework
{
    /// <summary>
    /// 泛型对象池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    internal readonly struct Pool<T> : IPool<T> where T : new()
    {
        /// <summary>
        /// 静态对象池
        /// </summary>
        [ShowInInspector] private readonly Stack<T> pool;

        /// <summary>
        /// 对象数量
        /// </summary>
        public int Count => pool.Count;

        /// <summary>
        /// 创建时推入对象
        /// </summary>
        /// <param name="obj">传入泛型对象</param>
        public Pool(T obj)
        {
            pool = new Stack<T>();
            pool.Push(obj);
        }

        /// <summary>
        /// 对象弹出
        /// </summary>
        /// <returns>返回对象</returns>
        public T Pop()
        {
            return pool.TryPop(out var obj) ? obj : new T();
        }

        /// <summary>
        /// 对象推入
        /// </summary>
        /// <param name="obj">推入对象</param>
        public void Push(T obj)
        {
            if (!pool.Contains(obj))
            {
                pool.Push(obj);
            }
        }

        /// <summary>
        /// 清空对象池
        /// </summary>
        public void Clear() => pool.Clear();
    }
}