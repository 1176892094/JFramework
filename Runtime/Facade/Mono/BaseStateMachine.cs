using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JFramework.Basic
{
    public abstract class BaseStateMachine : BaseBehaviour
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

        public void ChangeState(string type, float time)
        {
            MonoManager.Instance.StartCoroutine(WaitForCompleted(type, time));
        }

        private IEnumerator WaitForCompleted(string type, float time)
        {
            yield return new WaitForSeconds(time);
            if (this == null) yield break;
            ChangeState(type);
        }
    }
    
    public interface IState
    {
        void OnEnter();
        
        void OnUpdate();

        void OnExit();
    }
}