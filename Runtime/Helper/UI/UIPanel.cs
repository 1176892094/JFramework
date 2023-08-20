using System.Reflection;
using JFramework.Interface;
using UnityEngine;
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
            InitField();
            InitMethod();
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
        /// 初始化字段
        /// </summary>
        private void InitField()
        {
            var fields = GetType().GetFields(Reflection.Instance);

            foreach (var field in fields)
            {
                var attribute = field.GetCustomAttribute<UIFieldAttribute>(true);
                if (attribute != null)
                {
                    var child = FindChild(transform, attribute.name);
                    if (child != null)
                    {
                        var component = child.GetComponent(field.FieldType);
                        if (component != null)
                        {
                            field.SetValue(this, component);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 初始化方法
        /// </summary>
        private void InitMethod()
        {
            var methods = GetType().GetMethods(Reflection.Instance);

            foreach (var method in methods)
            {
                var attribute = method.GetCustomAttribute<UIMethodAttribute>(false);
                if (attribute != null)
                {
                    var child = FindChild(transform, attribute.name);
                    if (child != null)
                    {
                        if (child.TryGetComponent(out Button button))
                        {
                            button.onClick.AddListener(() =>
                            {
                                if (state == UIState.Freeze) return;
                                method.Invoke(this, null);
                            });
                        }
                        else if (child.TryGetComponent(out Toggle toggle))
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
        }

        /// <summary>
        /// 递归查找子物体
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Transform FindChild(Transform parent, string name)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                Transform child = parent.GetChild(i);
                if (child.name == name)
                {
                    return child;
                }

                Transform result = FindChild(child, name);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
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