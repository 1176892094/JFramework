using System;
using JFramework.Interface;

namespace JFramework
{
    /// <summary>
    /// 状态的抽象类
    /// </summary>
    [Serializable]
    public abstract class State<T> : IState where T : ICharacter
    {
        /// <summary>
        /// 状态的所有者
        /// </summary>
        protected T owner;

        /// <summary>
        /// 基本状态机的接口
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
        /// <param name="owner">状态机的所有者</param>
        /// <param name="machine">基本状态机</param>
        void IState.OnAwake(ICharacter owner, IStateMachine machine)
        {
            this.owner = (T)owner;
            this.machine = machine;
        }

        /// <summary>
        /// 受保护的状态进入方法
        /// </summary>
        void IEnter.OnEnter() => OnEnter();

        /// <summary>
        /// 受保护的状态更新方法
        /// </summary>
        void IUpdate.OnUpdate() => OnUpdate();

        /// <summary>
        /// 受保护的状态退出方法
        /// </summary>
        void IExit.OnExit() => OnExit();
    }
}