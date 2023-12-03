// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-10-24  23:39
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using JFramework.Interface;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JFramework.Core
{
    using Components = Dictionary<Type, ScriptableObject>;

    /// <summary>
    /// 控制器管理器
    /// </summary>
    internal static class EntityManager
    {
        /// <summary>
        /// 全局控制器容器
        /// </summary>
        public static readonly Dictionary<IEntity, Components> entities = new Dictionary<IEntity, Components>();
        
        /// <summary>
        /// 注册并返回控制器
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ScriptableObject Register(IEntity entity, Type type)
        {
            if (!entities.TryGetValue(entity, out Components components))
            {
                components = new Components();
                entities.Add(entity, components);
            }

            if (!components.ContainsKey(type))
            {
                var component = ScriptableObject.CreateInstance(type);
                components.Add(type, component);
                ((IController)component).Register(entity);
            }

            return entities[entity][type];
        }

        /// <summary>
        /// 销毁指定控制器
        /// </summary>
        /// <param name="entity"></param>
        public static void UnRegister(IEntity entity)
        {
            if (entities.TryGetValue(entity, out Components components))
            {
                foreach (var component in components.Values)
                {
                    Object.Destroy(component);
                }

                components.Clear();
                entities.Remove(entity);
            }
        }

        /// <summary>
        /// 销毁所有控制器
        /// </summary>
        public static void Dispose()
        {
            var copies = entities.Keys.ToList();
            foreach (var entity in copies)
            {
                entity.UnRegister();
            }

            entities.Clear();
        }
    }
}