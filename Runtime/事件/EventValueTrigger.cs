using System;
using System.Collections.Generic;
using JFramework.Core;
using JFramework.Interface;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JFramework
{
    /// <summary>
    /// 绑定事件变量触发组件
    /// </summary>
    [Serializable]
    public class EventValueTrigger : MonoBehaviour
    {
        /// <summary>
        /// 事件变量列表
        /// </summary>
        [ShowInInspector] private List<IEventValue> valueList;

        /// <summary>
        /// 初始化事件变量列表
        /// </summary>
        private void Awake() => valueList = new List<IEventValue>();

        /// <summary>
        /// 侦听事件变量列表
        /// </summary>
        private void Start() => EventManager.Instance.ListenValue(valueList);

        /// <summary>
        /// 注册单位的事件变量
        /// </summary>
        /// <param name="value">传入单位的事件变量</param>
        public void Register(IEventValue value) => valueList.Add(value);

        /// <summary>
        /// 销毁时移除所有事件变量
        /// </summary>
        private void OnDestroy()
        {
            EventManager.Instance.RemoveValue(valueList);
            foreach (var value in valueList)
            {
                value.Dispose();
            }

            valueList = null;
        }
    }
}