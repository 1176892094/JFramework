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
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JFramework.Core
{
    public static partial class UIManager
    {
        public static Canvas canvas { get; private set; }
        internal static readonly Dictionary<Type, UIPanel> panels = new();
        private static readonly Dictionary<UILayer, Transform> layers = new();
        private static readonly Dictionary<Type, Task<UIPanel>> requests = new();

        internal static void Register()
        {
            canvas = GlobalManager.Instance.transform.Find("UICanvas").GetComponent<Canvas>();
            layers[UILayer.Bottom] = canvas.transform.Find("Layer1");
            layers[UILayer.Normal] = canvas.transform.Find("Layer2");
            layers[UILayer.Middle] = canvas.transform.Find("Layer3");
            layers[UILayer.Height] = canvas.transform.Find("Layer4");
            layers[UILayer.Ignore] = canvas.transform.Find("Layer5");
        }

        private static async Task<UIPanel> Load(Type type)
        {
            if (requests.TryGetValue(type, out var request))
            {
                return await request;
            }

            request = LoadAsync(type);
            requests.Add(type, request);
            try
            {
                return await request;
            }
            finally
            {
                requests.Remove(type);
            }
        }

        private static async Task<UIPanel> LoadAsync(Type type)
        {
            var obj = await AssetManager.Load<GameObject>(SettingManager.GetUIPath(type.Name));
            var panel = obj.GetComponent<UIPanel>() ?? (UIPanel)obj.AddComponent(type);
            panel.transform.SetParent(layers[panel.layer], false);
            panels.Add(type, panel);
            return panel;
        }

        public static void Add<T>(T panel) where T : UIPanel
        {
            if (!panels.ContainsKey(typeof(T)))
            {
                panels.Add(typeof(T), panel);
            }
        }

        public static void Remove<T>(T panel) where T : UIPanel
        {
            if (panels.Remove(typeof(T)))
            {
                Object.Destroy(panel.gameObject);
            }
        }

        public static Transform Layer(UILayer layer)
        {
            return layers.GetValueOrDefault(layer);
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

    public static partial class UIManager
    {
        public static async void Show<TPanel>() where TPanel : UIPanel
        {
            if (!GlobalManager.Instance) return;
            if (!panels.TryGetValue(typeof(TPanel), out var panel))
            {
                panel = await Load(typeof(TPanel));
            }

            panel.Show();
        }

        public static async void Show<TPanel>(Action action) where TPanel : UIPanel
        {
            if (!GlobalManager.Instance) return;
            if (!panels.TryGetValue(typeof(TPanel), out var panel))
            {
                panel = await Load(typeof(TPanel));
            }

            panel.Show();
            action?.Invoke();
        }

        public static async void Show<TPanel>(Action<TPanel> action) where TPanel : UIPanel
        {
            if (!GlobalManager.Instance) return;
            if (!panels.TryGetValue(typeof(TPanel), out var panel))
            {
                panel = await Load(typeof(TPanel));
            }

            panel.Show();
            action?.Invoke((TPanel)panel);
        }

        public static void Hide<TPanel>() where TPanel : UIPanel
        {
            if (!GlobalManager.Instance) return;
            if (panels.TryGetValue(typeof(TPanel), out var panel))
            {
                if (panel.gameObject.activeInHierarchy)
                {
                    panel.Hide();
                }
            }
        }

        public static TPanel Get<TPanel>() where TPanel : UIPanel
        {
            return (TPanel)panels.GetValueOrDefault(typeof(TPanel));
        }

        public static bool IsActive<TPanel>() where TPanel : UIPanel
        {
            return panels.TryGetValue(typeof(TPanel), out var panel) && panel.gameObject.activeInHierarchy;
        }
    }

    public static partial class UIManager
    {
        public static async void Show(Type type)
        {
            if (!GlobalManager.Instance) return;
            if (!panels.TryGetValue(type, out var panel))
            {
                panel = await Load(type);
            }

            panel.Show();
        }

        public static async void Show(Type type, Action action)
        {
            if (!GlobalManager.Instance) return;
            if (!panels.TryGetValue(type, out var panel))
            {
                panel = await Load(type);
            }

            panel.Show();
            action?.Invoke();
        }

        public static async void Show(Type type, Action<UIPanel> action)
        {
            if (!GlobalManager.Instance) return;
            if (!panels.TryGetValue(type, out var panel))
            {
                panel = await Load(type);
            }

            panel.Show();
            action?.Invoke(panel);
        }

        public static void Hide(Type type)
        {
            if (!GlobalManager.Instance) return;
            if (panels.TryGetValue(type, out var panel))
            {
                if (panel.gameObject.activeInHierarchy)
                {
                    panel.Hide();
                }
            }
        }

        public static UIPanel Get(Type type)
        {
            return panels.GetValueOrDefault(type);
        }

        public static bool IsActive(Type type)
        {
            return panels.TryGetValue(type, out var panel) && panel.gameObject.activeInHierarchy;
        }
    }
}