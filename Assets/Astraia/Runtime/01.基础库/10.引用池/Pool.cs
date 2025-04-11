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
    public static partial class HeapManager
    {
        private static readonly Dictionary<Type, IPool> poolData = new Dictionary<Type, IPool>();

        public static T Dequeue<T>()
        {
            return LoadPool<T>(typeof(T)).Dequeue();
        }

        public static T Dequeue<T>(Type type)
        {
            return LoadPool<T>(type).Dequeue();
        }

        public static void Enqueue<T>(T item)
        {
            LoadPool<T>(typeof(T)).Enqueue(item);
        }

        public static void Enqueue<T>(T item, Type type)
        {
            LoadPool<T>(type).Enqueue(item);
        }

        private static HeapPool<T> LoadPool<T>(Type type)
        {
            if (poolData.TryGetValue(type, out var item))
            {
                return (HeapPool<T>)item;
            }

            item = new HeapPool<T>(type);
            poolData.Add(type, item);
            return (HeapPool<T>)item;
        }

        internal static Reference[] Reference()
        {
            var index = 0;
            var items = new Reference[poolData.Count];
            foreach (var value in poolData.Values)
            {
                items[index++] = new Reference(value.type, value.path, value.acquire, value.release, value.dequeue, value.enqueue);
            }

            return items;
        }

        internal static void Dispose()
        {
            var types = new List<Type>(poolData.Keys);
            foreach (var type in types)
            {
                if (poolData.TryGetValue(type, out var item))
                {
                    item.Dispose();
                    poolData.Remove(type);
                }
            }

            poolData.Clear();
        }
    }
}