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
using JFramework.Interface;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JFramework.Core
{
    public static partial class UIManager
    {
        public static Canvas canvas { get; private set; }
        private static readonly Dictionary<UILayer, Transform> layers = new();
        internal static readonly Dictionary<Type, UIPanel> panels = new();

        internal static void Register()
        {
            canvas = GlobalManager.Instance.transform.Find("UICanvas").GetComponent<Canvas>();
            layers[UILayer.Bottom] = canvas.transform.Find("Layer1");
            layers[UILayer.Normal] = canvas.transform.Find("Layer2");
            layers[UILayer.Middle] = canvas.transform.Find("Layer3");
            layers[UILayer.Height] = canvas.transform.Find("Layer4");
            layers[UILayer.Ignore] = canvas.transform.Find("Layer5");
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
                panel = await Load<TPanel>();
            }

            panel.Show();
        }

        public static async void Show<TPanel>(Action action) where TPanel : UIPanel
        {
            if (!GlobalManager.Instance) return;
            if (!panels.TryGetValue(typeof(TPanel), out var panel))
            {
                panel = await Load<TPanel>();
            }

            panel.Show();
            action?.Invoke();
        }

        public static async void Show<TPanel>(Action<TPanel> action) where TPanel : UIPanel
        {
            if (!GlobalManager.Instance) return;
            if (!panels.TryGetValue(typeof(TPanel), out var panel))
            {
                panel = await Load<TPanel>();
            }

            panel.Show();
            action?.Invoke((TPanel)panel);
        }

        private static async Task<TPanel> Load<TPanel>() where TPanel : UIPanel
        {
            panels[typeof(TPanel)] = null;
            var obj = await AssetManager.Load<GameObject>(SettingManager.GetUIPath(typeof(TPanel).Name));
            var panel = obj.GetComponent<TPanel>() ?? obj.AddComponent<TPanel>();
            panel.transform.SetParent(layers[panel.layer], false);
            panels[typeof(TPanel)] = panel;
            return panel;
        }

        public static void Hide<TPanel>() where TPanel : UIPanel
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

        private static async Task<UIPanel> Load(Type type)
        {
            panels[type] = null;
            var obj = await AssetManager.Load<GameObject>(SettingManager.GetUIPath(type.Name));
            var panel = obj.GetComponent<UIPanel>() ?? (UIPanel)obj.AddComponent(type);
            panel.transform.SetParent(layers[panel.layer], false);
            panels[typeof(UIPanel)] = panel;
            return panel;
        }

        public static void Hide(Type type)
        {
            if (!GlobalManager.Instance) return;
            if (panels.TryGetValue(type, out var panel))
            {
                if (IsActive(type))
                {
                    panel.Hide();
                }
            }
        }

        public static UIPanel Get(Type key)
        {
            return panels.GetValueOrDefault(key);
        }

        public static bool IsActive(Type type)
        {
            return panels.TryGetValue(type, out var panel) && panel.gameObject.activeInHierarchy;
        }
    }

    [Serializable]
    public class UIScroll<TItem, TGrid> where TGrid : IGrid<TItem> where TItem : new()
    {
        private readonly Dictionary<int, TGrid> grids = new Dictionary<int, TGrid>();
        private RectTransform content;
        private List<TItem> items;
        private int oldMinIndex = -1;
        private int oldMaxIndex = -1;
        private int row;
        private int column;
        private float width;
        private float height;
        private string path;

        public void SetContent(RectTransform content, string path)
        {
            this.path = path;
            this.content = content;
        }

        public void InitGrids(int width, int height, int row, int column)
        {
            this.width = width;
            this.height = height;
            this.column = column;
            this.row = row * height;
        }

        public void Refresh(List<TItem> items)
        {
            foreach (var i in grids.Keys)
            {
                if (grids.TryGetValue(i, out var grid))
                {
                    if (grid != null)
                    {
                        PoolManager.Push(grid.gameObject);
                    }
                }
            }

            grids.Clear();
            this.items = items;
            content.anchoredPosition = Vector2.zero;
            content.sizeDelta = new Vector2(0, Mathf.CeilToInt((float)items.Count / column) * height + 1);
        }

        public async void OnUpdate()
        {
            if (items == null) return;
            var fixHeight = height;
            var position = content.anchoredPosition;
            var minIndex = Math.Max(0, (int)(position.y / fixHeight) * column);
            var maxIndex = Math.Min((int)((position.y + row) / fixHeight) * column + column - 1, items.Count - 1);

            if (minIndex != oldMinIndex || maxIndex != oldMaxIndex)
            {
                for (int i = oldMinIndex; i < minIndex; ++i)
                {
                    if (grids.TryGetValue(i, out var grid))
                    {
                        if (grid != null)
                        {
                            PoolManager.Push(grid.gameObject);
                        }

                        grids.Remove(i);
                    }
                }

                for (int i = maxIndex + 1; i <= oldMaxIndex; ++i)
                {
                    if (grids.TryGetValue(i, out var grid))
                    {
                        if (grid != null)
                        {
                            PoolManager.Push(grid.gameObject);
                        }

                        grids.Remove(i);
                    }
                }
            }

            oldMinIndex = minIndex;
            oldMaxIndex = maxIndex;

            for (int index = minIndex; index <= maxIndex; ++index)
            {
                if (!grids.TryAdd(index, default)) continue;
                var obj = await PoolManager.Pop(path);
                obj.transform.SetParent(content);
                obj.transform.localScale = Vector3.one;
                var posX = index % column * width + width / 2;
                var posY = -(index / column) * fixHeight - height / 2;
                obj.transform.localPosition = new Vector3(posX, posY, 0);
                if (obj.TryGetComponent(out TGrid grid))
                {
                    grid.SetItem(items[index]);
                }

                if (grids.ContainsKey(index))
                {
                    grids[index] = grid;
                }
                else
                {
                    PoolManager.Push(obj);
                }
            }
        }
    }
}