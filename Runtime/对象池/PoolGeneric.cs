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
    internal sealed class Pool<T> : IPool<T> where T : new()
    {
        /// <summary>
        /// 静态对象池
        /// </summary>
        [ShowInInspector] private readonly HashSet<T> pool = new HashSet<T>();

        /// <summary>
        /// 对象数量
        /// </summary>
        public int Count => pool.Count;

        /// <summary>
        /// 创建时推入对象
        /// </summary>
        /// <param name="object">传入泛型对象</param>
        public Pool(T @object) => Push(@object);

        /// <summary>
        /// 对象弹出
        /// </summary>
        /// <returns>返回对象</returns>
        public T Pop() => pool.TryPop(out var @object) ? @object : new T();

        /// <summary>
        /// 对象推入
        /// </summary>
        /// <param name="object">推入对象</param>
        public bool Push(T @object) => pool.Add(@object);

        /// <summary>
        /// 清空对象池
        /// </summary>
        public void Clear() => pool.Clear();
    }
}