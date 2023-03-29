using System;
using System.Collections.Generic;
using JFramework.Core;
using JFramework.Interface;
using UnityEngine;

namespace JFramework
{
    /// <summary>
    /// 实体的抽象类
    /// </summary>
    [Serializable]
    public abstract class Entity : MonoBehaviour, IEntity
    {
        /// <summary>
        /// 控制器容器
        /// </summary>
        private Dictionary<string, IController> controllerDict;

        /// <summary>
        /// 实体生成
        /// </summary>
        /// <param name="value">传入生成参数</param>
        public virtual void Spawn(params object[] value)
        {
        }

        /// <summary>
        /// 实体更新
        /// </summary>
        protected virtual void OnUpdate()
        {
        }

        /// <summary>
        /// 实体启用
        /// </summary>
        protected virtual void OnEnable()
        {
            if (!GlobalManager.Instance) return;
            GlobalManager.Instance.Listen(this);
        }

        /// <summary>
        /// 实体禁用
        /// </summary>
        protected virtual void OnDisable()
        {
            if (!GlobalManager.Instance) return;
            GlobalManager.Instance.Remove(this);
        }

        /// <summary>
        /// 实体销毁
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (controllerDict == null) return;
            foreach (var controller in controllerDict.Values)
            {
                controller.Clear();
            }
        }

        /// <summary>
        /// 获取控制器
        /// </summary>
        /// <typeparam name="T">可使用任何继承IController的对象</typeparam>
        /// <returns>返回控制器对象</returns>
        public T Get<T>() where T : ScriptableObject, IController
        {
            var key = typeof(T).Name;
            controllerDict ??= new Dictionary<string, IController>();
            if (controllerDict.ContainsKey(key)) return (T)controllerDict[key];
            var controller = ScriptableObject.CreateInstance<T>();
            controllerDict.Add(key, controller);
            controller.Start(this);
            return controller;
        }

        /// <summary>
        /// 实体接口调用实体更新方法
        /// </summary>
        void IEntity.Update() => OnUpdate();
    }
}