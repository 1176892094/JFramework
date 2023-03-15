using System.Collections.Generic;
using JFramework.Interface;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JFramework
{
    /// <summary>
    /// 实体的抽象类
    /// </summary>
    public abstract class Entity : MonoBehaviour, IEntity
    {
        [ShowInInspector, LabelText("实体控制器列表")]
        private Dictionary<string, IController> controllerDict = new Dictionary<string, IController>();

        protected virtual void Awake()
        {
        }

        protected virtual void Enable()
        {
        }

        protected virtual void Spawn(params object[] value)
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

        protected T GetController<T>() where T : ScriptableObject, IController
        {
            var key = typeof(T).Name;
            if (controllerDict.ContainsKey(key))
            {
                return (T)controllerDict[key];
            }

            Debug.Log($"{key.Red()} 不存在");
            return null;
        }

        protected T AddController<T>() where T : ScriptableObject, IController
        {
            var key = typeof(T).Name;
            if (!controllerDict.ContainsKey(key))
            {
                var controller = ScriptableObject.CreateInstance<T>();
                controllerDict.Add(key, controller);
                controller.Start(this);
                return controller;
            }

            Debug.Log($"{key.Red()} 已经存在");
            return (T)controllerDict[key];
        }

        void IEntity.Spawn(params object[] value) => Spawn(value);

        void IEntity.Update() => OnUpdate();

        T IEntity.GetController<T>() => GetController<T>();

        T IEntity.AddController<T>() => AddController<T>();
    }
}