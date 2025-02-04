// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-10 02:01:43
// # Recently: 2025-01-10 02:01:44
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using JFramework.Common;
using UnityEngine;

namespace JFramework
{
    internal class AgentPool : IPool
    {
        private readonly HashSet<ScriptableObject> cached = new HashSet<ScriptableObject>();
        private readonly Queue<ScriptableObject> unused = new Queue<ScriptableObject>();

        public AgentPool(Type assetType)
        {
            this.assetType = assetType;
        }

        public Type assetType { get; private set; }
        public string assetPath { get; private set; }
        public int acquire => cached.Count;
        public int release => unused.Count;
        public int dequeue { get; private set; }
        public int enqueue { get; private set; }

        public ScriptableObject Dequeue()
        {
            dequeue++;
            ScriptableObject assetData;
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

            assetData = ScriptableObject.CreateInstance(assetType);
            assetData.name = assetType.Name;
            cached.Add(assetData);
            return assetData;
        }

        public void Enqueue(ScriptableObject assetData)
        {
            if (cached.Remove(assetData))
            {
                enqueue++;
                unused.Enqueue(assetData);
            }
        }

        void IDisposable.Dispose()
        {
            cached.Clear();
            unused.Clear();
        }
    }
}