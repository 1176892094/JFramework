using System.Collections.Generic;
using JFramework.Interface;

// ReSharper disable All

namespace JFramework
{
    internal static class Event<TEvent> where TEvent : struct, IEvent
    {
        /// <summary>
        /// 存储事件的哈希表
        /// </summary>
        public static HashSet<IEvent> events = new HashSet<IEvent>();

        /// <summary>
        /// 泛型事件管理器侦听
        /// </summary>
        /// <param name="event">传入观察的对象接口</param>
        /// <returns>返回是否添加成功</returns>
        public static bool Listen(IEvent<TEvent> @event)
        {
            return events.Add(@event);
        }

        /// <summary>
        /// 泛型事件管理器移除
        /// </summary>
        /// <param name="event">传入观察的对象接口</param>
        /// <returns>返回是否移除成功</returns>
        public static bool Remove(IEvent<TEvent> @event)
        {
            return events.Remove(@event);
        }

        /// <summary>
        /// 泛型事件管理器调用事件
        /// </summary>
        /// <param name="message"></param>
        public static void Invoke(TEvent message)
        {
            foreach (var @event in events)
            {
                ((IEvent<TEvent>)@event)?.Execute(message);
            }
        }
    }
}