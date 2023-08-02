using System;
using System.Collections.Generic;
using System.Linq;
using JFramework.Interface;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JFramework
{
    using Controllers = Dictionary<Type, ScriptableObject>;

    /// <summary>
    /// 控制器管理器
    /// </summary>
    internal static class CtrlManager
    {
        /// <summary>
        /// 全局控制器容器
        /// </summary>
        internal static readonly Dictionary<IEntity, Controllers> controllerDict = new Dictionary<IEntity, Controllers>();

        /// <summary>
        /// 获取或添加控制器
        /// </summary>
        /// <param name="entity"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetOrAddCtrl<T>(IEntity entity) where T : ScriptableObject, IController
        {
            var key = typeof(T);
            if (!controllerDict.TryGetValue(entity, out Controllers controllers))
            {
                controllers = new Controllers();
                controllerDict.Add(entity, controllers);
            }

            if (!controllers.ContainsKey(key))
            {
                var controller = ScriptableObject.CreateInstance<T>();
                controllers.Add(key, controller);
                controller.Spawn(entity);
            }

            return (T)controllerDict[entity][key];
        }

        /// <summary>
        /// 销毁指定控制器
        /// </summary>
        /// <param name="entity"></param>
        public static void Destroy(IEntity entity)
        {
            if (controllerDict.TryGetValue(entity, out Controllers controllers))
            {
                foreach (var controller in controllers.Values)
                {
                    Object.Destroy(controller);
                }

                controllers.Clear();
                controllerDict.Remove(entity);
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