using System;
using JFramework.Interface;
using UnityEngine;

namespace JFramework
{
    /// <summary>
    /// 状态的抽象类
    /// </summary>
    [Serializable]
    public abstract class State<TEntity> : IState where TEntity : MonoBehaviour, IEntity
    {
        /// <summary>
        /// 状态的所有者
        /// </summary>
        protected TEntity owner;

        /// <summary>
        /// 状态机的接口
        /// </summary>
        protected IStateMachine stateMachine;

        /// <summary>
        /// 状态醒来
        /// </summary>
        /// <param name="owner">传入状态的所有者</param>
        /// <param name="stateMachine">传入状态机</param>
        private void OnAwake(TEntity owner, IStateMachine stateMachine)
        {
            this.owner = owner;
            this.stateMachine = stateMachine;
            OnAwake();
        }

        /// <summary>
        /// 状态初始化
        /// </summary>
        protected virtual void OnAwake()
        {
        }

        /// <summary>
        /// 进入状态
        /// </summary>
        protected abstract void OnEnter();

        /// <summary>
        /// 状态更新
        /// </summary>
        protected abstract void OnUpdate();

        /// <summary>
        /// 退出状态
        /// </summary>
        protected abstract void OnExit();

        /// <summary>
        /// 受保护的状态醒来方法
        /// </summary>
        /// <param name="owner">传入状态的所有者</param>
        /// <param name="stateMachine">传入状态机</param>
        void IState.OnAwake(IEntity owner, IStateMachine stateMachine) => OnAwake((TEntity)owner, stateMachine);

        /// <summary>
        /// 受保护的状态进入方法
        /// </summary>
        void IState.OnEnter() => OnEnter();

        /// <summary>
        /// 受保护的状态更新方法
        /// </summary>
        void IState.OnUpdate() => OnUpdate();

        /// <summary>
        /// 受保护的状态退出方法
        /// </summary>
        void IState.OnExit() => OnExit();
    }
}