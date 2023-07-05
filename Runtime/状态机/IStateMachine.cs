namespace JFramework.Interface
{
    public interface IStateMachine
    {
        /// <summary>
        /// 状态机添加状态
        /// </summary>
        /// <typeparam name="TState">可传入任何继承IState的对象</typeparam>
        void AddState<TState>() where TState : IState, new();

        /// <summary>
        /// 状态机添加状态
        /// </summary>
        /// <param name="state">添加的状态类型</param>
        /// <typeparam name="TState">可传入任何继承IState的对象</typeparam>
        void AddState<TState>(IState state) where TState : IState;

        /// <summary>
        /// 改变状态
        /// </summary>
        /// <typeparam name="TState">可传入任何继承IState的对象</typeparam>
        void ChangeState<TState>() where TState : IState;

        /// <summary>
        /// 延迟改变状态
        /// </summary>
        /// <param name="time">延迟时间</param>
        /// <typeparam name="TState">可传入任何继承IState的对象</typeparam>
        void ChangeState<TState>(float time) where TState : IState;
    }
}