// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-02-04  18:32
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using JFramework.Core;
using JFramework.Interface;
using Sirenix.OdinInspector;

// ReSharper disable All

namespace JFramework
{
    [Serializable]
    public abstract class State<T> : IState where T : IEntity
    {
        public T owner { get; private set; }

        public IStateMachine machine { get; private set; }

        protected abstract void OnEnter();

        protected abstract void OnUpdate();

        protected abstract void OnExit();

        void IState.OnAwake(IEntity owner, IStateMachine machine)
        {
            this.owner = (T)owner;
            this.machine = machine;
        }

        void IState.OnEnter() => OnEnter();

        void IState.OnUpdate() => OnUpdate();

        void IState.OnExit() => OnExit();
    }

    [Serializable]
    public abstract class StateMachine<T1> : Component<T1>, IStateMachine where T1 : IEntity
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
            var state = StreamPool.Pop<IState>(typeof(T2));
            state.OnAwake(owner, this);
            states[typeof(T2)] = state;
        }

        public void AddState<T2, T3>() where T2 : IState where T3 : IState, new()
        {
            var state = StreamPool.Pop<IState>(typeof(T3));
            state.OnAwake(owner, this);
            states[typeof(T2)] = state;
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
            foreach (var state in states.Values)
            {
                StreamPool.Push(state, state.GetType());
            }

            states.Clear();
        }
    }
}