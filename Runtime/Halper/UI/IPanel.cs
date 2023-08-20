namespace JFramework.Interface
{
    public interface IPanel : IEntity
    {
        /// <summary>
        /// UI层级
        /// </summary>
        UILayer layer { get; }
        
        /// <summary>
        /// UI面板状态
        /// </summary>
        UIState state { get; }

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