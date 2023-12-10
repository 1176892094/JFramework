// *********************************************************************************
// # Project: Forest
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-12-03  18:32
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using JFramework.Interface;

namespace JFramework
{
    /// <summary>
    /// 数据池
    /// </summary>
    public static class StreamPool
    {
        /// <summary>
        /// 数据字典
        /// </summary>
        internal static readonly Dictionary<Type, IPool> streams = new Dictionary<Type, IPool>();

        /// <summary>
        /// 出流队列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Pop<T>()
        {
            if (streams.TryGetValue(typeof(T), out var stream) && stream.Count > 0)
            {
                return ((IPool<T>)stream).Pop();
            }

            return Activator.CreateInstance<T>();
        }

        /// <summary>
        /// 出流队列
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T Pop<T>(Type type)
        {
            if (streams.TryGetValue(type, out var stream) && stream.Count > 0)
            {
                return ((IPool<T>)stream).Pop();
            }

            return (T)Activator.CreateInstance(type);
        }

        /// <summary>
        /// 进流队列
        /// </summary>
        /// <param name="obj"></param>
        /// <typeparam name="T"></typeparam>
        public static void Push<T>(T obj)
        {
            if (streams.TryGetValue(typeof(T), out var pool))
            {
                ((IPool<T>)pool).Push(obj);
                return;
            }

            streams.Add(typeof(T), new Pool<T>(obj));
        }

        /// <summary>
        /// 进流队列
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        public static void Push<T>(T obj, Type type)
        {
            if (streams.TryGetValue(type, out var pool))
            {
                ((IPool<T>)pool).Push(obj);
                return;
            }

            streams.Add(type, new Pool<T>(obj));
        }

        /// <summary>
        /// 清理流数据
        /// </summary>
        internal static void Dispose()
        {
            streams.Clear();
        }
    }
}