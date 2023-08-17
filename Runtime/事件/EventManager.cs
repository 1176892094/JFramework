using System;
using System.Collections.Generic;
using JFramework.Interface;

namespace JFramework.Core
{
    /// <summary>
    /// 事件管理器
    /// </summary>
    public static class EventManager
    {
        /// <summary>
        /// 事件观察字典
        /// </summary>
        internal static readonly Dictionary<Type, HashSet<IEvent>> observers = new Dictionary<Type, HashSet<IEvent>>();

        /// <summary>
        /// 事件管理器侦听事件
        /// </summary>
        /// <param name="event">传入观察的游戏对象</param>
        /// <typeparam name="TEvent">事件类型</typeparam>
        /// <returns>返回是否能被侦听</returns>
        public static void Listen<TEvent>(IEvent<TEvent> @event) where TEvent : struct, IEvent
        {
            if (!GlobalManager.Runtime) return;
            if (!observers.ContainsKey(typeof(TEvent)))
            {
                observers.Add(typeof(TEvent), Event<TEvent>.events = new HashSet<IEvent>());
            }

            Log.Info($"侦听 => {@event} {typeof(TEvent).Name.Yellow()}", Option.Event);
            Event<TEvent>.Listen(@event);
        }

        /// <summary>
        /// 事件管理器移除事件
        /// </summary>
        /// <param name="event">传入观察的游戏对象</param>
        /// <typeparam name="TEvent">事件类型</typeparam>
        /// <returns>返回是否能被移除</returns>
        public static void Remove<TEvent>(IEvent<TEvent> @event) where TEvent : struct, IEvent
        {
            if (!GlobalManager.Runtime) return;
            Log.Info($"移除 => {@event} {typeof(TEvent).Name.Yellow()}", Option.Event);
            Event<TEvent>.Remove(@event);
        }

        /// <summary>
        /// 事件管理器广播事件
        /// </summary>
        /// <param name="event">传入观察事件数据</param>
        /// <typeparam name="TEvent">事件类型</typeparam>
        public static void Invoke<TEvent>(TEvent @event = default) where TEvent : struct, IEvent
        {
            if (!GlobalManager.Runtime) return;
            Log.Info($"广播 => {@event} {typeof(TEvent).Name.Yellow()}", Option.Event);
            Event<TEvent>.Invoke(@event);
        }

        /// <summary>
        /// 事件管理器在开始游戏前重置
        /// </summary>
        internal static void Clear()
        {
            foreach (var observer in observers.Values)
            {
                observer.Clear();
            }

            observers.Clear();
        }
    }
}