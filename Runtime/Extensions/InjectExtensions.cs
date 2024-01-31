// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-12-21  21:03
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.Reflection;
using JFramework.Interface;
using UnityEngine;
using UnityEngine.UI;

namespace JFramework
{
    /// <summary>
    /// 注入接口的拓展
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 对自身进行依赖注入
        /// </summary>
        /// <param name="inject">对注入接口的拓展</param>
        public static void Inject(this IEntity inject)
        {
            var type = inject.GetType();
            var fields = type.GetFields(Reflection.Instance | BindingFlags.Static);
            foreach (var field in fields)
            {
                var attribute = field.GetCustomAttribute<InjectAttribute>(true);
                if (attribute == null) continue;

                if (typeof(IController).IsAssignableFrom(field.FieldType))
                {
                    var obj = GlobalManager.Entity.Register(inject.gameObject, field.FieldType);
                    field.SetValue(inject, obj);
                }
                else if (typeof(Component).IsAssignableFrom(field.FieldType))
                {
                    var fieldName = attribute.name;
                    if (string.IsNullOrEmpty(fieldName))
                    {
                        var component = inject.transform.GetComponent(field.FieldType);
                        if (component != null)
                        {
                            field.SetValue(inject, component);
                        }
                    }
                    else
                    {
                        var target = inject.transform.GetChild(fieldName);
                        if (target != null)
                        {
                            var component = target.GetComponent(field.FieldType);
                            if (component != null)
                            {
                                field.SetValue(inject, component);
                            }

                            var method = type.GetMethod(fieldName, Reflection.Instance);
                            if (method != null)
                            {
                                if (component is Button button)
                                {
                                    inject.SetButton(fieldName, button);
                                }
                                else if (component is Toggle toggle)
                                {
                                    inject.SetToggle(fieldName, toggle);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 注册按钮
        /// </summary>
        /// <param name="inject"></param>
        /// <param name="name"></param>
        /// <param name="button"></param>
        private static void SetButton(this IEntity inject, string name, Button button)
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
                button.onClick.AddListener(() => inject.transform.SendMessage(name));
            }
        }

        /// <summary>
        /// 注册开关
        /// </summary>
        /// <param name="inject"></param>
        /// <param name="name"></param>
        /// <param name="toggle"></param>
        private static void SetToggle(this IEntity inject, string name, Toggle toggle)
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
                toggle.onValueChanged.AddListener(value => inject.transform.SendMessage(name, value));
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