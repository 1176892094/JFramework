using JFramework.Interface;

namespace JFramework
{
    /// <summary>
    /// 对象池的抽象类
    /// </summary>
    /// <typeparam name="T">传入对象池的对象类型</typeparam>
    public abstract class Pool<T> : IPool where T : class
    {
        /// <summary>
        /// 对象池物体数量
        /// </summary>
        public abstract int Count { get; }

        /// <summary>
        /// 取出对象
        /// </summary>
        /// <returns>返回对象</returns>
        protected abstract T Pop();

        /// <summary>
        /// 推入对象
        /// </summary>
        /// <param name="obj">传入推入的对象</param>
        protected abstract void Push(T obj);

        /// <summary>
        /// 通过接口取出对象
        /// </summary>
        /// <returns>返回对象</returns>
        object IPool.Pop() => Pop();

        /// <summary>
        /// 通过接口推入对象
        /// </summary>
        /// <param name="obj">传入推入的对象</param>
        void IPool.Push(object obj) => Push((T)obj);
    }
}