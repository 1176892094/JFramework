using System;
using JFramework.Interface;
using UnityEngine;

namespace JFramework
{
    public abstract class Entity : MonoBehaviour, IEntity
    {
        protected virtual void Awake() { }
        
        protected virtual void Start() { }
        
        [Obsolete("[JFramework] 请使用OnUpdate来代替Update管理生命周期", true)]
        private void Update() { }

        protected virtual void OnDestroy() { }
        
        /// <summary>
        /// 实体启用
        /// </summary>
        private void OnEnable()
        {
            GlobalManager.Instance.Listen(this);
            Enable();
        }

        /// <summary>
        /// 实体禁用
        /// </summary>
        private void OnDisable()
        {
            if (!GlobalManager.Instance) return;
            GlobalManager.Instance.Remove(this);
            Disable();
        }

        protected virtual void OnInit(params object[] args){}

        /// <summary>
        /// 实体生命周期
        /// </summary>
        protected virtual void OnUpdate() { }
        
        /// <summary>
        /// 自定义实体启用
        /// </summary>
        protected virtual void Enable() { }

        /// <summary>
        /// 自定义实体禁用
        /// </summary>
        protected virtual void Disable() { }
        
        int IEntity.Id { get; set; }

        void IEntity.OnInit(params object[] args) => OnInit(args);

        void IEntity.OnUpdate() => OnUpdate();
    }
}