// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-10-24  23:58
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

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
        /// <typeparam name="T">事件类型</typeparam>
        /// <returns>返回是否能被侦听</returns>
        public static void Listen<T>(IEvent<T> @event) where T : struct, IEvent
        {
            if (!GlobalManager.Runtime) return;
            if (!observers.ContainsKey(typeof(T)))
            {
                observers.Add(typeof(T), Event<T>.events = new HashSet<IEvent>());
            }
            
            Event<T>.Listen(@event);
        }

        /// <summary>
        /// 事件管理器移除事件
        /// </summary>
        /// <param name="event">传入观察的游戏对象</param>
        /// <typeparam name="T">事件类型</typeparam>
        /// <returns>返回是否能被移除</returns>
        public static void Remove<T>(IEvent<T> @event) where T : struct, IEvent
        {
            if (!GlobalManager.Runtime) return;
            Event<T>.Remove(@event);
        }

        /// <summary>
        /// 事件管理器广播事件
        /// </summary>
        /// <param name="event">传入观察事件数据</param>
        /// <typeparam name="T">事件类型</typeparam>
        public static void Invoke<T>(T @event = default) where T : struct, IEvent
        {
            if (!GlobalManager.Runtime) return;
            Event<T>.Invoke(@event);
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