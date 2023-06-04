namespace JFramework.Interface
{
    public interface IPool
    {
        /// <summary>
        /// 容器物体数量
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 弹出对象
        /// </summary>
        /// <returns>返回对象</returns>
        object Pop();

        /// <summary>
        /// 推入对象
        /// </summary>
        /// <param name="obj">传入推入的对象</param>
        void Push(object obj);
    }
}