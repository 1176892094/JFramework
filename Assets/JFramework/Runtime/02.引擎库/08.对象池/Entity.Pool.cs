// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2025-01-08 17:01:39
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JFramework.Common
{
    public static partial class EntityManager
    {
        private class EntityPool : IPool
        {
            private readonly HashSet<GameObject> cached = new HashSet<GameObject>();
            private readonly Queue<GameObject> unused = new Queue<GameObject>();

            public EntityPool(string assetPath, Type assetType)
            {
                this.assetPath = assetPath;
                this.assetType = assetType;
            }

            public Type assetType { get; private set; }
            public string assetPath { get; private set; }
            public int acquire => cached.Count;
            public int release => unused.Count;
            public int dequeue { get; private set; }
            public int enqueue { get; private set; }

            public async Task<GameObject> Dequeue()
            {
                dequeue++;
                GameObject assetData;
                if (unused.Count > 0)
                {
                    assetData = unused.Dequeue();
                    if (assetData != null)
                    {
                        cached.Add(assetData);
                        return assetData;
                    }

                    enqueue++;
                    cached.Remove(assetData);
                }

                assetData = await AssetManager.Load<GameObject>(assetPath);
                Object.DontDestroyOnLoad(assetData);
                assetData.name = assetPath;
                cached.Add(assetData);
                return assetData;
            }

            public bool Enqueue(GameObject assetData)
            {
                if (cached.Remove(assetData))
                {
                    enqueue++;
                    unused.Enqueue(assetData);
                    return true;
                }

                return false;
            }

            void IDisposable.Dispose()
            {
                cached.Clear();
                unused.Clear();
            }
        }
    }
}