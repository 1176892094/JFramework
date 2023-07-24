using System;
using System.Collections.Generic;
using JFramework.Core;
using JFramework.Interface;
using Sirenix.OdinInspector;
using UnityEngine;

// ReSharper disable All
namespace JFramework
{
    public abstract class Character : Entity, IEntity
    {
        /// <summary>
        /// 控制器容器
        /// </summary>
        [ShowInInspector, LabelText("控制器列表"), SerializeField]
        private readonly Dictionary<Type, ScriptableObject> controllers = new Dictionary<Type, ScriptableObject>();

        /// <summary>
        /// 获取控制器
        /// </summary>
        /// <typeparam name="T">可使用任何继承IController的对象</typeparam>
        /// <returns>返回控制器对象</returns>
        public T GetOrAddCtrl<T>() where T : ScriptableObject, IController
        {
            var key = typeof(T);
            if (!controllers.ContainsKey(key))
            {
                var controller = ScriptableObject.CreateInstance<T>();
                controllers.Add(key, controller);
                return (T)controller.Spawn(this);
            }

            return (T)controllers[key];
        }

        /// <summary>
        /// 实体销毁
        /// </summary>
        void IEntity.Destroy()
        {
            try
            {
                foreach (var scriptable in controllers.Values)
                {
                    Destroy(scriptable);
                }

                controllers.Clear();
                Despawn();
            }
            catch (Exception e)
            {
                Log.Info(DebugOption.Custom, e.ToString());
            }
        }
    }
}