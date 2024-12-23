// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2024-12-24 01:12:30
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;

namespace JFramework
{
    public abstract class Agent<T> : IAgent
    {
        public T owner { get; private set; }

        void IAgent.OnAwake(object owner)
        {
            this.owner = (T)owner;
            Awake();
        }

        void IDisposable.Dispose()
        {
            Dispose();
            owner = default;
        }

        protected virtual void Awake()
        {
        }

        protected virtual void Dispose()
        {
        }
    }
}