// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-4-4  18:2
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Reflection;
using JFramework.Core;
using JFramework.Interface;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using EventEntry = UnityEngine.EventSystems.EventTrigger.Entry;

namespace JFramework
{
    public static partial class Extensions
    {
        public static void Inject(this IEntity inject)
        {
            var fields = inject.GetType().GetFields(Reflection.Instance);
            foreach (var field in fields)
            {
                var attribute = field.GetCustomAttribute<InjectAttribute>(true);
                if (attribute == null) continue;

                if (typeof(IComponent).IsAssignableFrom(field.FieldType))
                {
                    var component = EntityManager.GetComponent(inject, field.FieldType);
                    field.SetValue(inject, component);
                }
                else if (field.FieldType.IsSubclassOf(typeof(Component)))
                {
                    if (!typeof(Transform).IsAssignableFrom(field.FieldType))
                    {
                        var component = inject.transform.GetComponent(field.FieldType);
                        if (component != null)
                        {
                            field.SetValue(inject, component);
                            continue;
                        }
                    }

                    var name = char.ToUpper(field.Name[0]) + field.Name.Substring(1);
                    inject.SetValue(field, name);
                }
            }
        }

        private static void SetValue(this IEntity inject, FieldInfo field, string name)
        {
            var child = inject.transform.GetChild(name);
            if (child == null) return;

            var component = child.GetComponent(field.FieldType);
            if (component == null) return;

            field.SetValue(inject, component);

            switch (component)
            {
                case Button button:
                    inject.SetButton(name, button);
                    break;
                case Toggle toggle:
                    inject.SetToggle(name, toggle);
                    break;
            }
        }

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

        private static void SetButton(this IEntity inject, string name, Button button)
        {
            if (inject.transform.TryGetComponent(out UIPanel panel))
            {
                button.onClick.AddListener(() =>
                {
                    if (panel.state != UIState.Freeze)
                    {
                        inject.transform.SendMessage(name);
                    }
                });
            }
            else
            {
                button.onClick.AddListener(() => inject.transform.SendMessage(name));
            }
        }

        private static void SetToggle(this IEntity inject, string name, Toggle toggle)
        {
            if (inject.transform.TryGetComponent(out UIPanel panel))
            {
                toggle.onValueChanged.AddListener(value =>
                {
                    if (panel.state != UIState.Freeze)
                    {
                        inject.transform.SendMessage(name, value);
                    }
                });
            }
            else
            {
                toggle.onValueChanged.AddListener(value => inject.transform.SendMessage(name, value));
            }
        }

        public static void AddListener(this UIBehaviour target, EventTriggerType eventID, Action<PointerEventData> action)
        {
            var trigger = target.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = target.gameObject.AddComponent<EventTrigger>();
            }

            var entry = new EventEntry { eventID = eventID };
            entry.callback.AddListener(data => action?.Invoke((PointerEventData)data));
            trigger.triggers.Add(entry);
        }
    }
}