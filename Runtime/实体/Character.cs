using System;
using System.Collections.Generic;
using JFramework.Core;
using JFramework.Interface;
using Sirenix.OdinInspector;
using UnityEngine;

// ReSharper disable All
namespace JFramework
{
    public abstract class Character<TEvent> : Entity, IEntity where TEvent : IEvent
    {
        /// <summary>
        /// 控制器容器
        /// </summary>
        [ShowInInspector, LabelText("控制器容器"), SerializeField]
        private readonly Dictionary<Type, ScriptableObject> controllerDict = new Dictionary<Type, ScriptableObject>();

        /// <summary>
        /// 获取控制器
        /// </summary>
        /// <typeparam name="T">可使用任何继承IController的对象</typeparam>
        /// <returns>返回控制器对象</returns>
        public T GetOrAddCtrl<T>() where T : ScriptableObject, IController
        {
            var key = typeof(T);
            if (!controllerDict.ContainsKey(key))
            {
                var controller = ScriptableObject.CreateInstance<T>();
                controllerDict.Add(key, controller);
                return (T)controller.Spawn(this);
            }

            return (T)controllerDict[key];
        }

        /// <summary>
        /// 实体销毁
        /// </summary>
        void IEntity.Destroy()
        {
            try
            {
                foreach (var scriptable in controllerDict.Values)
                {
                    Destroy(scriptable);
                }

                controllerDict.Clear();
                Despawn();
            }
            catch (Exception e)
            {
                Log.Info(DebugOption.Custom, e.ToString());
            }
        }
    }
}