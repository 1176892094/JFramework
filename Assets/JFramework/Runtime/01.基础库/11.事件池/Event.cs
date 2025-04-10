// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-09 16:01:50
// # Recently: 2025-01-11 18:01:35
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;

namespace JFramework.Common
{
    public static partial class EventManager
    {
        private static readonly Dictionary<Type, IPool> poolData = new Dictionary<Type, IPool>();

        public static void Listen<T>(IEvent<T> data) where T : struct, IEvent
        {
            LoadPool<T>().Listen(data);
        }

        public static void Remove<T>(IEvent<T> data) where T : struct, IEvent
        {
            LoadPool<T>().Remove(data);
        }
        
        public static void Invoke<T>(T data) where T : struct, IEvent
        {
            LoadPool<T>().Invoke(data);
        }

        private static EventPool<T> LoadPool<T>() where T : struct, IEvent
        {
            if (poolData.TryGetValue(typeof(T), out var pool))
            {
                return (EventPool<T>)pool;
            }

            pool = new EventPool<T>(typeof(T));
            poolData.Add(typeof(T), pool);
            return (EventPool<T>)pool;
        }

        internal static Reference[] Reference()
        {
            var index = 0;
            var results = new Reference[poolData.Count];
            foreach (var value in poolData.Values)
            {
                results[index++] = new Reference(value.type, value.path, value.acquire, value.release, value.dequeue, value.enqueue);
            }

            return results;
        }

        internal static void Dispose()
        {
            var poolCaches = new List<Type>(poolData.Keys);
            foreach (var cache in poolCaches)
            {
                if (poolData.TryGetValue(cache, out var pool))
                {
                    pool.Dispose();
                    poolData.Remove(cache);
                }
            }

            poolData.Clear();
        }
    }
}