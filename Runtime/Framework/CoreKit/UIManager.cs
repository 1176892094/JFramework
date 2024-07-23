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
        private static readonly Dictionary<Type, UIPanel> panels = new();
        private static readonly Dictionary<Type, List<UIPanel>> groups = new();
        private static readonly Dictionary<Type, Task<UIPanel>> requests = new();
        private static readonly Dictionary<UILayer, Transform> layers = new();

        internal static void Register()
        {
            canvas = GlobalManager.Instance.GetComponentInChildren<Canvas>();
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

        public static void AddPanel<T>(T panel) where T : UIPanel
        {
            if (!panels.ContainsKey(typeof(T)))
            {
                panels.Add(typeof(T), panel);
            }
        }

        public static void RemovePanel<T>(T panel) where T : UIPanel
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
            groups.Clear();
        }
    }

    public static partial class UIManager
    {
        public static void AddGroup<TGroup>(UIPanel panel)
        {
            if (!groups.TryGetValue(typeof(TGroup), out var group))
            {
                group = new List<UIPanel>();
                groups.Add(typeof(TGroup), group);
            }

            if (!group.Contains(panel))
            {
                panel.group = typeof(TGroup);
                group.Add(panel);
            }
        }

        public static void RemoveGroup<TGroup>(UIPanel panel)
        {
            if (!groups.TryGetValue(typeof(TGroup), out var group))
            {
                return;
            }

            if (group.Contains(panel))
            {
                panel.group = null;
                group.Remove(panel);
            }
        }
        
        public static void ShowGroup<TGroup>()
        {
            if (groups.TryGetValue(typeof(TGroup), out var group))
            {
                foreach (var target in group)
                {
                    if (!target.gameObject.activeInHierarchy)
                    {
                        target.Show();
                    }
                }
            }
        }

        public static void HideGroup<TGroup>()
        {
            if (groups.TryGetValue(typeof(TGroup), out var group))
            {
                foreach (var target in group)
                {
                    if (target.gameObject.activeInHierarchy)
                    {
                        target.Hide();
                    }
                }
            }
        }

        private static void ShowInGroup(UIPanel panel)
        {
            if (panel.group != null)
            {
                if (groups.TryGetValue(panel.group, out var group))
                {
                    foreach (var target in group.Where(target => target != panel))
                    {
                        if (target.gameObject.activeInHierarchy)
                        {
                            target.Hide();
                        }
                    }
                }
            }

            panel.Show();
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

            ShowInGroup(panel);
        }

        public static async void Show<TPanel>(Action action) where TPanel : UIPanel
        {
            if (!GlobalManager.Instance) return;
            if (!panels.TryGetValue(typeof(TPanel), out var panel))
            {
                panel = await Load(typeof(TPanel));
            }

            ShowInGroup(panel);
            action?.Invoke();
        }

        public static async void Show<TPanel>(Action<TPanel> action) where TPanel : UIPanel
        {
            if (!GlobalManager.Instance) return;
            if (!panels.TryGetValue(typeof(TPanel), out var panel))
            {
                panel = await Load(typeof(TPanel));
            }

            ShowInGroup(panel);
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
}