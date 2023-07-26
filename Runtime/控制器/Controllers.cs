using System;
using System.Collections.Generic;
using System.Linq;
using JFramework.Core;
using JFramework.Interface;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JFramework
{
    using Components = Dictionary<Type, ScriptableObject>;

    /// <summary>
    /// 控制器管理器
    /// </summary>
    public static class Controllers
    {
        /// <summary>
        /// 全局控制器容器
        /// </summary>
        [ShowInInspector] internal static readonly Dictionary<int, Components> controllers = new Dictionary<int, Components>();

        /// <summary>
        /// 获取或添加控制器
        /// </summary>
        /// <param name="id"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetOrAddCtrl<T>(int id) where T : ScriptableObject, IController
        {
            var key = typeof(T);
            if (!controllers.TryGetValue(id, out Components components))
            {
                components = new Components();
                controllers.Add(id, components);
            }

            if (!components.ContainsKey(key))
            {
                var controller = ScriptableObject.CreateInstance<T>();
                controller.Spawn(GlobalManager.Get<IEntity>(id));
                components.Add(key, controller);
            }

            return (T)controllers[id][key];
        }

        /// <summary>
        /// 销毁控制器
        /// </summary>
        /// <param name="id"></param>
        public static void RemoveCtrl(int id)
        {
            if (controllers.TryGetValue(id, out Components components))
            {
                foreach (var scriptable in components.Values)
                {
                    Object.Destroy(scriptable);
                }

                components.Clear();
            }
        }

        internal static void Destroy()
        {
            foreach (var scriptable in controllers.Values.SelectMany(components => components.Values))
            {
                Object.Destroy(scriptable);
            }
            controllers.Clear();
        }
    }
}