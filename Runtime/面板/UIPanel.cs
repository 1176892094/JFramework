namespace JFramework
{
    /// <summary>
    /// UI面板的抽象类
    /// </summary>
    public abstract class UIPanel : Entity
    {
        /// <summary>
        /// 显示UI面板
        /// </summary>
        public virtual void Show()
        {
        }

        /// <summary>
        /// 隐藏UI面板
        /// </summary>
        public virtual void Hide()
        {
        }
    }
}