// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-10-24  23:57
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

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
    internal readonly struct Pool<T> : IPool<T>
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
            return pool.TryPop(out var obj) ? obj : Activator.CreateInstance<T>();
        }

        /// <summary>
        /// 对象推入
        /// </summary>
        /// <param name="obj">推入对象</param>
        public bool Push(T obj)
        {
            if (!pool.Contains(obj))
            {
                pool.Push(obj);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 清空对象池
        /// </summary>
        public void Dispose() => pool.Clear();
    }
}