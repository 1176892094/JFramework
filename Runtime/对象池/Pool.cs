using JFramework.Interface;
using Sirenix.OdinInspector;

namespace JFramework
{
    /// <summary>
    /// 对象池的抽象类
    /// </summary>
    /// <typeparam name="TPool">传入对象池的对象类型</typeparam>
    internal abstract class Pool<TPool> : IPool where TPool : class
    {
        /// <summary>
        /// 对象池容器
        /// </summary>
        [ShowInInspector] protected IPool stackPool;

        /// <summary>
        /// 对象池物体数量
        /// </summary>
        public int Count => stackPool.Count;

        /// <summary>
        /// 取出对象
        /// </summary>
        /// <returns>返回对象</returns>
        protected abstract TPool Pop();

        /// <summary>
        /// 推入对象
        /// </summary>
        /// <param name="obj">传入推入的对象</param>
        protected abstract void Push(TPool obj);

        /// <summary>
        /// 通过接口取出对象
        /// </summary>
        /// <returns>返回对象</returns>
        object IPool.Pop() => Pop();

        /// <summary>
        /// 通过接口推入对象
        /// </summary>
        /// <param name="obj">传入推入的对象</param>
        void IPool.Push(object obj) => Push((TPool)obj);
    }
}