using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace JFramework
{
    using UIContainer = Dictionary<string, UIBehaviour>;

    /// <summary>
    /// UI面板的抽象类
    /// </summary>
    public abstract class UIPanel : Entity
    {
        /// <summary>
        /// 视觉容器字典
        /// </summary>
        [ShowInInspector, LabelText("视觉元素")]
        private readonly Dictionary<string, UIContainer> containerDict = new Dictionary<string, UIContainer>();

        /// <summary>
        /// 开始时查找所有控件
        /// </summary>
        protected override void Awake()
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
                if (!containerDict.ContainsKey(type))
                {
                    var container = new UIContainer();
                    containerDict.Add(type, container);
                    container.Add(key, component);
                }
                else if (!containerDict[type].ContainsKey(key))
                {
                    containerDict[type].Add(key, component);
                }

                switch (component)
                {
                    case Button button:
                        button.onClick.AddListener(() => OnButtonClick(key));
                        break;
                }
            }
        }

        /// <summary>
        /// 反射按钮名称
        /// </summary>
        /// <param name="key">点击的按钮名称</param>
        private void OnButtonClick(string key)
        {
            var method = GetType().GetMethod(key, BindingFlags.Instance | BindingFlags.NonPublic);
            method?.Invoke(this, null);
        }

        /// <summary>
        /// 查找存在的组件
        /// </summary>
        /// <param name="key">组件的名称</param>
        /// <typeparam name="T">可以使用任何继承UIBehaviour的组件</typeparam>
        /// <returns>返回查找到的组件</returns>
        protected T Get<T>(string key) where T : UIBehaviour
        {
            var type = typeof(T).Name;
            if (containerDict.ContainsKey(type))
            {
                return (T)containerDict[type][key];
            }

            return null;
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