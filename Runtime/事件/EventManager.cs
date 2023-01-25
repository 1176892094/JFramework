using System.Collections.Generic;
using JFramework.Interface;
using Sirenix.OdinInspector;

namespace JFramework.Core
{
    /// <summary>
    /// 事件管理器
    /// </summary>
    public sealed class EventManager : Singleton<EventManager>
    {
        /// <summary>
        /// 存储所有事件的字典
        /// </summary>
        [ShowInInspector, ReadOnly, LabelText("事件管理数据"), FoldoutGroup("事件管理视图")]
        private Dictionary<string, EventData> eventDict;

        [ShowInInspector, ReadOnly, LabelText("事件变量列表"), FoldoutGroup("事件管理视图")]
        private List<List<IEventValue>> valueList;

        /// <summary>
        /// 事件管理器初始化
        /// </summary>
        protected override void OnInit(params object[] args)
        {
            valueList = new List<List<IEventValue>>();
            eventDict = new Dictionary<string, EventData>();
        }

        /// <summary>
        /// 事件管理侦听事件的方法
        /// </summary>
        /// <param name="name">传入事件的名称</param>
        /// <param name="action">传入事件的方法</param>
        public void Listen(string name, EventData action)
        {
            if (eventDict.ContainsKey(name))
            {
                eventDict[name] += action;
            }
            else
            {
                eventDict.Add(name, action);
            }
        }

        /// <summary>
        /// 事件管理移除事件的方法
        /// </summary>
        /// <param name="name">传入事件的名称</param>
        /// <param name="action">传入事件的方法</param>
        public void Remove(string name, EventData action)
        {
            if (eventDict.ContainsKey(name))
            {
                eventDict[name] -= action;
            }
        }

        /// <summary>
        /// 事件管理发送事件的方法
        /// </summary>
        /// <param name="name">传入事件的名称</param>
        /// <param name="args">传入事件的参数</param>
        public void Send(string name, params object[] args)
        {
            if (eventDict.ContainsKey(name))
            {
                eventDict[name]?.Invoke(args);
            }
        }

        /// <summary>
        /// 事件管理器侦听事件变量组
        /// </summary>
        /// <param name="values">对象所拥有的事件变量组</param>
        internal void ListenValue(List<IEventValue> values)
        {
            valueList.Add(values);
        }

        /// <summary>
        /// 事件管理器移除事件变量组
        /// </summary>
        /// <param name="values">对象所拥有的事件变量组</param>
        internal void RemoveValue(List<IEventValue> values)
        {
            valueList.Remove(values);
        }
    }
}