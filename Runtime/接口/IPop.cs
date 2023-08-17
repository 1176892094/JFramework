namespace JFramework.Interface
{
    /// <summary>
    /// 继承后会在 PoolManager.Pop 时调用 (限定GameObject)
    /// </summary>
    public interface IPop
    {
        /// <summary>
        /// 当从对象池弹出时调用
        /// </summary>
        void OnPop();
    }
}