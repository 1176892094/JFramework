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
    using Controllers = Dictionary<Type, IController>;

    /// <summary>
    /// 控制器管理器
    /// </summary>
    internal sealed class EntityManager : ScriptableObject
    {
        /// <summary>
        /// 控制器存储字典
        /// </summary>
        [ShowInInspector, LabelText("实体列表")]
        private readonly Dictionary<IEntity, Controllers> components = new Dictionary<IEntity, Controllers>();

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
        public IController Register(IEntity entity, Type type)
        {
            if (!components.TryGetValue(entity, out var controls))
            {
                controls = new Controllers();
                components.Add(entity, controls);
            }

            if (!controls.TryGetValue(type, out var control))
            {
                stacks.Push(entity);
                control = (IController)CreateInstance(type);
                ((ScriptableObject)control).name = type.Name;
                controls.Add(type, control);
                control.Awake();
            }

            return components[entity][type];
        }

        /// <summary>
        /// 取消注册控制器
        /// </summary>
        /// <param name="entity"></param>
        public void UnRegister(IEntity entity)
        {
            if (components.TryGetValue(entity, out var controls))
            {
                foreach (var control in controls.Values)
                {
                    Destroy((ScriptableObject)control);
                }

                controls.Clear();
                components.Remove(entity);
            }
        }

        /// <summary>
        /// 管理器销毁
        /// </summary>
        private void OnDestroy()
        {
            var copies = components.Keys.ToList();
            foreach (var entity in copies)
            {
                UnRegister(entity);
            }

            components.Clear();
        }
    }
}