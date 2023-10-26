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
using System.Collections.Generic;
using JFramework.Core;
using JFramework.Interface;

// ReSharper disable All

namespace JFramework
{
    /// <summary>
    /// 状态机类
    /// </summary>
    /// <typeparam name="TCharacter"></typeparam>
    [Serializable]
    public abstract class StateMachine<TCharacter> : Controller<TCharacter>, IStateMachine where TCharacter : ICharacter
    {
        /// <summary>
        /// 存储状态的字典
        /// </summary>
        private Dictionary<Type, IState> states = new Dictionary<Type, IState>();

        /// <summary>
        /// 状态的接口
        /// </summary>
        protected IState state;

        /// <summary>
        /// 状态机更新
        /// </summary>
        public virtual void OnUpdate() => state?.OnUpdate();

        /// <summary>
        /// 状态机添加状态
        /// </summary>
        /// <typeparam name="TState">可传入任何继承IState的对象</typeparam>
        public void AddState<TState>() where TState : IState, new()
        {
            var state = new TState();
            state.OnAwake(owner, this);
            states[typeof(TState)] = state;
        }

        /// <summary>
        /// 状态机添加状态
        /// </summary>
        /// <typeparam name="TState">可传入任何继承IState的对象</typeparam>
        /// <typeparam name="TValue">用于重写状态</typeparam>
        public void AddState<TState, TValue>() where TState : IState where TValue : IState, new()
        {
            var state = new TValue();
            state.OnAwake(owner, this);
            states[typeof(TState)] = state;
        }

        /// <summary>
        /// 改变状态
        /// </summary>
        /// <typeparam name="TState">可传入任何继承IState的对象</typeparam>
        public void ChangeState<TState>() where TState : IState
        {
            state?.OnExit();
            state = states[typeof(TState)];
            state?.OnEnter();
        }

        /// <summary>
        /// 延迟改变状态
        /// </summary>
        /// <param name="duration">延迟时间</param>
        /// <typeparam name="TState">可传入任何继承IState的对象</typeparam>
        public void ChangeState<TState>(float duration) where TState : IState
        {
            TimerManager.Pop(duration).Invoke(ChangeState<TState>);
        }
    }
}