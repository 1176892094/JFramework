using System;
using System.Collections.Generic;


namespace JFramework.Core
{
    public static class EventManager
    {
        /// <summary>
        /// 事件观察字典
        /// </summary>
        internal static readonly Dictionary<Type, HashSet<IEvent>> observerDict = new Dictionary<Type, HashSet<IEvent>>();

        /// <summary>
        /// 事件管理器侦听事件
        /// </summary>
        /// <param name="observer">传入观察的游戏对象</param>
        /// <typeparam name="TEvent">事件类型</typeparam>
        /// <returns>返回是否能被侦听</returns>
        public static bool Listen<TEvent>(IEvent<TEvent> observer) where TEvent : struct, IEvent
        {
            if (!GlobalManager.Runtime) return false;
            if (!observerDict.ContainsKey(typeof(TEvent)))
            {
                EventManager<TEvent>.observers = new HashSet<IEvent>();
                observerDict.Add(typeof(TEvent), EventManager<TEvent>.observers);
            }

            Log.Info(DebugOption.Event, $"侦听 => {observer} " + $"IEvent<{typeof(TEvent).Name}>".Yellow() + " 事件");
            return EventManager<TEvent>.Listen(observer);
        }

        /// <summary>
        /// 事件管理器移除事件
        /// </summary>
        /// <param name="observer">传入观察的游戏对象</param>
        /// <typeparam name="TEvent">事件类型</typeparam>
        /// <returns>返回是否能被移除</returns>
        public static bool Remove<TEvent>(IEvent<TEvent> observer) where TEvent : struct, IEvent
        {
            if (!GlobalManager.Runtime) return false;
            Log.Info(DebugOption.Event, $"移除 => {observer} " + $"IEvent<{typeof(TEvent).Name}>".Yellow() + " 事件");
            return EventManager<TEvent>.Remove(observer);
        }

        /// <summary>
        /// 事件管理器广播事件
        /// </summary>
        /// <param name="observer">传入观察事件数据</param>
        /// <typeparam name="TEvent">事件类型</typeparam>
        public static void Invoke<TEvent>(TEvent observer = default) where TEvent : struct, IEvent
        {
            if (!GlobalManager.Runtime) return;
            Log.Info(DebugOption.Event, $"触发 => {observer} " + $"IEvent<{typeof(TEvent).Name}>".Yellow() + " 事件");
            EventManager<TEvent>.Invoke(observer);
        }

        /// <summary>
        /// 事件管理器销毁并释放
        /// </summary>
        internal static void Destroy()
        {
            foreach (var hashSet in observerDict.Values)
            {
                hashSet.Clear();
            }

            observerDict.Clear();
        }
    }
}