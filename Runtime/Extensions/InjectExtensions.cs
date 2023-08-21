using System.Reflection;
using JFramework.Interface;
using UnityEngine;
using UnityEngine.UI;

namespace JFramework
{
    /// <summary>
    /// 注入接口的拓展
    /// </summary>
    public static class InjectExtensions
    {
        /// <summary>
        /// 对自身进行依赖注入
        /// </summary>
        /// <param name="inject"></param>
        public static void Inject(this IInject inject)
        {
            var type = inject.GetType();
            var fields = type.GetFields(Reflection.Instance);
            foreach (var field in fields)
            {
                var attribute = field.GetCustomAttribute<InjectAttribute>(true);
                if (attribute == null) continue;

                var target = inject.transform.GetChild(attribute.name);
                if (target == null) continue;

                var component = target.GetComponent(field.FieldType);
                if (component != null)
                {
                    field.SetValue(inject, component);
                }

                var method = type.GetMethod(attribute.name, Reflection.Instance);
                if (method == null) continue;

                if (target.TryGetComponent(out Button button) && component == button)
                {
                    inject.SetButton(attribute.name, button);
                }
                else if (target.TryGetComponent(out Toggle toggle) && component == toggle)
                {
                    inject.SetToggle(attribute.name, toggle);
                }
            }
        }

        /// <summary>
        /// 注册按钮
        /// </summary>
        /// <param name="inject"></param>
        /// <param name="name"></param>
        /// <param name="button"></param>
        private static void SetButton(this IInject inject, string name, Button button)
        {
            if (inject.transform.TryGetComponent(out IPanel panel))
            {
                button.onClick.AddListener(() =>
                {
                    if (panel.state == UIState.Freeze) return;
                    inject.transform.SendMessage(name);
                });
            }
            else
            {
                button.onClick.AddListener(() =>
                {
                    inject.transform.SendMessage(name);
                });
            }
        }

        /// <summary>
        /// 注册开关
        /// </summary>
        /// <param name="inject"></param>
        /// <param name="name"></param>
        /// <param name="toggle"></param>
        private static void SetToggle(this IInject inject, string name, Toggle toggle)
        {
            if (inject.transform.TryGetComponent(out IPanel panel))
            {
                toggle.onValueChanged.AddListener(value =>
                {
                    if (panel.state == UIState.Freeze) return;
                    inject.transform.SendMessage(name, value);
                });
            }
            else
            {
                toggle.onValueChanged.AddListener(value =>
                {
                    inject.transform.SendMessage(name, value);
                });
            }
        }

        /// <summary>
        /// 迭代查找子物体
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private static Transform GetChild(this Transform parent, string name)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                var child = parent.GetChild(i);
                if (child.name == name)
                {
                    return child;
                }

                var result = child.GetChild(name);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }
    }
}