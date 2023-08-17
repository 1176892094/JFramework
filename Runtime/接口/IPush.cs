namespace JFramework.Interface
{
    /// <summary>
    /// 继承后会在 PoolManager.Push 时调用 (限定GameObject)
    /// </summary>
    public interface IPush
    {
        /// <summary>
        /// 当推入对象池时调用
        /// </summary>
        void OnPush();
    }
}