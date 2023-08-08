namespace JFramework.Interface
{
    /// <summary>
    /// 对象池接口
    /// </summary>
    public interface IPool
    {
        /// <summary>
        /// 容器物体数量
        /// </summary>
        int Count { get; }
        
        /// <summary>
        /// 清空对象池
        /// </summary>
        void Clear();
    }
}