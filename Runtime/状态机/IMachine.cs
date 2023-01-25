namespace JFramework.Interface
{
    /// <summary>
    /// 状态机接口
    /// </summary>
    public interface IMachine : IEntity
    {
        /// <summary>
        /// 添加状态机的状态
        /// </summary>
        /// <param name="name">状态名称</param>
        /// <param name="state">状态</param>
        void ListenState(string name, IState state);

        /// <summary>
        /// 切换状态机的状态
        /// </summary>
        /// <param name="name">状态名称</param>
        void ChangeState(string name);

        /// <summary>
        /// 延迟切换状态机的状态
        /// </summary>
        /// <param name="name">状态名称</param>
        /// <param name="time">延迟时间</param>
        void ChangeState(string name, float time);

        /// <summary>
        /// 移除状态机的状态
        /// </summary>
        /// <param name="name">状态名称</param>
        void RemoveState(string name);
    }
}