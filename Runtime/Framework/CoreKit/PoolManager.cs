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
    public sealed class PoolManager : ScriptableObject
    {
        [ShowInInspector, LabelText("对象池组")] private Dictionary<string, GameObject> parents = new();
        [ShowInInspector, LabelText("对象列表")] private Dictionary<string, IPool<GameObject>> pools = new();
        private Transform poolManager;

        internal void OnEnable()
        {
            if (!GlobalManager.Instance) return;
            poolManager = GlobalManager.Instance.transform.Find("PoolManager");
        }

        public async Task<GameObject> Pop(string path)
        {
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

            var o = await GlobalManager.Asset.Load<GameObject>(path);
            DontDestroyOnLoad(o);
            o.name = path;
            return o;
        }

        public async void Pop(string path, Action<GameObject> action)
        {
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

            var o = await GlobalManager.Asset.Load<GameObject>(path);
            DontDestroyOnLoad(o);
            o.name = path;
            action?.Invoke(o);
        }

        public void Push(GameObject obj)
        {
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

        internal void OnDisable()
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