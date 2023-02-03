using System;
using JFramework.Interface;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JFramework
{
    /// <summary>
    /// 事件变量
    /// </summary>
    /// <typeparam name="T">可以指定任何对象和值</typeparam>
    [Serializable]
    public class EventValue<T> : IEventValue
    {
        /// <summary>
        /// 当值改变时触发事件
        /// </summary>
        [ShowInInspector] private Action<T> OnValueChanged;

        /// <summary>
        /// 私有的绑定值
        /// </summary>
        [ShowInInspector] private T value;

        /// <summary>
        /// 公开的绑定值
        /// </summary>
        public T Value
        {
            get => value;
            set
            {
                if (value == null && this.value == null) return;
                if (value != null && value.Equals(this.value)) return;
                this.value = value;
                OnValueChanged?.Invoke(value);
            }
        }

        /// <summary>
        /// 构造函数初始化事件变量
        /// </summary>
        /// <param name="value"></param>
        public EventValue(T value = default)
        {
            this.value = value;
            OnValueChanged = null;
        }

        /// <summary>
        /// 事件变量侦听值改变事件
        /// </summary>
        /// <param name="onValueChanged">值改变时调用的方法</param>
        /// <returns>返回自身</returns>
        public IEventValue Listen(Action<T> onValueChanged)
        {
            OnValueChanged += onValueChanged;
            return this;
        }

        /// <summary>
        /// 事件变量移除值改变事件
        /// </summary>
        /// <param name="onValueChanged">值改变时调用的方法</param>
        public void Remove(Action<T> onValueChanged)
        {
            OnValueChanged -= onValueChanged;
        }

        /// <summary>
        /// 设置事件变量绑定的游戏对象
        /// </summary>
        /// <param name="target">传入绑定的游戏对象，事件会随着对象销毁而释放</param>
        public void SetTarget(GameObject target)
        {
            if (!target.TryGetComponent<EventValueTrigger>(out var trigger))
            {
                trigger = target.AddComponent<EventValueTrigger>();
            }

            trigger.Register(this);
        }

        /// <summary>
        /// 释放事件变量
        /// </summary>
        public void Dispose()
        {
            Value = default;
            OnValueChanged = null;
        }

        /// <summary>
        /// 将变量值转化为字符串
        /// </summary>
        /// <returns>返回转化为字符串的变量值</returns>
        public override string ToString()
        {
            return Value.ToString();
        }

        /// <summary>
        /// 可以对该变量进行自定义隐式转换
        /// </summary>
        /// <param name="property">传入的属性</param>
        /// <returns>返回变量值</returns>
        public static implicit operator T(EventValue<T> property)
        {
            return property.Value;
        }
    }
}