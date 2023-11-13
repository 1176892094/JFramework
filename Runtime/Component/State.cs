// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-10-24  23:38
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using JFramework.Interface;

namespace JFramework
{
    /// <summary>
    /// 状态的抽象类
    /// </summary>
    [Serializable]
    public abstract class State<T> : IState where T : IEntity
    {
        /// <summary>
        /// 状态的所有者
        /// </summary>
        public T owner { get; private set; }

        /// <summary>
        /// 基本状态机的接口
        /// </summary>
        public IStateMachine machine { get; private set; }

        /// <summary>
        /// 状态是否活跃
        /// </summary>
        public bool isActive { get; private set; }

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
        /// 状态机醒来
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="machine">基本状态机</param>
        void IState.OnAwake(IEntity owner, IStateMachine machine)
        {
            this.owner = (T)owner;
            this.machine = machine;
            OnAwake();
        }

        /// <summary>
        /// 受保护的状态进入方法
        /// </summary>
        void IEnter.OnEnter()
        {
            isActive = true;
            OnEnter();
        }

        /// <summary>
        /// 受保护的状态更新方法
        /// </summary>
        void IUpdate.OnUpdate() => OnUpdate();

        /// <summary>
        /// 受保护的状态退出方法
        /// </summary>
        void IExit.OnExit()
        {
            isActive = false;
            OnExit();
        }
    }
}