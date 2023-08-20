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