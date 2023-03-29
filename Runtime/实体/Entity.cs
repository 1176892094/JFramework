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
        private Dictionary<string, IController> controllerDict;

        public virtual void Spawn(params object[] value)
        {
        }

        protected virtual void OnUpdate()
        {
        }

        protected virtual void OnEnable()
        {
            if (!GlobalManager.Instance) return;
            GlobalManager.Instance.Listen(this);
        }

        protected virtual void OnDisable()
        {
            if (!GlobalManager.Instance) return;
            GlobalManager.Instance.Remove(this);
        }

        protected virtual void OnDestroy()
        {
            if (controllerDict == null) return;
            foreach (var controller in controllerDict.Values)
            {
                controller.Clear();
            }
        }

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

        void IEntity.Update() => OnUpdate();
    }
}