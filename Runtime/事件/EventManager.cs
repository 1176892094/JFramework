using System.Collections.Generic;
using JFramework.Core;
using UnityEngine;

namespace JFramework
{
    /// <summary>
    /// 可变长参数委托
    /// </summary>
    public delegate void EventData(params object[] args);

    /// <summary>
    /// 事件管理器
    /// </summary>
    public static class EventManager
    {
        /// <summary>
        /// 事件字典
        /// </summary>
        internal static Dictionary<int, EventData> eventDict;

        /// <summary>
        /// 管理器名称
        /// </summary>
        private static string Name => nameof(EventManager);

        /// <summary>
        /// 事件管理器醒来
        /// </summary>
        internal static void Awake() => eventDict = new Dictionary<int, EventData>();

        /// <summary>
        /// 侦听事件
        /// </summary>
        /// <param name="id">事件唯一标识</param>
        /// <param name="action">传入侦听的事件</param>
        public static void Listen(int id, EventData action)
        {
            if (eventDict == null) return;
            if (eventDict.ContainsKey(id))
            {
                eventDict[id] += action;
            }
            else
            {
                eventDict.Add(id, action);
            }

            if (DebugManager.IsDebugEvent)
            {
                Debug.Log($"{Name.Sky()} 侦听 => {action.Method.ToString().Yellow()} 事件");
            }
        }

        /// <summary>
        /// 移除事件
        /// </summary>
        /// <param name="id">事件唯一标识</param>
        /// <param name="action">传入移除的事件</param>
        public static void Remove(int id, EventData action)
        {
            if (eventDict == null) return;
            if (eventDict.ContainsKey(id))
            {
                if (DebugManager.IsDebugEvent)
                {
                    Debug.Log($"{Name.Sky()} 移除 => {action.Method.ToString().Yellow()} 事件");
                }

                eventDict[id] -= action;
            }
        }

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="id">事件唯一标识</param>
        /// <param name="value">传入事件的参数</param>
        public static void Send(int id, params object[] value)
        {
            if (eventDict == null) return;
            if (eventDict.ContainsKey(id))
            {
                if (DebugManager.IsDebugEvent)
                {
                    Debug.Log($"{Name.Sky()} 触发 => {eventDict[id]?.Method.ToString().Yellow()} 事件");
                }

                eventDict[id]?.Invoke(value);
            }
        }

        /// <summary>
        /// 清空事件管理器
        /// </summary>
        internal static void Destroy() => eventDict = null;
    }
}