namespace JFramework.Interface
{
    /// <summary>
    /// 状态接口
    /// </summary>
    public interface IState
    {
        /// <summary>
        /// 状态的初始化方法
        /// </summary>
        /// <param name="owner">状态的所有者</param>
        /// <param name="stateMachine">状态机</param>
        void OnAwake(IEntity owner, IStateMachine stateMachine);

        /// <summary>
        /// 进入该状态的方法
        /// </summary>
        void OnEnter();

        /// <summary>
        /// 更新该状态的方法
        /// </summary>
        void OnUpdate();

        /// <summary>
        /// 退出该状态的方法
        /// </summary>
        void OnExit();
    }
}