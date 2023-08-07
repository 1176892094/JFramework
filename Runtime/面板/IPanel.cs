namespace JFramework.Interface
{
    public interface IPanel : IEntity
    {
        /// <summary>
        /// 是否为活跃的
        /// </summary>
        bool isActive { get; }
        
        /// <summary>
        /// UI面板状态
        /// </summary>
        UIStateType stateType { get; }

        /// <summary>
        /// 显示
        /// </summary>
        void Show();

        /// <summary>
        /// 隐藏
        /// </summary>
        void Hide();
    }
}