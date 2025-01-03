// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2024-12-24 01:12:29
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;

namespace JFramework
{
    public static partial class Service
    {
        public static class Panel
        {
            public static async void Show<T>(Action<T> assetAction = null) where T : IPanel
            {
                if (helper == null) return;
                var assetPath = GetPanelPath(typeof(T).Name);
                if (!panelData.TryGetValue(typeof(T), out var panel))
                {
                    panel = (IPanel)await panelHelper.Instantiate(assetPath, typeof(T));
                    panelData.Add(typeof(T), panel);
                    Surface(panel);
                    panel.Show();
                }

                if (Group.ShowInGroup(panel))
                {
                    panel.Show();
                }

                assetAction?.Invoke((T)panel);
            }

            public static void Hide<T>() where T : IPanel
            {
                if (helper == null) return;
                if (panelData.TryGetValue(typeof(T), out var panel))
                {
                    if (IsActive(panel))
                    {
                        panel.Hide();
                    }
                }
            }

            public static T Find<T>() where T : IPanel
            {
                if (helper == null) return default;
                if (panelData.TryGetValue(typeof(T), out var panel))
                {
                    return (T)panel;
                }

                return default;
            }

            public static void Destroy<T>()
            {
                if (helper == null) return;
                if (panelData.TryGetValue(typeof(T), out var panel))
                {
                    Destroy(panel, typeof(T));
                }
            }

            public static async void Show(Type assetType, Action<IPanel> assetAction = null)
            {
                if (helper == null) return;
                var assetPath = GetPanelPath(assetType.Name);
                if (!panelData.TryGetValue(assetType, out var panel))
                {
                    panel = (IPanel)await panelHelper.Instantiate(assetPath, assetType);
                    panelData.Add(assetType, panel);
                    Surface(panel);
                    panel.Show();
                }

                if (Group.ShowInGroup(panel))
                {
                    panel.Show();
                }

                assetAction?.Invoke(panel);
            }

            public static void Hide(Type assetType)
            {
                if (helper == null) return;
                if (panelData.TryGetValue(assetType, out var panel))
                {
                    if (IsActive(panel))
                    {
                        panel.Hide();
                    }
                }
            }

            public static IPanel Find(Type assetType)
            {
                if (helper == null) return default;
                if (panelData.TryGetValue(assetType, out var panel))
                {
                    return panel;
                }

                return default;
            }

            public static void Destroy(Type assetType)
            {
                if (helper == null) return;
                if (panelData.TryGetValue(assetType, out var panel))
                {
                    Destroy(panel, assetType);
                }
            }

            public static void Clear()
            {
                var panelData = new List<Type>(Service.panelData.Keys);
                foreach (var assetType in panelData)
                {
                    if (Service.panelData.TryGetValue(assetType, out var panel))
                    {
                        if (panel.state != UIState.Stable)
                        {
                            Destroy(panel, assetType);
                        }
                    }
                }
            }

            public static void Surface(IPanel panel, int layer = 1)
            {
                if (helper == null) return;
                panelHelper.Surface(panel, layer);
            }

            private static void Destroy(IPanel panel, Type assetType)
            {
                if (helper == null) return;
                if (Service.groupPanel.TryGetValue(panel, out var groupPanel))
                {
                    foreach (var group in groupPanel)
                    {
                        panelGroup.Remove(group);
                    }

                    groupPanel.Clear();
                    Service.groupPanel.Remove(panel);
                }

                panelData.Remove(assetType);
                panelHelper.Destroy(panel);
            }

            internal static void Dispose()
            {
                panelData.Clear();
            }
        }
    }
}