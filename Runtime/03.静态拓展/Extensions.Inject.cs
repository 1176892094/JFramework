// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-18 21:12:29
// # Recently: 2024-12-22 20:12:32
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace JFramework
{
    public static partial class Extensions
    {
        public static void Inject(this IEntity entity)
        {
            var fields = entity.GetType().GetFields(Service.Depend.Instance);
            foreach (var field in fields)
            {
                if (field.GetCustomAttribute<InjectAttribute>(true) == null)
                {
                    continue;
                }

                if (typeof(IAgent).IsAssignableFrom(field.FieldType))
                {
                    var component = entity.Agent(field.FieldType);
                    field.SetValue(entity, component);
                    continue;
                }
                
                if (!field.FieldType.IsSubclassOf(typeof(Component)))
                {
                    continue;
                }

                if (!typeof(Transform).IsAssignableFrom(field.FieldType))
                {
                    var transform = entity.GetComponent<Transform>();
                    var component = transform.GetComponent(field.FieldType);
                    if (component != null)
                    {
                        field.SetValue(entity, component);
                        continue;
                    }
                }

                var name = char.ToUpper(field.Name[0]) + field.Name.Substring(1);
                entity.SetValue(field, name);
            }
        }

        private static void SetValue(this IEntity inject, FieldInfo field, string name)
        {
            var transform = inject.GetComponent<Transform>();
            var child = transform.GetChild(name);
            if (child == null)
            {
                return;
            }

            var component = child.GetComponent(field.FieldType);
            if (component == null)
            {
                Debug.Log(Service.Text.Format("没有找到依赖注入的组件: {0} {1} != {2}", field.FieldType, field.FieldType.Name, name));
                return;
            }

            field.SetValue(inject, component);

            var method = inject.GetType().GetMethod(name, Service.Depend.Instance);
            if (method == null)
            {
                return;
            }

            if (component.TryGetComponent(out Button button))
            {
                transform.SetButton(name, button);
                return;
            }

            if (component.TryGetComponent(out Toggle toggle))
            {
                transform.SetToggle(name, toggle);
            }
        }

        private static Transform GetChild(this Transform parent, string name)
        {
            for (var i = 0; i < parent.childCount; i++)
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

        private static void SetButton(this Transform inject, string name, Button button)
        {
            if (!inject.TryGetComponent(out IPanel panel))
            {
                button.onClick.AddListener(() => inject.SendMessage(name));
                return;
            }

            button.onClick.AddListener(() =>
            {
                if (panel.state != UIState.Freeze)
                {
                    inject.SendMessage(name);
                }
            });
        }

        private static void SetToggle(this Transform inject, string name, Toggle toggle)
        {
            if (!inject.TryGetComponent(out IPanel panel))
            {
                toggle.onValueChanged.AddListener(value => inject.SendMessage(name, value));
                return;
            }

            toggle.onValueChanged.AddListener(value =>
            {
                if (panel.state != UIState.Freeze)
                {
                    inject.SendMessage(name, value);
                }
            });
        }
    }
}