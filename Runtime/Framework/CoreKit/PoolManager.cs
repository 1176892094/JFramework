// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-02-04  17:44
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JFramework.Interface;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JFramework.Core
{
    public static partial class PoolManager
    {
        public static Transform manager;
        private static readonly Dictionary<Type, IPool> streams = new();
        private static readonly Dictionary<string, GameObject> parents = new();
        private static readonly Dictionary<string, IPool<GameObject>> pools = new();

        internal static void Register()
        {
            var obj = new GameObject();
            Object.DontDestroyOnLoad(obj);
            obj.name = nameof(PoolManager);
            manager = obj.transform;
        }

        public static async Task<GameObject> Pop(string path)
        {
            if (!GlobalManager.Instance) return null;
            if (pools.TryGetValue(path, out var pool) && pool.count > 0)
            {
                var obj = pool.Pop();
                if (obj != null)
                {
                    obj.SetActive(true);
                    obj.transform.SetParent(null);
                    return obj;
                }
            }

            var o = await AssetManager.Load<GameObject>(path);
            Object.DontDestroyOnLoad(o);
            o.name = path;
            return o;
        }

        public static void Pop(string path, Action<GameObject> action)
        {
            if (!GlobalManager.Instance) return;
            if (pools.TryGetValue(path, out var pool) && pool.count > 0)
            {
                var obj = pool.Pop();
                if (obj != null)
                {
                    obj.SetActive(true);
                    obj.transform.SetParent(null);
                    action?.Invoke(obj);
                    return;
                }
            }

            AssetManager.Load<GameObject>(path, obj =>
            {
                Object.DontDestroyOnLoad(obj);
                obj.name = path;
                action?.Invoke(obj);
            });
        }

        public static void Push(GameObject obj)
        {
            if (!GlobalManager.Instance) return;
            if (obj == null) return;
            if (!parents.TryGetValue(obj.name, out var parent))
            {
                parent = new GameObject(obj.name + "-Pool");
                parent.transform.SetParent(manager);
                pools.Add(obj.name, new Pool<GameObject>(obj));
                obj.transform.SetParent(parent.transform);
                parents.Add(obj.name, parent);
                obj.SetActive(false);
                return;
            }

            if (pools.TryGetValue(obj.name, out var pool))
            {
                if (!pool.Push(obj)) return;
                obj.transform.SetParent(parent.transform);
                obj.SetActive(false);
            }
        }

        internal static void UnRegister()
        {
            foreach (var pool in pools.Values)
            {
                pool.Dispose();
            }

            pools.Clear();
            parents.Clear();
            manager = null;

            foreach (var pool in streams.Values)
            {
                pool.Dispose();
            }

            streams.Clear();
        }
    }

    public static partial class PoolManager
    {
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