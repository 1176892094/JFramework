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
    public static class PoolManager
    {
        private static readonly Dictionary<string, GameObject> parents = new();
        internal static readonly Dictionary<string, IPool<GameObject>> pools = new();
        private static Transform poolManager;

        internal static void Register()
        {
            poolManager = GlobalManager.Instance.transform.Find("PoolManager");
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

        public static async void Pop(string path, Action<GameObject> action)
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

            var o = await AssetManager.Load<GameObject>(path);
            Object.DontDestroyOnLoad(o);
            o.name = path;
            action?.Invoke(o);
        }

        public static void Push(GameObject obj)
        {
            if (!GlobalManager.Instance) return;
            if (obj == null) return;
            if (!parents.TryGetValue(obj.name, out var parent))
            {
                parent = new GameObject(obj.name + "-Pool");
                parent.transform.SetParent(poolManager);
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
            poolManager = null;
        }
    }
}