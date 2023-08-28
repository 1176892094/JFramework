namespace JFramework.Interface
{
    /// <summary>
    /// 状态接口
    /// </summary>
    public interface IState : IUpdate, IEnter, IExit
    {
        /// <summary>
        /// 状态的初始化方法
        /// </summary>
        /// <param name="owner">状态的所有者</param>
        /// <param name="machine">状态机</param>
        void OnAwake(ICharacter owner, IStateMachine machine);
    }

    /// <summary>
    /// 更新接口
    /// </summary>
    public interface IUpdate
    {
        /// <summary>
        /// 状态更新
        /// </summary>
        void OnUpdate();
    }

    /// <summary>
    /// 进入接口
    /// </summary>
    public interface IEnter
    {
        /// <summary>
        /// 进入状态
        /// </summary>
        void OnEnter();
    }

    /// <summary>
    /// 退出接口
    /// </summary>
    public interface IExit
    {
        /// <summary>
        /// 退出状态
        /// </summary>
        void OnExit();
    }
}