namespace JFramework.Interface
{
    /// <summary>
    /// 状态接口
    /// </summary>
    public interface IState
    {
        /// <summary>
        /// 进入状态的初始化方法
        /// </summary>
        /// <param name="owner">可传入所有者的信息</param>
        void OnInit(object owner);

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