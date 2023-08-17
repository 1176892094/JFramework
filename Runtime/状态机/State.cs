using System;
using JFramework.Interface;
// ReSharper disable All

namespace JFramework
{
    /// <summary>
    /// 状态的抽象类
    /// </summary>
    [Serializable]
    public abstract class State<TCharacter> : IState where TCharacter :  ICharacter
    {
        /// <summary>
        /// 状态的所有者
        /// </summary>
        protected TCharacter owner;

        /// <summary>
        /// 基本状态机的接口 (用于转换指定状态机)
        /// </summary>
        protected IStateMachine machine;

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
        /// 状态机醒来
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="machine"></param>
        void IState.OnAwake(ICharacter owner, IStateMachine machine)
        {
            this.owner = (TCharacter)owner;
            this.machine = machine;
        }

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