// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2025-01-08 17:01:33
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace JFramework.Common
{
    public static partial class PoolManager
    {
        public static async Task<GameObject> Show(string path)
        {
            if (!GlobalManager.Instance) return null;
            var assetData = await LoadPool(path).Dequeue();
            assetData.transform.SetParent(null);
            assetData.SetActive(true);
            return assetData;
        }

        public static async void Show(string path, Action<GameObject> action)
        {
            if (!GlobalManager.Instance) return;
            var assetData = await LoadPool(path).Dequeue();
            assetData.transform.SetParent(null);
            assetData.SetActive(true);
            action.Invoke(assetData);
        }

        public static bool Hide(GameObject item)
        {
            if (!GlobalManager.Instance) return false;
            if (!GlobalManager.poolGroup.TryGetValue(item.name, out var pool))
            {
                pool = new GameObject(Service.Text.Format("Pool - {0}", item.name));
                pool.transform.SetParent(GlobalManager.Instance.transform);
                GlobalManager.poolGroup.Add(item.name, pool);
            }

            item.SetActive(false);
            item.transform.SetParent(pool.transform);
            return LoadPool(item.name).Enqueue(item);
        }

        private static EntityPool LoadPool(string path)
        {
            if (GlobalManager.poolData.TryGetValue(path, out var pool))
            {
                return (EntityPool)pool;
            }

            pool = new EntityPool(typeof(GameObject), path);
            GlobalManager.poolData.Add(path, pool);
            return (EntityPool)pool;
        }

        internal static Reference[] Reference()
        {
            var index = 0;
            var items = new Reference[GlobalManager.poolData.Count];
            foreach (var value in GlobalManager.poolData.Values)
            {
                items[index++] = new Reference(value.type, value.path, value.acquire, value.release, value.dequeue, value.enqueue);
            }

            return items;
        }

        internal static void Dispose()
        {
            var paths = new List<string>(GlobalManager.poolData.Keys);
            foreach (var path in paths)
            {
                if (GlobalManager.poolData.TryGetValue(path, out var pool))
                {
                    pool.Dispose();
                    GlobalManager.poolData.Remove(path);
                }
            }

            GlobalManager.poolData.Clear();
            GlobalManager.poolGroup.Clear();
        }
    }
}