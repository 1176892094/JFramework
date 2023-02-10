using System.Collections.Generic;
using JFramework.Core;
using JFramework.Interface;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JFramework
{
    /// <summary>
    /// 状态机的抽象类(继承于控制器)
    /// </summary>
    public abstract class Machine<T> : Controller<T> where T : MonoBehaviour, IEntity
    {
        /// <summary>
        /// 存储状态的字典
        /// </summary>
        [ShowInInspector, LabelText("持有状态")] private Dictionary<string, IState> stateDict;

        /// <summary>
        /// 状态的接口
        /// </summary>
        [ShowInInspector, LabelText("当前状态"), SerializeField] protected IState currentState;

        /// <summary>
        /// 状态机初始化
        /// </summary>
        /// <param name="owner">状态机的所有者</param>
        protected override void OnInit(T owner)
        {
            base.OnInit(owner);
            stateDict = new Dictionary<string, IState>();
        }

        /// <summary>
        /// 状态机进行更新
        /// </summary>
        public virtual void OnUpdate() => currentState?.OnUpdate();

        /// <summary>
        /// 固定时间更新
        /// </summary>
        public virtual void FixedUpdate() => currentState?.FixedUpdate();

        /// <summary>
        /// 添加状态机的状态
        /// </summary>
        /// <typeparam name="T2">添加的状态类型</typeparam>
        public void AddState<T2>() where T2 : IState, new()
        {
            var key = typeof(T2).Name;
            if (!stateDict.ContainsKey(key))
            {
                var newState = new T2();
                stateDict.Add(key, newState);
                newState.OnInit(owner);
            }
        }


        /// <summary>
        /// 切换状态机的状态
        /// </summary>
        /// <typeparam name="T2">改变的状态类型</typeparam>
        public void ChangeState<T2>() where T2 : IState
        {
            var key = typeof(T2).Name;
            currentState?.OnExit();
            currentState = stateDict[key];
            currentState?.OnEnter();
        }

        /// <summary>
        /// 延迟切换状态机的状态
        /// </summary>
        /// <param name="time">延迟时间</param>
        /// <typeparam name="T2">改变的状态类型</typeparam>
        public void ChangeState<T2>(float time) where T2 : IState
        {
            TimerManager.Instance.Listen(time, ChangeState<T2>).SetTarget(owner.gameObject);
        }

        /// <summary>
        /// 移除状态机的状态
        /// </summary>
        /// <typeparam name="T2">移除的状态类型</typeparam>
        public void RemoveState<T2>() where T2 : IState
        {
            var key = typeof(T2).Name;
            if (stateDict.ContainsKey(key))
            {
                stateDict.Remove(key);
            }
        }
    }
}