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
        private Dictionary<string, IController> controlDict;

        protected virtual void Spawn(params object[] value)
        {
        }

        protected virtual void Enable()
        {
        }

        protected virtual void OnUpdate()
        {
        }

        protected virtual void Disable()
        {
        }

        private void OnEnable()
        {
            if (!GlobalManager.Instance) return;
            GlobalManager.Instance.Listen(this);
            Enable();
        }

        private void OnDisable()
        {
            if (!GlobalManager.Instance) return;
            GlobalManager.Instance.Remove(this);
            Disable();
        }

        public T Get<T>() where T : ScriptableObject, IController
        {
            var key = typeof(T).Name;
            controlDict ??= new Dictionary<string, IController>();
            if (controlDict.ContainsKey(key)) return (T)controlDict[key];
            var controller = ScriptableObject.CreateInstance<T>();
            controlDict.Add(key, controller);
            controller.Start(this);
            return controller;
        }

        void IEntity.Spawn(params object[] value) => Spawn(value);

        void IEntity.Update() => OnUpdate();
    }
}