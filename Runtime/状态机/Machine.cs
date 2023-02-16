using System.Collections.Generic;
using JFramework.Interface;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JFramework
{
    public class Machine<T> : Controller<T>, IMachine where T : MonoBehaviour, IEntity
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

        protected override void OnInit()
        {
            stateDict = new Dictionary<string, IState>();
        }

        public void AddState<T1>(IState state = null) where T1 : IState, new()
        {
            var key = typeof(T1).Name;
            if (!stateDict.ContainsKey(key))
            {
                state ??= new T1();
                stateDict.Add(key, state);
                state.OnAwake(owner);
            }
        }

        public void ChangeState<T1>() where T1 : IState
        {
            var key = typeof(T1).Name;
            state?.OnExit();
            state = stateDict[key];
            state?.OnEnter();
        }

        public void ChangeState<T1>(float time) where T1 : IState
        {
            throw new System.NotImplementedException();
        }

        public void RemoveState<T1>() where T1 : IState
        {
            var key = typeof(T1).Name;
            if (stateDict.ContainsKey(key))
            {
                stateDict.Remove(key);
            }
        }
    }
}