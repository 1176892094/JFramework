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

    /// <summary>
    /// 泛型对象池接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPool<T> : IPool
    {
        /// <summary>
        /// 弹出对象
        /// </summary>
        /// <returns>返回对象</returns>
        T Pop();

        /// <summary>
        /// 推入对象
        /// </summary>
        /// <param name="obj">传入推入的对象</param>
        void Push(T obj);
    }

    /// <summary>
    /// 继承后会在 Pop 时调用
    /// </summary>
    public interface IPop
    {
        /// <summary>
        /// 当从对象池弹出时调用
        /// </summary>
        void OnPop();
    }

    /// <summary>
    /// 继承后会在 Push 时调用
    /// </summary>
    public interface IPush
    {
        /// <summary>
        /// 当推入对象池时调用
        /// </summary>
        void OnPush();
    }
}