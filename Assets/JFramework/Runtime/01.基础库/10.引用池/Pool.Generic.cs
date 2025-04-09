// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-10 18:01:34
// # Recently: 2025-01-11 18:01:32
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;

namespace JFramework.Common
{
    public static partial class PoolManager
    {
        [Serializable]
        private class Pool<T> : IPool
        {
            private readonly HashSet<T> cached = new HashSet<T>();
            private readonly Queue<T> unused = new Queue<T>();

            public Pool(Type assetType)
            {
                this.assetType = assetType;
            }

            public Type assetType { get; private set; }
            public string assetPath { get; private set; }
            public int acquire => cached.Count;
            public int release => unused.Count;
            public int dequeue { get; private set; }
            public int enqueue { get; private set; }

            void IDisposable.Dispose()
            {
                cached.Clear();
                unused.Clear();
            }

            public T Dequeue()
            {
                T assetData;
                lock (unused)
                {
                    dequeue++;
                    if (unused.Count > 0)
                    {
                        assetData = unused.Dequeue();
                    }
                    else
                    {
                        assetData = (T)Activator.CreateInstance(assetType);
                    }

                    cached.Add(assetData);
                }

                return assetData;
            }

            public void Enqueue(T assetData)
            {
                lock (unused)
                {
                    if (cached.Remove(assetData))
                    {
                        enqueue++;
                        unused.Enqueue(assetData);
                    }
                }
            }
        }
    }
}