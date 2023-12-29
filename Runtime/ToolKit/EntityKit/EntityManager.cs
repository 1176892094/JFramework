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
using Sirenix.OdinInspector;
using UnityEngine;

namespace JFramework
{
    using Controllers = Dictionary<Type, Controller>;

    public sealed partial class GlobalManager
    {
        /// <summary>
        /// 控制器管理器
        /// </summary>
        internal sealed class EntityManager : ScriptableObject
        {
            /// <summary>
            /// 控制器存储字典
            /// </summary>
            [ShowInInspector] private readonly Dictionary<GameObject, Controllers> components = new Dictionary<GameObject, Controllers>();

            /// <summary>
            /// 临时实体存储栈
            /// </summary>
            private readonly Stack<GameObject> stacks = new Stack<GameObject>();

            /// <summary>
            /// 临时实体值
            /// </summary>
            public GameObject instance => stacks.Pop();

            /// <summary>
            /// 为实体注册控制器
            /// </summary>
            /// <param name="gameObject"></param>
            /// <param name="type"></param>
            /// <returns></returns>
            public Controller Register(GameObject gameObject, Type type)
            {
                if (!components.TryGetValue(gameObject, out var controls))
                {
                    controls = new Controllers();
                    components.Add(gameObject, controls);
                }

                if (!controls.TryGetValue(type, out var control))
                {
                    stacks.Push(gameObject);
                    control = (Controller)CreateInstance(type);
                    if (control.owner)
                    {
                        controls.Add(type, control);
                    }
                }

                return components[gameObject][type];
            }

            /// <summary>
            /// 取消注册控制器
            /// </summary>
            /// <param name="gameObject"></param>
            public void UnRegister(GameObject gameObject)
            {
                if (components.TryGetValue(gameObject, out var controls))
                {
                    foreach (var control in controls.Values)
                    {
                        Destroy(control);
                    }

                    controls.Clear();
                    components.Remove(gameObject);
                }
            }

            /// <summary>
            /// 管理器销毁
            /// </summary>
            private void OnDestroy()
            {
                var copies = components.Keys.ToList();
                foreach (var gameObject in copies)
                {
                    UnRegister(gameObject);
                }

                components.Clear();
            }
        }
    }
}