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
using Sirenix.OdinInspector;
using UnityEngine;

namespace JFramework.Core
{
    public sealed class PoolManager : Component<GlobalManager>
    {
        private Transform poolManager;
        [ShowInInspector] private readonly Dictionary<string, GameObject> parents = new Dictionary<string, GameObject>();
        [ShowInInspector] private readonly Dictionary<string, IPool<GameObject>> pools = new Dictionary<string, IPool<GameObject>>();

        private void Awake() => poolManager = owner.transform.Find("PoolManager");

        public async Task<GameObject> Pop(string path)
        {
            if (!GlobalManager.Instance) return default;
            if (pools.TryGetValue(path, out var pool) && pool.Count > 0)
            {
                var obj = pool.Pop();
                if (obj != null)
                {
                    obj.SetActive(true);
                    obj.transform.SetParent(null);
                    return obj;
                }
            }

            var o = await GlobalManager.Asset.Load<GameObject>(path);
            DontDestroyOnLoad(o);
            o.name = path;
            return o;
        }

        public async void Pop(string path, Action<GameObject> action)
        {
            if (!GlobalManager.Instance) return;
            if (pools.TryGetValue(path, out var pool) && pool.Count > 0)
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

            var o = await GlobalManager.Asset.Load<GameObject>(path);
            DontDestroyOnLoad(o);
            o.name = path;
            action?.Invoke(o);
        }

        public void Push(GameObject obj)
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

        private void OnDestroy()
        {
            foreach (var pool in pools.Values)
            {
                pool.Dispose();
            }

            pools.Clear();
            parents.Clear();
        }
    }
}