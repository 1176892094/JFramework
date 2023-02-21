namespace JFramework.Interface
{
    /// <summary>
    /// 单例接口
    /// </summary>
    public interface ISingleton
    {
        /// <summary>
        /// 单例初始化
        /// </summary>
        void Awake();

        /// <summary>
        /// 清空单例
        /// </summary>
        void Destroy();
    }
}