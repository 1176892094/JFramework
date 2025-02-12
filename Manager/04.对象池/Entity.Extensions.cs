// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-08 18:01:44
// # Recently: 2025-01-08 18:01:44
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Reflection;
using JFramework.Common;
using UnityEngine;
using UnityEngine.Events;

namespace JFramework
{
    public static partial class Extensions
    {
        public static void Register(this Component current, Type agentType)
        {
            AgentManager.Register(current, agentType);
        }

        public static T Find<T>(this Component current) where T : IAgent
        {
            return (T)AgentManager.Find(current, typeof(T));
        }

        public static T Find<T>(this Component current, Type agentType) where T : IAgent
        {
            return (T)AgentManager.Find(current, agentType);
        }

        public static void UnRegister(this Component current, Type agentType)
        {
            AgentManager.UnRegister(current, agentType);
        }

        public static void Inject(this Component entity)
        {
            var fields = entity.GetType().GetFields(Service.Find.Instance);
            foreach (var field in fields)
            {
                if (field.GetCustomAttribute<InjectAttribute>(true) == null)
                {
                    continue;
                }

                if (!field.FieldType.IsSubclassOf(typeof(Component)))
                {
                    continue;
                }

                if (!typeof(Transform).IsAssignableFrom(field.FieldType))
                {
                    var component = entity.GetComponent(field.FieldType);
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

        private static void SetValue(this Component inject, FieldInfo field, string name)
        {
            var child = inject.transform.GetChild(name);
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

            var method = inject.GetType().GetMethod(name, Service.Find.Instance);
            if (method == null)
            {
                return;
            }

            var injectType = Service.Find.Type("UnityEngine.UI.Button,UnityEngine.UI");
            if (component.TryGetComponent(injectType, out var button))
            {
                var property = injectType.GetProperty("onClick", Service.Find.Instance);
                if (property != null)
                {
                    inject.SetButton(name, (UnityEvent)property.GetValue(button));
                }

                return;
            }

            injectType = Service.Find.Type("UnityEngine.UI.Toggle,UnityEngine.UI");
            if (component.TryGetComponent(injectType, out var toggle))
            {
                var property = injectType.GetProperty("onValueChanged", Service.Find.Instance);
                if (property != null)
                {
                    inject.SetToggle(name, (UnityEvent<bool>)property.GetValue(toggle));
                }
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

        private static void SetButton(this Component inject, string name, UnityEvent button)
        {
            if (!inject.TryGetComponent(out UIPanel panel))
            {
                button.AddListener(() => inject.SendMessage(name));
                return;
            }

            button.AddListener(() =>
            {
                if (panel.state != UIState.Freeze)
                {
                    inject.SendMessage(name);
                }
            });
        }

        private static void SetToggle(this Component inject, string name, UnityEvent<bool> toggle)
        {
            if (!inject.TryGetComponent(out UIPanel panel))
            {
                toggle.AddListener(value => inject.SendMessage(name, value));
                return;
            }

            toggle.AddListener(value =>
            {
                if (panel.state != UIState.Freeze)
                {
                    inject.SendMessage(name, value);
                }
            });
        }
    }
}