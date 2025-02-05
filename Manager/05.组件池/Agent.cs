// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2025-01-08 17:01:24
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using JFramework.Common;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JFramework
{
    internal static class AgentManager
    {
        public static T Show<T>(Component entity) where T : ScriptableObject
        {
            if (!GlobalManager.Instance) return default;
            if (!GlobalManager.agentData.TryGetValue(entity, out var agentData))
            {
                agentData = new Dictionary<Type, ScriptableObject>();
                GlobalManager.agentData.Add(entity, agentData);
            }

            if (!agentData.TryGetValue(typeof(T), out var agent))
            {
                GlobalManager.cached = entity;
                agent = LoadPool(typeof(T)).Dequeue();
                agentData.Add(typeof(T), agent);
                ((IAgent)agent).OnAwake(entity);
            }
            
            return (T)GlobalManager.agentData[entity][typeof(T)];
        }

        public static ScriptableObject Show(Component entity, Type agentType)
        {
            if (!GlobalManager.Instance) return default;
            if (!GlobalManager.agentData.TryGetValue(entity, out var agentData))
            {
                agentData = new Dictionary<Type, ScriptableObject>();
                GlobalManager.agentData.Add(entity, agentData);
            }

            if (!agentData.TryGetValue(agentType, out var agent))
            {
                GlobalManager.cached = entity;
                agent = LoadPool(agentType).Dequeue();
                agentData.Add(agentType, agent);
                ((IAgent)agent).OnAwake(entity);
            }

            return GlobalManager.agentData[entity][agentType];
        }

        public static void Hide(Component entity)
        {
            if (!GlobalManager.Instance) return;
            if (GlobalManager.agentData.TryGetValue(entity, out var agentData))
            {
                foreach (var agent in agentData.Values)
                {
                    ((IAgent)agent).Dispose();
                    LoadPool(agent.GetType()).Enqueue(agent);
                }

                agentData.Clear();
                GlobalManager.agentData.Remove(entity);
            }
        }

        private static AgentPool LoadPool(Type assetType)
        {
            var assetName = assetType.Name;
            if (GlobalManager.poolData.TryGetValue(assetName, out var poolData))
            {
                return (AgentPool)poolData;
            }

            poolData = new AgentPool(assetType);
            GlobalManager.poolData.Add(assetName, poolData);
            return (AgentPool)poolData;
        }

        internal static void Dispose()
        {
            var agentCaches = new List<Component>(GlobalManager.agentData.Keys);
            foreach (var cache in agentCaches)
            {
                if (GlobalManager.agentData.TryGetValue(cache, out var agentData))
                {
                    foreach (var agent in agentData.Values)
                    {
                        Object.Destroy(agent);
                    }

                    agentData.Clear();
                    GlobalManager.agentData.Remove(cache);
                }
            }

            GlobalManager.agentData.Clear();
        }
    }
}