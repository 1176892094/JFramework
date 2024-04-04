// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: jinyijie
// # Version: 1.0.0
// # History: 2024-4-4  18:2
// # Copyright: 2024, jinyijie
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.Reflection;
using JFramework.Interface;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using EventEntry = UnityEngine.EventSystems.EventTrigger.Entry;

namespace JFramework
{
    public static partial class Extensions
    {
        public static T FindComponent<T>(this IEntity entity) where T : ScriptableObject, IComponent
        {
            return (T)GlobalManager.Entity.FindComponent(entity, typeof(T));
        }

        public static IComponent FindComponent(this IEntity entity, Type type)
        {
            return GlobalManager.Entity.FindComponent(entity, type);
        }

        public static void Destroy(this IEntity entity)
        {
            if (!GlobalManager.Instance) return;
            GlobalManager.Entity.Destroy(entity);
        }

        public static void Refresh<TGird, TItem>(this TGird[] grids, List<TItem> items) where TGird : IGrid<TItem>
        {
            for (int i = 0; i < grids.Length; i++)
            {
                if (grids[i] != null && i < items.Count && items[i] != null)
                {
                    grids[i].SetItem(items[i]);
                }
                else
                {
                    grids[i].Dispose();
                }
            }
        }

        public static void AddListener(this UIBehaviour target, EventTriggerType type, Action<PointerEventData> action)
        {
            var trigger = target.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = target.gameObject.AddComponent<EventTrigger>();
            }

            var entry = new EventEntry
            {
                eventID = type
            };
            entry.callback.AddListener(eventData => action?.Invoke((PointerEventData)eventData));
            trigger.triggers.Add(entry);
        }
    }

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
                    var component = GlobalManager.Entity.FindComponent(inject, field.FieldType);
                    field.SetValue(inject, component);
                }
                else if (typeof(Component).IsAssignableFrom(field.FieldType))
                {
                    var name = attribute.name;
                    if (string.IsNullOrEmpty(name))
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

                        name = char.ToUpper(field.Name[0]) + field.Name.Substring(1);
                    }

                    inject.SetComponent(field, name);
                }
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

        private static void SetComponent(this IEntity inject, FieldInfo field, string name)
        {
            var child = inject.transform.GetChild(name);
            if (child == null) return;

            var component = child.GetComponent(field.FieldType);
            if (component == null) return;

            field.SetValue(inject, component);

            var method = inject.GetType().GetMethod(name, Reflection.Instance);
            if (method == null) return;

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
    }
}