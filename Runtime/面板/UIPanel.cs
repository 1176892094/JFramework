using System;
using System.Collections.Generic;
using JFramework.Core;
using JFramework.Interface;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable All
namespace JFramework
{
    using VisualElement = Dictionary<string, Component>;

    /// <summary>
    /// UI面板的抽象类
    /// </summary>
    public abstract class UIPanel : MonoBehaviour, IPanel
    {
        /// <summary>
        /// 视觉容器字典
        /// </summary>
        [ShowInInspector] private Dictionary<Type, VisualElement> elements = new Dictionary<Type, VisualElement>();

        /// <summary>
        /// UI隐藏类型
        /// </summary>
        public UIStateType stateType = UIStateType.Normal;

        /// <summary>
        /// UI层级
        /// </summary>
        public UILayerType layerType = UILayerType.Normal;

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
        protected void FindComponent<T>() where T : Component
        {
            var components = GetComponentsInChildren<T>();
            foreach (var component in components)
            {
                var key = component.gameObject.name;
                if (!elements.ContainsKey(typeof(T)))
                {
                    var container = new VisualElement();
                    elements.Add(typeof(T), container);
                    container.Add(key, component);
                }
                else if (!elements[typeof(T)].ContainsKey(key))
                {
                    elements[typeof(T)].Add(key, component);
                }

                if (component is Button button)
                {
                    button.onClick.AddListener(() =>
                    {
                        if (stateType == UIStateType.Freeze) return;
                        SendMessage(key);
                    });
                }

                if (component is Toggle toggle)
                {
                    toggle.onValueChanged.AddListener(value =>
                    {
                        if (stateType == UIStateType.Freeze) return;
                        SendMessage(key, value);
                    });
                }
            }
        }

        /// <summary>
        /// 查找存在的组件
        /// </summary>
        /// <param name="key">组件的名称</param>
        /// <typeparam name="T">可以使用任何继承UIBehaviour的组件</typeparam>
        /// <returns>返回查找到的组件</returns>
        public T Get<T>(string key) where T : Component
        {
            return elements.TryGetValue(typeof(T), out var component) ? (T)component[key] : null;
        }

        /// <summary>
        /// 显示UI面板
        /// </summary>
        public virtual void Show() => gameObject.SetActive(true);

        /// <summary>
        /// 隐藏UI面板
        /// </summary>
        public virtual void Hide() => gameObject.SetActive(false);

        /// <summary>
        /// UI 面板状态
        /// </summary>
        UIStateType IPanel.stateType => stateType;

        /// <summary>
        /// UI 面板层级
        /// </summary>
        UILayerType IPanel.layerType => layerType;

        /// <summary>
        /// 实体接口调用实体更新方法
        /// </summary>
        void IEntity.Update() => OnUpdate();
    }
}