using System;
using System.Collections.Generic;
using JFramework.Async;
using UnityEngine;

namespace JFramework.Basic
{
    public class BaseStateMachine : BaseBehaviour
    {
        protected Dictionary<string, IState> stateDict = new Dictionary<string, IState>();
        protected IState state;

        protected override void OnUpdate() => state.OnUpdate();

        public void ChangeState(string type)
        {
            state?.OnExit();
            state = stateDict[type];
            state.OnEnter();
        }

        public async void ChangeState(string type, float time)
        {
            await new WaitForSeconds(time);
            if (this == null) return;
            ChangeState(type);
        }
    }
}