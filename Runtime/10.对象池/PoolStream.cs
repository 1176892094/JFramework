// *********************************************************************************
// # Project: Forest
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-08-25  01:08
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using JFramework.Interface;

namespace JFramework
{
    public static partial class PoolManager
    {
        private static readonly Dictionary<Type, IPool> streams = new();

        public static T Dequeue<T>()
        {
            if (streams.TryGetValue(typeof(T), out var stream) && stream.count > 0)
            {
                return ((IPool<T>)stream).Pop();
            }

            return Activator.CreateInstance<T>();
        }

        public static T Dequeue<T>(Type type)
        {
            if (streams.TryGetValue(type, out var stream) && stream.count > 0)
            {
                return ((IPool<T>)stream).Pop();
            }

            return (T)Activator.CreateInstance(type);
        }

        public static void Enqueue<T>(T obj)
        {
            if (streams.TryGetValue(typeof(T), out var pool))
            {
                ((IPool<T>)pool).Push(obj);
                return;
            }

            streams.Add(typeof(T), new Pool<T>(obj));
        }

        public static void Enqueue<T>(T obj, Type type)
        {
            if (streams.TryGetValue(type, out var pool))
            {
                ((IPool<T>)pool).Push(obj);
                return;
            }

            streams.Add(type, new Pool<T>(obj));
        }
    }
}