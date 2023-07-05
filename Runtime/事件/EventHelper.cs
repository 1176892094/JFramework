using System.Collections.Generic;
// ReSharper disable All

namespace JFramework
{
    internal static class EventManager<TEvent> where TEvent : struct, IEvent
    {
        /// <summary>
        /// 存储事件的哈希表
        /// </summary>
        public static HashSet<IEvent> observers = new HashSet<IEvent>();

        /// <summary>
        /// 泛型事件管理器侦听
        /// </summary>
        /// <param name="observer">传入观察的对象接口</param>
        /// <returns>返回是否添加成功</returns>
        public static bool Listen(IEvent<TEvent> observer)
        {
            return observers.Add(observer);
        }

        /// <summary>
        /// 泛型事件管理器移除
        /// </summary>
        /// <param name="observer">传入观察的对象接口</param>
        /// <returns>返回是否移除成功</returns>
        public static bool Remove(IEvent<TEvent> observer)
        {
            return observers.Remove(observer);
        }

        /// <summary>
        /// 泛型事件管理器调用事件
        /// </summary>
        /// <param name="message"></param>
        public static void Invoke(TEvent message)
        {
            foreach (var observer in observers)
            {
                ((IEvent<TEvent>)observer)?.Execute(message);
            }
        }
    }
}