// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2024-12-24 01:12:30
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.Collections.Generic;

namespace JFramework
{
    public static partial class Service
    {
        public static class Group
        {
            public static void Listen(string group, IPanel panel)
            {
                if (!Service.panelGroup.TryGetValue(group, out var panelGroup))
                {
                    panelGroup = new HashSet<IPanel>();
                    Service.panelGroup.Add(group, panelGroup);
                }

                if (!Service.groupPanel.TryGetValue(panel, out var groupPanel))
                {
                    groupPanel = new HashSet<string>();
                    Service.groupPanel.Add(panel, groupPanel);
                }

                if (panelGroup.Add(panel))
                {
                    groupPanel.Add(group);
                }
            }

            public static void Remove(string group, IPanel panel)
            {
                if (!Service.panelGroup.TryGetValue(group, out var panelGroup))
                {
                    panelGroup = new HashSet<IPanel>();
                    Service.panelGroup.Add(group, panelGroup);
                }

                if (!Service.groupPanel.TryGetValue(panel, out var groupPanel))
                {
                    groupPanel = new HashSet<string>();
                    Service.groupPanel.Add(panel, groupPanel);
                }

                if (panelGroup.Remove(panel))
                {
                    groupPanel.Remove(group);
                }
            }

            public static void Show(string group)
            {
                if (Service.panelGroup.TryGetValue(group, out var panelGroup))
                {
                    foreach (var panel in panelGroup)
                    {
                        if (!IsActive(panel))
                        {
                            panel.Show();
                        }
                    }
                }
            }

            public static void Hide(string group)
            {
                if (Service.panelGroup.TryGetValue(group, out var panelGroup))
                {
                    foreach (var panel in panelGroup)
                    {
                        if (IsActive(panel))
                        {
                            panel.Hide();
                        }
                    }
                }
            }

            internal static bool ShowInGroup(IPanel panel)
            {
                if (!Service.groupPanel.TryGetValue(panel, out var groupPanel))
                {
                    groupPanel = new HashSet<string>();
                    Service.groupPanel.Add(panel, groupPanel);
                }

                foreach (var group in groupPanel)
                {
                    if (!Service.panelGroup.TryGetValue(group, out var panelGroup))
                    {
                        continue;
                    }

                    foreach (var other in panelGroup)
                    {
                        if (panel != other && IsActive(other))
                        {
                            Pool.Hide(other);
                        }
                    }
                }

                return !IsActive(panel);
            }

            internal static void Dispose()
            {
                var groupPanel = new List<IPanel>(Service.groupPanel.Keys);
                foreach (var panel in groupPanel)
                {
                    if (Service.groupPanel.TryGetValue(panel, out var group))
                    {
                        group.Clear();
                    }
                }

                Service.groupPanel.Clear();

                var panelGroup = new List<string>(Service.panelGroup.Keys);
                foreach (var group in panelGroup)
                {
                    if (Service.panelGroup.TryGetValue(group, out var panel))
                    {
                        panel.Clear();
                    }
                }

                Service.panelGroup.Clear();
            }
        }
    }
}