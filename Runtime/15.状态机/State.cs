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
using JFramework.Interface;

namespace JFramework
{
    [Serializable]
    public abstract class State<T> : IState where T : IEntity
    {
        public T owner { get; private set; }

        protected abstract void OnEnter();

        protected abstract void OnUpdate();

        protected abstract void OnExit();

        void IState.OnAwake(IEntity owner) => this.owner = (T)owner;

        void IState.OnEnter() => OnEnter();

        void IState.OnUpdate() => OnUpdate();

        void IState.OnExit() => OnExit();
    }
}