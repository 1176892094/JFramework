using System;
using System.Collections.Generic;
using JFramework.Core;
using JFramework.Interface;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JFramework
{
    /// <summary>
    /// 实体的抽象类
    /// </summary>
    [Serializable]
    public abstract class Entity : MonoBehaviour, IEntity
    {
        [ShowInInspector,LabelText("控制器列表")] private Dictionary<string, IController> controllerDict;

        protected virtual void Awake()
        {
        }

        protected virtual void Enable()
        {
        }

        protected virtual void Start()
        {
        }

        protected virtual void OnUpdate()
        {
        }

        protected virtual void Disable()
        {
        }

        protected virtual void OnDestroy()
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
        
        protected virtual void Spawn(params object[] value)
        {
            controllerDict = new Dictionary<string, IController>();
        }

        protected T Get<T>() where T : ScriptableObject, IController
        {
            var key = typeof(T).Name;
            if (controllerDict.ContainsKey(key)) return (T)controllerDict[key];
            var controller = ScriptableObject.CreateInstance<T>();
            controllerDict.Add(key, controller);
            controller.Start(this);
            return controller;
        }

        void IEntity.Spawn(params object[] value) => Spawn(value);

        void IEntity.Update() => OnUpdate();

        T IEntity.Get<T>() => Get<T>();
    }
}