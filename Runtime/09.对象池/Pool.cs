// *********************************************************************************
// # Project: Forest
// # Unity: 2022.3.5f1c1
// # Author: jinyijie
// # Version: 1.0.0
// # History: 2024-08-25  03:08
// # Copyright: 2024, jinyijie
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using JFramework.Interface;

namespace JFramework.Core
{
    public static partial class PoolManager
    {
        [Serializable]
        private class Pool<T> : IPool<T>
        {
            private readonly Queue<T> objects = new Queue<T>();
            private readonly HashSet<T> unique = new HashSet<T>();
            public int count => objects.Count;

            public Pool(T obj)
            {
                unique.Add(obj);
                objects.Enqueue(obj);
            }

            public T Pop()
            {
                if (objects.Count > 0)
                {
                    var obj = objects.Dequeue();
                    unique.Remove(obj);
                    return obj;
                }

                return Activator.CreateInstance<T>();
            }

            public bool Push(T obj)
            {
                if (unique.Add(obj))
                {
                    objects.Enqueue(obj);
                    return true;
                }

                return false;
            }

            public void Dispose()
            {
                unique.Clear();
                objects.Clear();
            }
        }
    }
}