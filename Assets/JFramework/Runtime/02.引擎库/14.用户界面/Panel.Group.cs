// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-09 16:01:50
// # Recently: 2025-01-10 20:01:58
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.Collections.Generic;

namespace JFramework.Common
{
    public static partial class UIManager
    {
        public static void Listen(string group, UIPanel panel)
        {
            if (!GlobalManager.Instance) return;
            if (!GlobalManager.panelGroup.TryGetValue(group, out var panelGroup))
            {
                panelGroup = new HashSet<UIPanel>();
                GlobalManager.panelGroup.Add(group, panelGroup);
            }

            if (panelGroup.Add(panel))
            {
                panel.groups.Add(group);
            }
        }

        public static void Remove(string group, UIPanel panel)
        {
            if (!GlobalManager.Instance) return;
            if (!GlobalManager.panelGroup.TryGetValue(group, out var panelGroup))
            {
                panelGroup = new HashSet<UIPanel>();
                GlobalManager.panelGroup.Add(group, panelGroup);
            }

            if (panelGroup.Remove(panel))
            {
                panel.groups.Remove(group);
            }
        }

        public static void Show(string group)
        {
            if (!GlobalManager.Instance) return;
            if (GlobalManager.panelGroup.TryGetValue(group, out var panelGroup))
            {
                foreach (var panel in panelGroup)
                {
                    if (!panel.gameObject.activeInHierarchy)
                    {
                        panel.Show();
                    }
                }
            }
        }

        public static void Hide(string group)
        {
            if (!GlobalManager.Instance) return;
            if (GlobalManager.panelGroup.TryGetValue(group, out var panelGroup))
            {
                foreach (var panel in panelGroup)
                {
                    if (panel.gameObject.activeInHierarchy)
                    {
                        panel.Hide();
                    }
                }
            }
        }

        private static bool ShowInGroup(UIPanel panel)
        {
            foreach (var group in panel.groups)
            {
                if (!GlobalManager.panelGroup.TryGetValue(group, out var panelGroup))
                {
                    continue;
                }

                foreach (var other in panelGroup)
                {
                    if (panel != other && other.gameObject.activeInHierarchy)
                    {
                        other.Hide();
                    }
                }
            }
            return !panel.gameObject.activeInHierarchy;
        }

        internal static void Dispose()
        {
            var panelGroup = new List<string>(GlobalManager.panelGroup.Keys);
            foreach (var group in panelGroup)
            {
                if (GlobalManager.panelGroup.TryGetValue(group, out var panel))
                {
                    panel.Clear();
                }
            }

            GlobalManager.panelData.Clear();
            GlobalManager.panelGroup.Clear();
            GlobalManager.panelLayer.Clear();
        }
    }
}