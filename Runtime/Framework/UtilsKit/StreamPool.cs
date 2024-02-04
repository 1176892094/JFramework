// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-02-04  14:48
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using JFramework.Interface;

namespace JFramework
{
    public static class StreamPool
    {
        private static readonly Dictionary<Type, IPool> streams = new Dictionary<Type, IPool>();

        public static T Pop<T>()
        {
            if (streams.TryGetValue(typeof(T), out var stream) && stream.Count > 0)
            {
                return ((IPool<T>)stream).Pop();
            }

            return Activator.CreateInstance<T>();
        }

        public static T Pop<T>(Type type)
        {
            if (streams.TryGetValue(type, out var stream) && stream.Count > 0)
            {
                return ((IPool<T>)stream).Pop();
            }

            return (T)Activator.CreateInstance(type);
        }

        public static void Push<T>(T obj)
        {
            if (streams.TryGetValue(typeof(T), out var pool))
            {
                ((IPool<T>)pool).Push(obj);
                return;
            }

            streams.Add(typeof(T), new Pool<T>(obj));
        }

        public static void Push<T>(T obj, Type type)
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