using System;
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
        /// 实体Id
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 实体更新
        /// </summary>`
        protected virtual void OnUpdate() { }

        /// <summary>
        /// 实体启用
        /// </summary>
        protected virtual void OnEnable() => GlobalManager.Listen(this);

        /// <summary>
        /// 实体禁用
        /// </summary>
        protected virtual void OnDisable() => GlobalManager.Remove(this);

        /// <summary>
        /// 实体Id
        /// </summary>
        int IEntity.Id
        {
            get => Id;
            set => Id = value;
        }
        
        /// <summary>
        /// 实体Transform
        /// </summary>
        Transform IEntity.transform => transform;

        /// <summary>
        /// 实体GameObject
        /// </summary>
        GameObject IEntity.gameObject => gameObject;

        /// <summary>
        /// 实体接口调用实体更新方法
        /// </summary>
        void IEntity.Update() => OnUpdate();
    }
}