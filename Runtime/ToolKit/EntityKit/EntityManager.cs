// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-12-21  17:46
// # Copyright: 2023, Charlotte
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

    /// <summary>
    /// 控制器管理器
    /// </summary>
    internal sealed class EntityManager : ScriptableObject
    {
        /// <summary>
        /// 控制器存储字典
        /// </summary>
        [ShowInInspector]
        private readonly Dictionary<IEntity, Components> entities = new Dictionary<IEntity, Components>();

        /// <summary>
        /// 临时实体存储栈
        /// </summary>
        private readonly Stack<IEntity> stacks = new Stack<IEntity>();

        /// <summary>
        /// 临时实体值
        /// </summary>
        public IEntity instance => stacks.Pop();

        /// <summary>
        /// 为实体注册控制器
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IComponent FindComponent(IEntity entity, Type type)
        {
            if (!entities.TryGetValue(entity, out var components))
            {
                components = new Components();
                entities.Add(entity, components);
            }

            if (!components.TryGetValue(type, out var component))
            {
                stacks.Push(entity);
                component = (IComponent)CreateInstance(type);
                ((ScriptableObject)component).name = type.Name;
                components.Add(type, component);
                component.Awake();
            }

            return entities[entity][type];
        }

        /// <summary>
        /// 取消注册控制器
        /// </summary>
        /// <param name="entity"></param>
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

        /// <summary>
        /// 管理器销毁
        /// </summary>
        private void OnDestroy()
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