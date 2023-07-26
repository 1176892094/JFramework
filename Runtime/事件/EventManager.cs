using System;
using System.Collections.Generic;
using JFramework.Interface;


namespace JFramework.Core
{
    public static class EventManager
    {
        /// <summary>
        /// 事件观察字典
        /// </summary>
        internal static readonly Dictionary<Type, HashSet<IEvent>> events = new Dictionary<Type, HashSet<IEvent>>();

        /// <summary>
        /// 事件管理器侦听事件
        /// </summary>
        /// <param name="event">传入观察的游戏对象</param>
        /// <typeparam name="TEvent">事件类型</typeparam>
        /// <returns>返回是否能被侦听</returns>
        public static bool Listen<TEvent>(IEvent<TEvent> @event) where TEvent : struct, IEvent
        {
            if (!GlobalManager.Runtime) return false;
            if (!events.ContainsKey(typeof(TEvent)))
            {
                events.Add(typeof(TEvent), Event<TEvent>.events = new HashSet<IEvent>());
            }

            Log.Info(DebugOption.Event, $"侦听 => {@event} {typeof(TEvent).Name.Yellow()} 事件");
            return Event<TEvent>.Listen(@event);
        }

        /// <summary>
        /// 事件管理器移除事件
        /// </summary>
        /// <param name="event">传入观察的游戏对象</param>
        /// <typeparam name="TEvent">事件类型</typeparam>
        /// <returns>返回是否能被移除</returns>
        public static bool Remove<TEvent>(IEvent<TEvent> @event) where TEvent : struct, IEvent
        {
            if (!GlobalManager.Runtime) return false;
            Log.Info(DebugOption.Event, $"移除 => {@event} {typeof(TEvent).Name.Yellow()} 事件");
            return Event<TEvent>.Remove(@event);
        }

        /// <summary>
        /// 事件管理器广播事件
        /// </summary>
        /// <param name="event">传入观察事件数据</param>
        /// <typeparam name="TEvent">事件类型</typeparam>
        public static void Invoke<TEvent>(TEvent @event = default) where TEvent : struct, IEvent
        {
            if (!GlobalManager.Runtime) return;
            Log.Info(DebugOption.Event, $"触发 => {@event} {typeof(TEvent).Name.Yellow()} 事件");
            Event<TEvent>.Invoke(@event);
        }

        /// <summary>
        /// 事件管理器销毁并释放
        /// </summary>
        internal static void Destroy()
        {
            foreach (var @event in events.Values)
            {
                @event.Clear();
            }

            events.Clear();
        }
    }
}