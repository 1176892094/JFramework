using System;
using System.Collections.Generic;
using JFramework.Interface;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable All
namespace JFramework
{
    using Components = Dictionary<string, Component>;

    /// <summary>
    /// UI面板的抽象类
    /// </summary>
    public abstract class UIPanel : MonoBehaviour, IPanel
    {
        /// <summary>
        /// 忽略名称
        /// </summary>
        private static readonly HashSet<string> filters = new HashSet<string>()
        {
            nameof(Text), nameof(Image), nameof(Button), nameof(Toggle),
            nameof(Slider), nameof(RawImage), nameof(InputField),
            "Panel", "Text (TMP)", "InputField (TMP)",
        };

        /// <summary>
        /// 视觉容器字典
        /// </summary>
        [ShowInInspector] private Dictionary<Type, Components> elements = new Dictionary<Type, Components>();

        /// <summary>
        /// UI层级
        /// </summary>
        public UILayer layer { get; protected set; } = UILayer.Normal;

        /// <summary>
        /// UI隐藏类型
        /// </summary>
        public UIState state { get; protected set; } = UIState.Default;

        /// <summary>
        /// 开始时查找所有控件
        /// </summary>
        protected virtual void Awake()
        {
            FindComponent<Text>();
            FindComponent<Image>();
            FindComponent<Button>();
            FindComponent<Toggle>();
            FindComponent<Slider>();
            FindComponent<RawImage>();
            FindComponent<InputField>();
        }

        /// <summary>
        /// 实体启用
        /// </summary>
        protected virtual void OnEnable() => GetComponent<IUpdate>()?.Listen();

        /// <summary>
        /// 实体禁用
        /// </summary>
        protected virtual void OnDisable() => GetComponent<IUpdate>()?.Remove();

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
                if (filters.Contains(key)) continue;
                if (!elements.ContainsKey(typeof(T)))
                {
                    var element = new Components();
                    elements.Add(typeof(T), element);
                    element.Add(key, component);
                }
                else if (!elements[typeof(T)].ContainsKey(key))
                {
                    elements[typeof(T)].Add(key, component);
                }

                if (component is Button button)
                {
                    var method = Reflection.GetMethod(GetType(), key);
                    if (method != null)
                    {
                        button.onClick.AddListener(() =>
                        {
                            if (state == UIState.Freeze) return;
                            method.Invoke(this, null);
                        });
                    }
                }
                else if (component is Toggle toggle)
                {
                    var method = Reflection.GetMethod(GetType(), key);
                    if (method != null)
                    {
                        toggle.onValueChanged.AddListener(value =>
                        {
                            if (state == UIState.Freeze) return;
                            method.Invoke(this, new object[] { value });
                        });
                    }
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
    }
}