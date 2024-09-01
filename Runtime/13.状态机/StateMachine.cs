// *********************************************************************************
// # Project: Forest
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-08-25  01:08
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using JFramework.Interface;
using Sirenix.OdinInspector;

namespace JFramework
{
    [Serializable]
    public abstract class StateMachine<T1> : Component<T1> where T1 : IEntity
    {
        [ShowInInspector] private readonly Dictionary<Type, IState> states = new Dictionary<Type, IState>();
        [ShowInInspector] private IState state;

        public void OnUpdate() => state?.OnUpdate();

        public bool IsActive<T2>() where T2 : IState
        {
            return state?.GetType() == typeof(T2);
        }

        public void AddState<T2>() where T2 : IState, new()
        {
            var newState = PoolManager.Dequeue<IState>(typeof(T2));
            states[typeof(T2)] = newState;
            newState.OnAwake(owner);
        }

        public void AddState<T2>(Type type) where T2 : IState
        {
            var newState = PoolManager.Dequeue<IState>(type);
            states[typeof(T2)] = newState;
            newState.OnAwake(owner);
        }

        public void ChangeState<T2>() where T2 : IState
        {
            if (!SceneManager.isLoading)
            {
                if (owner.gameObject.activeInHierarchy)
                {
                    state?.OnExit();
                    state = states[typeof(T2)];
                    state?.OnEnter();
                }
            }
        }

        protected virtual void OnDestroy()
        {
            states.Clear();
        }
    }
}