namespace JFramework.Interface
{
    /// <summary>
    /// 状态机接口
    /// </summary>
    public interface IMachine
    {
        /// <summary>
        /// 增加状态
        /// </summary>
        /// <param name="state">传入增加的状态</param>
        /// <typeparam name="T">传入继承IState的状态</typeparam>
        void AddState<T>(IState state = null) where T : IState, new();

        /// <summary>
        /// 改变状态
        /// </summary>
        /// <typeparam name="T">传入继承IState的状态</typeparam>
        void ChangeState<T>() where T : IState;

        /// <summary>
        /// 延迟改变状态
        /// </summary>
        /// <param name="time">延迟时间</param>
        /// <typeparam name="T">传入继承IState的状态</typeparam>
        void ChangeState<T>(float time) where T : IState;

        /// <summary>
        /// 移除状态
        /// </summary>
        /// <typeparam name="T">传入继承IState的状态</typeparam>
        void RemoveState<T>() where T : IState;
    }
}