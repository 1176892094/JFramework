namespace JFramework.Interface
{
    /// <summary>
    /// 继承后会在管理器Pop时调用 (限定GameObject)
    /// </summary>
    public interface IPop
    {
        /// <summary>
        /// 当从对象池弹出时调用
        /// </summary>
        void OnPop();
    }
}