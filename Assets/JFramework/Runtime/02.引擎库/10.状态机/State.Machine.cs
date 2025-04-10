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

        public override void OnHide()
        {
            base.OnHide();
            var copies = new List<IState>(states.Values);
            foreach (var stateData in copies)
            {
                stateData.OnHide();
                HeapManager.Enqueue(stateData, stateData.GetType());
            }

            states.Clear();
            state = null;
        }

        public override void OnUpdate()
        {
            state?.OnUpdate();
        }
        
        public void AddState<T>(Type stateType)
        {
            var stateData = HeapManager.Dequeue<IState>(stateType);
            states[typeof(T)] = stateData;
            stateData.OnShow(owner);
        }

        public void ChangeState<T>()
        {
            state?.OnExit();
            state = states[typeof(T)];
            state?.OnEnter();
        }

        public void RemoveState<T>()
        {
            if (states.TryGetValue(typeof(T), out var stateData))
            {
                stateData.OnHide();
                states.Remove(typeof(T));
                HeapManager.Enqueue(stateData, stateData.GetType());
            }
        }
    }
}