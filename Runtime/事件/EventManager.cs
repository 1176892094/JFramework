using System.Collections.Generic;

namespace JFramework
{
    /// <summary>
    /// 事件管理器
    /// </summary>
    public sealed class EventManager : Singleton<EventManager>
    {
        /// <summary>
        /// 事件存储字典
        /// </summary>
        internal Dictionary<int, EventData> eventDict;

        /// <summary>
        /// 事件管理器醒来
        /// </summary>
        public override void Awake()
        {
            base.Awake();
            eventDict = new Dictionary<int, EventData>();
        }

        /// <summary>
        /// 侦听事件
        /// </summary>
        /// <param name="id">事件唯一标识</param>
        /// <param name="action">传入侦听的事件</param>
        public void Listen(int id, EventData action)
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
        }

        /// <summary>
        /// 移除事件
        /// </summary>
        /// <param name="id">事件唯一标识</param>
        /// <param name="action">传入移除的事件</param>
        public void Remove(int id, EventData action)
        {
            if (eventDict == null) return;

            if (eventDict.ContainsKey(id))
            {
                eventDict[id] -= action;
            }
        }

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="id">事件唯一标识</param>
        /// <param name="args">传入事件的参数</param>
        public void Send(int id, params object[] args)
        {
            if (eventDict.ContainsKey(id))
            {
                eventDict[id]?.Invoke(args);
            }
        }

        /// <summary>
        /// 清空事件管理器
        /// </summary>
        public override void Destroy()
        {
            base.Destroy();
            eventDict = null;
        }
    }
}