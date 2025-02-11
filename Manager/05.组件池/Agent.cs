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

namespace JFramework
{
    internal static class AgentManager
    {
        internal static void Register(Component component, Type agentType)
        {
            if (!GlobalManager.Instance) return;
            if (!GlobalManager.agentData.TryGetValue(component, out var agentData))
            {
                agentData = new Dictionary<Type, IAgent>();
                GlobalManager.agentData.Add(component, agentData);
            }

            if (!agentData.TryGetValue(agentType, out var agent))
            {
                agent = Service.Pool.Dequeue<IAgent>(agentType);
                agentData.Add(agentType, agent);
                agent.OnShow(component);
            }
        }

        internal static IAgent Find(Component component, Type agentType)
        {
            if (!GlobalManager.Instance) return default;
            if (!GlobalManager.agentData.TryGetValue(component, out var agentData))
            {
                return default;
            }

            if (!agentData.TryGetValue(agentType, out var agent))
            {
                return default;
            }

            return agent;
        }

        internal static void UnRegister(Component component, Type agentType)
        {
            if (!GlobalManager.Instance) return;
            if (!GlobalManager.agentData.TryGetValue(component, out var agentData))
            {
                return;
            }

            if (agentData.TryGetValue(agentType, out var agent))
            {
                agent.OnHide();
                agentData.Remove(agentType);
                Service.Pool.Enqueue(agent, typeof(IAgent));
            }

            if (agentData.Count == 0)
            {
                GlobalManager.agentData.Remove(component);
            }
        }

        internal static void Update()
        {
            if (!GlobalManager.Instance) return;
            foreach (var agentData in GlobalManager.agentData.Values)
            {
                foreach (var agent in agentData.Values)
                {
                    agent.OnUpdate();
                }
            }
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
                        agent.OnHide();
                        Service.Pool.Enqueue(agent, agent.GetType());
                    }

                    agentData.Clear();
                    GlobalManager.agentData.Remove(cache);
                }
            }

            GlobalManager.agentData.Clear();
        }
    }
}