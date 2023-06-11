using System.Collections.Generic;

namespace JFramework.Core
{
    /// <summary>
    /// 可变长参数委托
    /// </summary>
    public delegate void EventHandler(params object[] args);

    /// <summary>
    /// 事件管理器
    /// </summary>
    public static class EventManager
    {
        /// <summary>
        /// 事件字典
        /// </summary>
        internal static Dictionary<int, EventHandler> eventDict;

        /// <summary>
        /// 事件管理器醒来
        /// </summary>
        internal static void Awake() => eventDict = new Dictionary<int, EventHandler>();

        /// <summary>
        /// 侦听事件
        /// </summary>
        /// <param name="id">事件唯一标识</param>
        /// <param name="action">传入侦听的事件</param>
        public static void Listen(int id, EventHandler action)
        {
            if (!GlobalManager.Runtime) return;
            if (eventDict.ContainsKey(id))
            {
                eventDict[id] += action;
            }
            else
            {
                eventDict.Add(id, action);
            }

            GlobalManager.Logger(DebugOption.Event, $"侦听 => {action.Method.ToString().Yellow()} 事件");
        }

        /// <summary>
        /// 移除事件
        /// </summary>
        /// <param name="id">事件唯一标识</param>
        /// <param name="action">传入移除的事件</param>
        public static void Remove(int id, EventHandler action)
        {
            if (!GlobalManager.Runtime) return;
            if (eventDict.ContainsKey(id))
            {
                GlobalManager.Logger(DebugOption.Event, $"移除 => {action.Method.ToString().Yellow()} 事件");
                eventDict[id] -= action;
            }
        }

        /// <summary>
        /// 调用事件
        /// </summary>
        /// <param name="id">事件唯一标识</param>
        /// <param name="value">传入事件的参数</param>
        public static void Invoke(int id, params object[] value)
        {
            if (!GlobalManager.Runtime) return;
            if (eventDict.ContainsKey(id))
            {
                GlobalManager.Logger(DebugOption.Event, $"触发 => {eventDict[id]?.Method.ToString().Yellow()} 事件");
                eventDict[id]?.Invoke(value);
            }
        }

        /// <summary>
        /// 清空事件管理器
        /// </summary>
        internal static void Destroy() => eventDict = null;
    }
}