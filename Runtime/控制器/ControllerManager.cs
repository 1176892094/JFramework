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
    using Controllers = Dictionary<Type, ScriptableObject>;

    /// <summary>
    /// 控制器管理器
    /// </summary>
    internal static class ControllerManager
    {
        /// <summary>
        /// 全局控制器容器
        /// </summary>
        [ShowInInspector]
        internal static readonly Dictionary<int, Controllers> controllerDict = new Dictionary<int, Controllers>();

        /// <summary>
        /// 获取或添加控制器
        /// </summary>
        /// <param name="id"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetOrAddCtrl<T>(int id) where T : ScriptableObject, IController
        {
            var key = typeof(T);
            if (!controllerDict.TryGetValue(id, out Controllers controllers))
            {
                controllers = new Controllers();
                controllerDict.Add(id, controllers);
            }

            if (!controllers.ContainsKey(key))
            {
                var controller = ScriptableObject.CreateInstance<T>();
                controller.Spawn(GlobalManager.Get<IEntity>(id));
                controllers.Add(key, controller);
            }

            return (T)controllerDict[id][key];
        }

        /// <summary>
        /// 销毁指定控制器
        /// </summary>
        /// <param name="id"></param>
        public static void Destroy(int id)
        {
            if (controllerDict.TryGetValue(id, out Controllers controllers))
            {
                foreach (var controller in controllers.Values)
                {
                    Object.Destroy(controller);
                }

                controllers.Clear();
                controllerDict.Remove(id);
            }
        }

        /// <summary>
        /// 销毁所有控制器
        /// </summary>
        internal static void Destroy()
        {
            var controllers = controllerDict.Values.SelectMany(dictionary => dictionary.Values);
            foreach (var controller in controllers)
            {
                Object.Destroy(controller);
            }

            controllerDict.Clear();
        }
    }
}