using System.Collections.Generic;
using JFramework.Core;
using JFramework.Interface;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JFramework
{
    public class Machine<T> : Controller<T> where T : MonoBehaviour, IEntity
    {
        /// <summary>
        /// 存储状态的字典
        /// </summary>
        [ShowInInspector, LabelText("持有状态")] 
        private Dictionary<string, IState> stateDict;

        /// <summary>
        /// 状态的接口
        /// </summary>
        [ShowInInspector, LabelText("当前状态"), SerializeField]
        protected IState state;

        /// <summary>
        /// 状态机初始化
        /// </summary>
        protected override void Start() => stateDict = new Dictionary<string, IState>();

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
            var key = typeof(TState).Name;
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
            state = stateDict[typeof(TState).Name];
            state?.OnEnter();
            if (owner.name=="32001Enemy")
            {
                Debug.Log(owner.name+"-----"+state);
            }
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
        /// 移除状态
        /// </summary>
        /// <typeparam name="TState">可传入任何继承IState的对象</typeparam>
        public void RemoveState<TState>() where TState : IState
        {
            var key = typeof(TState).Name;
            if (stateDict.ContainsKey(key))
            {
                stateDict.Remove(key);
            }
        }
    }
}