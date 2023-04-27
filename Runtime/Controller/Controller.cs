using JFramework.Interface;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JFramework
{
    /// <summary>
    /// 控制器的抽象类
    /// </summary>
    /// <typeparam name="TEntity">实现了IEntity接口的类型</typeparam>
    public abstract class Controller<TEntity> : SerializedScriptableObject, IController where TEntity : IEntity
    {
        /// <summary>
        /// 控制器的所有者
        /// </summary>
        [HideInInspector] public TEntity owner;

        /// <summary>
        /// 控制器初始化
        /// </summary>
        /// <param name="owner">传入控制器的所有者</param>
        private void Spawn(TEntity owner)
        {
            this.owner = owner;
            Spawn();
        }

        /// <summary>
        /// 控制器初始化
        /// </summary>
        protected virtual void Spawn() { }

        /// <summary>
        /// 通过接口初始化控制器
        /// </summary>
        /// <param name="owner">控制器的所有者</param>
        void IController.Spawn(IEntity owner) => Spawn((TEntity)owner);

        /// <summary>
        /// 控制器清除
        /// </summary>
        void IController.Despawn() => Destroy(this);
    }
}