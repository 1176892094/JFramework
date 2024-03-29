// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-02-04  14:32
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using JFramework.Interface;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JFramework.Core
{
    using Components = Dictionary<Type, IComponent>;

    internal sealed class EntityManager : ScriptableObject
    {
        [ShowInInspector, LabelText("实体组件")] private Dictionary<IEntity, Components> entities = new();
        public IEntity instance;

        public IComponent FindComponent(IEntity entity, Type type)
        {
            if (!entities.TryGetValue(entity, out var components))
            {
                components = new Components();
                entities.Add(entity, components);
            }

            if (!components.TryGetValue(type, out var component))
            {
                instance = entity;
                component = (IComponent)CreateInstance(type);
                ((ScriptableObject)component).name = type.Name;
                components.Add(type, component);
                component.OnAwake(instance);
            }

            return entities[entity][type];
        }

        public void Destroy(IEntity entity)
        {
            if (entities.TryGetValue(entity, out var components))
            {
                foreach (var component in components.Values)
                {
                    Destroy((ScriptableObject)component);
                }

                components.Clear();
                entities.Remove(entity);
            }
        }

        internal void OnDisable()
        {
            var copies = entities.Keys.ToList();
            foreach (var entity in copies)
            {
                Destroy(entity);
            }

            entities.Clear();
        }
    }
}