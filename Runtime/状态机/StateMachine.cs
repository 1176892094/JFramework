using System;
using System.Collections.Generic;
using JFramework.Core;
using JFramework.Interface;
using Sirenix.OdinInspector;
using UnityEngine;

// ReSharper disable All
namespace JFramework
{
    public abstract class StateMachine<TCharacter> : Controller<TCharacter>, IStateMachine where TCharacter : ICharacter
    {
        /// <summary>
        /// 存储状态的字典
        /// </summary>
        [ShowInInspector, LabelText("持有状态")]
        private Dictionary<Type, IState> stateDict = new Dictionary<Type, IState>();

        /// <summary>
        /// 状态的接口
        /// </summary>
        [ShowInInspector, LabelText("当前状态"), SerializeField]
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
            if (stateDict.ContainsKey(typeof(TState))) return;
            state = new TState();
            stateDict.Add(typeof(TState), state);
            state.OnAwake(owner, this);
        }

        /// <summary>
        /// 状态机添加状态
        /// </summary>
        /// <param name="state">添加的状态类型</param>
        /// <typeparam name="TState">可传入任何继承IState的对象</typeparam>
        public void AddState<TState>(IState state) where TState : IState
        {
            if (stateDict.ContainsKey(typeof(TState))) return;
            stateDict.Add(typeof(TState), state);
            state.OnAwake(owner, this);
        }

        /// <summary>
        /// 改变状态
        /// </summary>
        /// <typeparam name="TState">可传入任何继承IState的对象</typeparam>
        public void ChangeState<TState>() where TState : IState
        {
            state?.OnExit();
            state = stateDict[typeof(TState)];
            state?.OnEnter();
        }

        /// <summary>
        /// 延迟改变状态
        /// </summary>
        /// <param name="duration">延迟时间</param>
        /// <typeparam name="TState">可传入任何继承IState的对象</typeparam>
        public void ChangeState<TState>(float duration) where TState : IState
        {
            TimerManager.Pop(duration, ChangeState<TState>);
        }
    }
}