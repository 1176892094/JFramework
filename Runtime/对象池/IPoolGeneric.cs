namespace JFramework.Interface
{
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
        bool Push(T obj);
    }
}