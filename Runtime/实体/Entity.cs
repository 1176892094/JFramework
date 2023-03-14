using System;
using JFramework.Interface;
using UnityEngine;

namespace JFramework
{
    /// <summary>
    /// 实体的抽象类
    /// </summary>
    public abstract class Entity : MonoBehaviour, IEntity
    {
        protected virtual void Awake() { }

        protected virtual void Enable() { }
        
        protected virtual void Start(params object[] values) { }

        protected virtual void Start() { }
        
        [Obsolete("请使用OnUpdate来代替Update管理生命周期", true)]
        private void Update() { }
        
        protected virtual void OnUpdate() { }
        
        protected virtual void Disable() { }
        
        protected virtual void OnDestroy() { }
        
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
        
        void IEntity.Start(params object[] values) => Start(values);
        
        void IEntity.OnUpdate() => OnUpdate();
    }
}