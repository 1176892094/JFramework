// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-10-24  23:36
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

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
        /// <typeparam name="TState">可传入任何继承IState的对象</typeparam>
        /// <typeparam name="TValue"></typeparam>
        void AddState<TState, TValue>() where TState : IState where TValue : IState, new();

        /// <summary>
        /// 改变状态
        /// </summary>
        /// <typeparam name="TState">可传入任何继承IState的对象</typeparam>
        void ChangeState<TState>() where TState : IState;

        /// <summary>
        /// 延迟改变状态
        /// </summary>
        /// <param name="duration">延迟时间</param>
        /// <typeparam name="TState">可传入任何继承IState的对象</typeparam>
        void ChangeState<TState>(float duration) where TState : IState;
    }
}