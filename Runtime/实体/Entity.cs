using System;
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
        /// 实体销毁
        /// </summary>
        public virtual void Despawn() { }

        /// <summary>
        /// 实体更新
        /// </summary>`
        protected virtual void OnUpdate() { }

        /// <summary>
        /// 实体启用
        /// </summary>
        protected virtual void OnEnable() => ((IEntity)this).Enable();

        /// <summary>
        /// 实体禁用
        /// </summary>
        protected virtual void OnDisable() => ((IEntity)this).Disable();

        /// <summary>
        /// 实体销毁
        /// </summary>
        private void OnDestroy() => ((IEntity)this).Destroy();

        /// <summary>
        /// 实体接口调用实体更新方法
        /// </summary>
        void IEntity.Update() => OnUpdate();
    }
}