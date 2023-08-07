using System;
using System.Collections.Generic;
using System.Reflection;
using JFramework.Core;
using JFramework.Interface;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// ReSharper disable All
namespace JFramework
{
    /// <summary>
    /// UI面板的抽象类
    /// </summary>
    public abstract class UIPanel : MonoBehaviour, IPanel
    {
        /// <summary>
        /// 视觉容器字典
        /// </summary>
        [ShowInInspector, LabelText("视觉元素")] private Dictionary<Type, Dictionary<string, UIBehaviour>> components = new Dictionary<Type, Dictionary<string, UIBehaviour>>();

        /// <summary>
        /// UI隐藏类型
        /// </summary>
        [ShowInInspector, LabelText("面板状态")] public UIStateType stateType;

        /// <summary>
        /// 面板是否活跃
        /// </summary>
        private bool isActive;

        /// <summary>
        /// 开始时查找所有控件
        /// </summary>
        protected virtual void Awake()
        {
            FindComponent<Text>();
            FindComponent<Image>();
            FindComponent<Slider>();
            FindComponent<Button>();
            FindComponent<Toggle>();
            FindComponent<RawImage>();
            FindComponent<InputField>();
        }

        /// <summary>
        /// 实体更新
        /// </summary>`
        protected virtual void OnUpdate()
        {
        }

        /// <summary>
        /// 实体启用
        /// </summary>
        protected virtual void OnEnable() => GlobalManager.Listen(this);

        /// <summary>
        /// 实体禁用
        /// </summary>
        protected virtual void OnDisable() => GlobalManager.Remove(this);

        /// <summary>
        /// 查找所有组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private void FindComponent<T>() where T : UIBehaviour
        {
            var components = GetComponentsInChildren<T>();
            foreach (var component in components)
            {
                var key = component.gameObject.name;
                if (!this.components.ContainsKey(typeof(T)))
                {
                    var container = new Dictionary<string, UIBehaviour>();
                    this.components.Add(typeof(T), container);
                    container.Add(key, component);
                }
                else if (!this.components[typeof(T)].ContainsKey(key))
                {
                    this.components[typeof(T)].Add(key, component);
                }

                if (component is Button button)
                {
                    button.onClick.AddListener(() => OnButtonClick(key));
                }
            }
        }

        /// <summary>
        /// 反射按钮名称
        /// </summary>
        /// <param name="key">点击的按钮名称</param>
        private void OnButtonClick(string key)
        {
            if (stateType == UIStateType.Freeze) return;
            var method = GetType().GetMethod(key, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            method?.Invoke(this, null);
        }

        /// <summary>
        /// 查找存在的组件
        /// </summary>
        /// <param name="key">组件的名称</param>
        /// <typeparam name="T">可以使用任何继承UIBehaviour的组件</typeparam>
        /// <returns>返回查找到的组件</returns>
        public T Get<T>(string key) where T : UIBehaviour
        {
            return components.TryGetValue(typeof(T), out var component) ? (T)component[key] : null;
        }

        /// <summary>
        /// 显示UI面板
        /// </summary>
        public virtual void Show() => gameObject.SetActive(isActive = true);

        /// <summary>
        /// 隐藏UI面板
        /// </summary>
        public virtual void Hide() => gameObject.SetActive(isActive = false);

        bool IPanel.isActive => isActive;

        /// <summary>
        /// UI 面板状态
        /// </summary>
        UIStateType IPanel.stateType => stateType;

        /// <summary>
        /// 实体接口调用实体更新方法
        /// </summary>
        void IEntity.Update() => OnUpdate();
    }
}