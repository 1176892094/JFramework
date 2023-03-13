using System.Collections.Generic;
using JFramework.Core;
using UnityEngine;

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
        internal override void Awake()
        {
            base.Awake();
            eventDict = new Dictionary<int, EventData>();
        }

        /// <summary>
        /// 侦听事件
        /// </summary>
        /// <param name="id">事件唯一标识</param>
        /// <param name="action">传入侦听的事件</param>
        public void AddListener(int id, EventData action)
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
            
            if (DebugManager.Instance.isShowEvent)
            {
                Debug.Log($"EventManager侦听 {id.ToString().Yellow()} 事件！");
            }
        }

        /// <summary>
        /// 移除事件
        /// </summary>
        /// <param name="id">事件唯一标识</param>
        /// <param name="action">传入移除的事件</param>
        public void RemoveListener(int id, EventData action)
        {
            if (eventDict == null) return;

            if (eventDict.ContainsKey(id))
            {
                eventDict[id] -= action;
            }
            
            if (DebugManager.Instance.isShowEvent)
            {
                Debug.Log($"EventManager移除 {id.ToString().Yellow()} 事件！");
            }
        }

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="id">事件唯一标识</param>
        /// <param name="args">传入事件的参数</param>
        public void SendMessage(int id, params object[] args)
        {
            if (eventDict.ContainsKey(id))
            {
                eventDict[id]?.Invoke(args);
            }
            
            if (DebugManager.Instance.isShowEvent)
            {
                Debug.Log($"EventManager广播 {id.ToString().Yellow()} 事件！");
            }
        }

        /// <summary>
        /// 清空事件管理器
        /// </summary>
        internal override void Destroy()
        {
            base.Destroy();
            eventDict = null;
        }
    }
}