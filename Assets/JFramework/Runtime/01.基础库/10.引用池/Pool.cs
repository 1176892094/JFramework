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
    public static partial class PoolManager
    {
        private static readonly Dictionary<Type, IPool> poolData = new Dictionary<Type, IPool>();

        public static T Dequeue<T>()
        {
            return LoadPool<T>(typeof(T)).Dequeue();
        }

        public static T Dequeue<T>(Type heapType)
        {
            return LoadPool<T>(heapType).Dequeue();
        }

        public static void Enqueue<T>(T heapData)
        {
            LoadPool<T>(typeof(T)).Enqueue(heapData);
        }

        public static void Enqueue<T>(T heapData, Type heapType)
        {
            LoadPool<T>(heapType).Enqueue(heapData);
        }

        private static Pool<T> LoadPool<T>(Type heapType)
        {
            if (poolData.TryGetValue(heapType, out var pool))
            {
                return (Pool<T>)pool;
            }

            pool = new Pool<T>(heapType);
            poolData.Add(heapType, pool);
            return (Pool<T>)pool;
        }

        internal static Reference[] Reference()
        {
            var index = 0;
            var results = new Reference[poolData.Count];
            foreach (var value in poolData.Values)
            {
                var assetType = value.assetType;
                var assetPath = value.assetPath;
                results[index++] = new Reference(assetType, assetPath, value.acquire, value.release, value.dequeue, value.enqueue);
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