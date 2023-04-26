using System;
using System.Collections.Generic;
using JFramework.Core;
using JFramework.Interface;
using Sirenix.OdinInspector;
using UnityEngine;

// ReSharper disable All
namespace JFramework
{
    public abstract class StateMachine<T> : Controller<T> where T : MonoBehaviour, IEntity
    {
        /// <summary>
        /// 存储状态的字典
        /// </summary>
        [ShowInInspector, LabelText("持有状态")] 
        private Dictionary<Type, IState> stateDict;

        /// <summary>
        /// 状态的接口
        /// </summary>
        [ShowInInspector, LabelText("当前状态"), SerializeField]
        protected IState state;

        /// <summary>
        /// 状态机启动
        /// </summary>
        /// <typeparam name="TState">可传入任何继承IState的对象</typeparam>
        public void Enable<TState>() where TState : IState, new() => ChangeState<TState>();

        /// <summary>
        /// 状态机更新
        /// </summary>
        public void OnUpdate() => state?.OnUpdate();

        /// <summary>
        /// 状态机添加状态
        /// </summary>
        /// <param name="state">添加的状态类型</param>
        /// <typeparam name="TState">可传入任何继承IState的对象</typeparam>
        public void AddState<TState>(IState state = null) where TState : IState, new()
        {
            var key = typeof(TState);
            stateDict ??= new Dictionary<Type, IState>();
            if (stateDict.ContainsKey(key)) return;
            state ??= new TState();
            stateDict.Add(key, state);
            state.OnAwake(owner);
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
        /// <param name="time">延迟时间</param>
        /// <typeparam name="TState">可传入任何继承IState的对象</typeparam>
        public void ChangeState<TState>(float time) where TState : IState
        {
            TimerManager.Pop(time, ChangeState<TState>);
        }
        
        /// <summary>
        /// 状态机禁用
        /// </summary>
        public void Disable() => state = null;
    }
}