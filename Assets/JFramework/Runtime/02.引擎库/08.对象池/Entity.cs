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
        public static async Task<GameObject> Show(string assetPath)
        {
            if (!GlobalManager.Instance) return null;
            var assetData = await LoadPool(assetPath).Dequeue();
            assetData.transform.SetParent(null);
            assetData.SetActive(true);
            return assetData;
        }

        public static async void Show(string assetPath, Action<GameObject> assetAction)
        {
            if (!GlobalManager.Instance) return;
            var assetData = await LoadPool(assetPath).Dequeue();
            assetData.transform.SetParent(null);
            assetData.SetActive(true);
            assetAction.Invoke(assetData);
        }

        public static bool Hide(GameObject assetData)
        {
            if (!GlobalManager.Instance) return false;
            if (!GlobalManager.poolGroup.TryGetValue(assetData.name, out var parent))
            {
                parent = new GameObject(Service.Text.Format("Pool - {0}", assetData.name));
                parent.transform.SetParent(GlobalManager.Instance.transform);
                GlobalManager.poolGroup.Add(assetData.name, parent);
            }

            assetData.SetActive(false);
            assetData.transform.SetParent(parent.transform);
            return LoadPool(assetData.name).Enqueue(assetData);
        }

        private static EntityPool LoadPool(string assetPath)
        {
            if (GlobalManager.poolData.TryGetValue(assetPath, out var poolData))
            {
                return (EntityPool)poolData;
            }

            poolData = new EntityPool(typeof(GameObject), assetPath);
            GlobalManager.poolData.Add(assetPath, poolData);
            return (EntityPool)poolData;
        }

        internal static Reference[] Reference()
        {
            var index = 0;
            var results = new Reference[GlobalManager.poolData.Count];
            foreach (var value in GlobalManager.poolData.Values)
            {
                var assetType = value.type;
                var assetPath = value.path;
                results[index++] = new Reference(assetType, assetPath, value.acquire, value.release, value.dequeue, value.enqueue);
            }

            return results;
        }

        internal static void Dispose()
        {
            var poolCaches = new List<string>(GlobalManager.poolData.Keys);
            foreach (var cache in poolCaches)
            {
                if (GlobalManager.poolData.TryGetValue(cache, out var poolData))
                {
                    poolData.Dispose();
                    GlobalManager.poolData.Remove(cache);
                }
            }

            GlobalManager.poolData.Clear();
            GlobalManager.poolGroup.Clear();
        }
    }
}