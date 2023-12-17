// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-10-24  23:58
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JFramework.Interface;

// ReSharper disable All

namespace JFramework
{
    /// <summary>
    /// 静态泛型事件存储类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal struct Event<T> where T : struct, IEvent
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Listen(IEvent<T> @event) => events.Add(@event);

        /// <summary>
        /// 泛型事件管理器移除
        /// </summary>
        /// <param name="event">传入观察的对象接口</param>
        /// <returns>返回是否移除成功</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Remove(IEvent<T> @event) => events.Remove(@event);

        /// <summary>
        /// 泛型事件管理器调用事件
        /// </summary>
        /// <param name="message"></param>
        public static void Invoke(T message)
        {
            var copies = events.ToHashSet();
            foreach (var @event in copies)
            {
                ((IEvent<T>)@event)?.Execute(message);
            }
        }
    }
}