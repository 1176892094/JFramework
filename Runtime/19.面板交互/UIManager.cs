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
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace JFramework
{
    public static partial class UIManager
    {
        public static Canvas canvas { get; private set; }
        private static readonly Dictionary<string, UIPanel> panels = new();
        private static readonly Dictionary<string, List<UIPanel>> groups = new();
        private static readonly Dictionary<UILayer, Transform> layers = new();

        internal static void Register()
        {
            var manager = new GameObject("UIManager");
            canvas = manager.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            Object.DontDestroyOnLoad(manager);

            var scale = manager.AddComponent<CanvasScaler>();
            scale.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scale.referenceResolution = new Vector2(1920, 1080);
            scale.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scale.matchWidthOrHeight = 0.5f;

            for (var i = UILayer.Bottom; i <= UILayer.Ignore; i++)
            {
                var layer = new GameObject("Layer-" + (int)i);
                layer.transform.SetParent(canvas.transform);
                var child = layer.AddComponent<Canvas>();
                child.overrideSorting = true;
                child.sortingOrder = (int)i;
                if (i != UILayer.Ignore)
                {
                    layer.AddComponent<GraphicRaycaster>();
                }

                var transform = layer.GetComponent<RectTransform>();
                transform.anchorMin = Vector2.zero;
                transform.anchorMax = Vector2.one;
                transform.offsetMin = Vector2.zero;
                transform.offsetMax = Vector2.zero;
                layers.Add(i, transform);
            }
        }


        private static async Task<UIPanel> LoadAsync(Type type)
        {
            var obj = await AssetManager.Load<GameObject>(GlobalSetting.GetUIPath(type.Name));
            var panel = obj.GetComponent<UIPanel>() ?? (UIPanel)obj.AddComponent(type);
            panel.transform.SetParent(layers[panel.layer], false);
            panels.Add(type.Name, panel);
            panel.name = type.Name;
            return panel;
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
        public static void AddGroup(string key, UIPanel panel)
        {
            if (!groups.TryGetValue(key, out var group))
            {
                group = new List<UIPanel>();
                groups.Add(key, group);
            }

            if (!group.Contains(panel))
            {
                panel.group = key;
                group.Add(panel);
            }
        }

        public static void RemoveGroup(string key, UIPanel panel)
        {
            if (!groups.TryGetValue(key, out var group))
            {
                return;
            }

            if (group.Contains(panel))
            {
                panel.group = string.Empty;
                group.Remove(panel);
            }
        }

        public static void HideGroup(string key)
        {
            if (groups.TryGetValue(key, out var group))
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
            if (!panels.TryGetValue(typeof(TPanel).Name, out var panel))
            {
                panel = await LoadAsync(typeof(TPanel));
            }

            ShowInGroup(panel);
        }

        public static async void Show<TPanel>(Action action) where TPanel : UIPanel
        {
            if (!GlobalManager.Instance) return;
            if (!panels.TryGetValue(typeof(TPanel).Name, out var panel))
            {
                panel = await LoadAsync(typeof(TPanel));
            }

            ShowInGroup(panel);
            action?.Invoke();
        }

        public static async void Show<TPanel>(Action<TPanel> action) where TPanel : UIPanel
        {
            if (!GlobalManager.Instance) return;
            if (!panels.TryGetValue(typeof(TPanel).Name, out var panel))
            {
                panel = await LoadAsync(typeof(TPanel));
            }

            ShowInGroup(panel);
            action?.Invoke((TPanel)panel);
        }

        public static void Hide<TPanel>() where TPanel : UIPanel
        {
            if (!GlobalManager.Instance) return;
            if (panels.TryGetValue(typeof(TPanel).Name, out var panel))
            {
                if (panel.gameObject.activeInHierarchy)
                {
                    panel.Hide();
                }
            }
        }

        public static TPanel Get<TPanel>() where TPanel : UIPanel
        {
            return (TPanel)panels.GetValueOrDefault(typeof(TPanel).Name);
        }

        public static bool IsActive<TPanel>() where TPanel : UIPanel
        {
            return panels.TryGetValue(typeof(TPanel).Name, out var panel) && panel.gameObject.activeInHierarchy;
        }
    }
}