namespace JFramework.Interface
{
    public interface IPanel : IEntity
    {
        /// <summary>
        /// UI层级
        /// </summary>
        UILayerType layerType { get; }
        
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