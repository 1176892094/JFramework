using JFramework.Interface;

namespace JFramework
{
    /// <summary>
    /// 状态的抽象类
    /// </summary>
    public abstract class State<T> : IState where T : IMachine
    {
        protected T stateMachine;

        private void OnAwake(T stateMachine)
        {
            this.stateMachine = stateMachine;
            OnAwake();
        }

        protected abstract void OnAwake();
        protected abstract void OnEnter();
        protected abstract void OnUpdate();
        protected abstract void OnExit();
        
        void IState.OnAwake(object stateMachine) => OnAwake((T)stateMachine);
        
        void IState.OnEnter() => OnEnter();
        
        void IState.OnUpdate() => OnUpdate();
        
        void IState.OnExit() => OnExit();
    }
}