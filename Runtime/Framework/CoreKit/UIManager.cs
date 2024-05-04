// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-02-04  18:20
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using JFramework.Interface;
using UnityEngine;
using Object = UnityEngine.Object;

// ReSharper disable All

namespace JFramework.Core
{
    public static class UIManager
    {
        public static Canvas canvas;
        private static readonly Dictionary<UILayer, Transform> layers = new();
        internal static readonly Dictionary<Type, IPanel> panels = new();

        internal static void Register()
        {
            canvas = GlobalManager.Instance.transform.Find("UICanvas").GetComponent<Canvas>();
            layers[UILayer.Bottom] = canvas.transform.Find("Layer1");
            layers[UILayer.Normal] = canvas.transform.Find("Layer2");
            layers[UILayer.Middle] = canvas.transform.Find("Layer3");
            layers[UILayer.Height] = canvas.transform.Find("Layer4");
            layers[UILayer.Ignore] = canvas.transform.Find("Layer5");
        }

        public static void Show<TPanel>() where TPanel : UIPanel
        {
            if (!GlobalManager.Instance) return;
            if (panels.TryGetValue(typeof(TPanel), out var panel))
            {
                panel.Show();
            }
            else
            {
                Load<TPanel>();
            }
        }

        public static void Show<TPanel>(Action action) where TPanel : UIPanel
        {
            if (!GlobalManager.Instance) return;
            if (panels.TryGetValue(typeof(TPanel), out var panel))
            {
                panel.Show();
                action?.Invoke();
            }
            else
            {
                Load<TPanel>(panel => action());
            }
        }

        public static void Show<TPanel>(Action<TPanel> action) where TPanel : UIPanel
        {
            if (!GlobalManager.Instance) return;
            if (panels.TryGetValue(typeof(TPanel), out var panel))
            {
                panel.Show();
                action?.Invoke((TPanel)panel);
            }
            else
            {
                Load<TPanel>(action);
            }
        }

        private static async void Load<TPanel>(Action<TPanel> action = null) where TPanel : UIPanel
        {
            if (panels.ContainsKey(typeof(TPanel)))
            {
                Debug.LogWarning($"加载  {typeof(TPanel).Name.Red()} 失败，面板已经加载!");
                return;
            }

            var obj = await AssetManager.Load<GameObject>(SettingManager.GetUIPath(typeof(TPanel).Name));
            if (!obj.TryGetComponent<TPanel>(out var panel))
            {
                panel = obj.AddComponent<TPanel>();
            }

            panel.transform.SetParent(layers[panel.layer], false);
            panels.Add(typeof(TPanel), panel);
            panel.Show();
            action?.Invoke(panel);
        }

        public static void Hide<TPanel>() where TPanel : IPanel
        {
            if (!GlobalManager.Instance) return;
            if (panels.TryGetValue(typeof(TPanel), out var panel))
            {
                if (IsActive<TPanel>())
                {
                    panel.Hide();
                }
            }
        }

        public static TPanel Get<TPanel>() where TPanel : IPanel => (TPanel)panels.GetValueOrDefault(typeof(TPanel));

        public static IPanel Get(Type key) => panels.GetValueOrDefault(key);

        public static Transform Get(UILayer type) => layers.GetValueOrDefault(type);

        public static bool IsActive<TPanel>() where TPanel : IPanel
        {
            return panels.TryGetValue(typeof(TPanel), out var panel) && panel.gameObject.activeInHierarchy;
        }

        public static void Register<T>(T panel) where T : IPanel
        {
            if (!panels.ContainsKey(typeof(T)))
            {
                panels.Add(typeof(T), panel);
            }
        }

        public static void UnRegister<T>(T panel) where T : IPanel
        {
            if (panels.Remove(typeof(T)))
            {
                Object.Destroy(panel.gameObject);
            }
        }

        public static void Clear()
        {
            var copies = panels.Keys.ToList();
            foreach (var type in copies)
            {
                if (panels.TryGetValue(type, out var panel))
                {
                    if (panel.state != UIState.DontDestroy)
                    {
                        Object.Destroy(panel.gameObject);
                        panels.Remove(type);
                    }
                }
            }
        }

        internal static void UnRegister()
        {
            canvas = null;
            panels.Clear();
            layers.Clear();
        }
    }
}