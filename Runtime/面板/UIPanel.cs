using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// ReSharper disable All
namespace JFramework
{
    using Component = Dictionary<string, UIBehaviour>;

    /// <summary>
    /// UI面板的抽象类
    /// </summary>
    public abstract class UIPanel : Entity
    {
        /// <summary>
        /// 视觉容器字典
        /// </summary>
        [ShowInInspector, LabelText("视觉元素")]
        private Dictionary<string, Component> componentDict = new Dictionary<string, Component>();

        /// <summary>
        /// UI隐藏类型
        /// </summary>
        [LabelText("面板状态")] public UIStateType stateType;

        /// <summary>
        /// 开始时查找所有控件
        /// </summary>
        protected virtual void Awake()
        {
            FindComponent<Image>();
            FindComponent<Slider>();
            FindComponent<Button>();
            FindComponent<Toggle>();
            FindComponent<RawImage>();
            FindComponent<TMP_Text>();
            FindComponent<TMP_InputField>();
        }

        /// <summary>
        /// 查找所有组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private void FindComponent<T>() where T : UIBehaviour
        {
            var type = typeof(T).Name;
            var components = GetComponentsInChildren<T>();
            foreach (var component in components)
            {
                var key = component.gameObject.name;
                if (!componentDict.ContainsKey(type))
                {
                    var container = new Component();
                    componentDict.Add(type, container);
                    container.Add(key, component);
                }
                else if (!componentDict[type].ContainsKey(key))
                {
                    componentDict[type].Add(key, component);
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
            var type = typeof(T).Name;
            return componentDict.ContainsKey(type) ? (T)componentDict[type][key] : null;
        }

        /// <summary>
        /// 显示UI面板
        /// </summary>
        public virtual void Show()
        {
        }

        /// <summary>
        /// 隐藏UI面板
        /// </summary>
        public virtual void Hide()
        {
        }
    }
}