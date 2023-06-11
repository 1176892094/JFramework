using System.Collections.Generic;

namespace JFramework.Core
{
    public static class EventManager
    {
        internal static Dictionary<int, EventHandler> eventDict;
        internal static void Awake() => eventDict = new Dictionary<int, EventHandler>();
        internal static void Destroy() => eventDict = null;

        /// <summary>
        /// 侦听事件
        /// </summary>
        /// <param name="id">事件唯一标识</param>
        /// <param name="eventHandler">传入侦听的事件</param>
        public static void Listen(int id, EventHandler eventHandler)
        {
            if (!GlobalManager.Runtime) return;
            if (eventDict.ContainsKey(id))
            {
                eventDict[id] += eventHandler;
            }
            else
            {
                eventDict.Add(id, eventHandler);
            }
        }

        /// <summary>
        /// 移除事件
        /// </summary>
        /// <param name="id">事件唯一标识</param>
        /// <param name="eventHandler">传入移除的事件</param>
        public static void Remove(int id, EventHandler eventHandler)
        {
            if (!GlobalManager.Runtime) return;
            if (eventDict.ContainsKey(id))
            {
                eventDict[id] -= eventHandler;
            }
        }

        /// <summary>
        /// 调用事件
        /// </summary>
        /// <param name="id">事件唯一标识</param>
        /// <param name="handler">传入事件的参数</param>
        public static void Invoke(int id, params object[] handler)
        {
            if (!GlobalManager.Runtime) return;
            if (eventDict.ContainsKey(id))
            {
                eventDict[id]?.Invoke(handler);
            }
        }
    }
}