// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-10-24  23:58
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

namespace JFramework.Interface
{
    /// <summary>
    /// 事件接口
    /// </summary>
    public interface IEvent
    {
    }

    /// <summary>
    /// 泛型事件接口
    /// </summary>
    /// <typeparam name="T">泛型事件数据</typeparam>
    public interface IEvent<in T> : IEvent where T : struct, IEvent
    {
        /// <summary>
        /// 事件被广播
        /// </summary>
        /// <param name="message">传入事件数据</param>
        void Execute(T message);
    }
}