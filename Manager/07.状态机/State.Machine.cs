// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2025-01-08 17:01:34
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using JFramework.Common;
using UnityEngine;

namespace JFramework
{
    public abstract class StateMachine<TOwner> : Agent<TOwner> where TOwner : Component
    {
        private readonly Dictionary<Type, IState> states = new Dictionary<Type, IState>();
        private IState state;

        public override void Dispose()
        {
            base.Dispose();
            var copies = new List<IState>(states.Values);
            foreach (var stateData in copies)
            {
                stateData.Dispose();
                Service.Pool.Enqueue(stateData, stateData.GetType());
            }

            states.Clear();
            state = null;
        }

        public void OnUpdate()
        {
            state.OnUpdate();
        }

        public void AddState<T>()
        {
            var stateData = Service.Pool.Dequeue<IState>(typeof(T));
            states[typeof(T)] = stateData;
            stateData.OnAwake(owner);
        }

        public void AddState<T>(Type stateType)
        {
            var stateData = Service.Pool.Dequeue<IState>(stateType);
            states[typeof(T)] = stateData;
            stateData.OnAwake(owner);
        }

        public void ChangeState<T>() 
        {
            state?.OnExit();
            state = states[typeof(T)];
            state?.OnEnter();
        }

        public void ChangeState(Type stateType)
        {
            state?.OnExit();
            state = states[stateType];
            state?.OnEnter();
        }

        public void RemoveState<T>()
        {
            states.Remove(typeof(T));
        }

        public void RemoveState(Type stateType)
        {
            states.Remove(stateType);
        }
    }
}