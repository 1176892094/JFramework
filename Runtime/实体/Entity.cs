using System;
using JFramework.Core;
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
        
        protected virtual void Start() { }
        
        [Obsolete("[JFramework] 请使用OnUpdate来代替Update管理生命周期", true)]
        private void Update() { }

        protected virtual void OnDestroy() { }

        /// <summary>
        /// 实体启用
        /// </summary>
        private void OnEnable()
        {
            GlobalManager.Instance.Listen(OnUpdate);
            Enable();
        }

        /// <summary>
        /// 实体禁用
        /// </summary>
        private void OnDisable()
        {
            if (!GlobalManager.Instance) return;
            GlobalManager.Instance.Remove(OnUpdate);
            Disable();
        }
        
        /// <summary>
        /// 实体初始化
        /// </summary>
        /// <param name="args">初始化参数</param>
        protected virtual void OnInit(params object[] args) { }

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

        /// <summary>
        /// 通过接口初始化
        /// </summary>
        /// <param name="args">初始化参数</param>
        void IEntity.OnInit(params object[] args) => OnInit(args);
    }
    
    public static class EntityExtension
    {
        public static T Get<T>(this IEntity entity) where T : IEntity
        {
            return (T)entity;
        }
    }
}