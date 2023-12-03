// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-10-24  23:56
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

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
        bool Push(T obj);
    }
}