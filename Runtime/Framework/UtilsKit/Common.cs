// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-02-04  18:08
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using JFramework.Interface;
using UnityEngine;

namespace JFramework
{
    [Serializable]
    public sealed class Variable<T> : IVariable where T : new()
    {
        public T value;

        public Variable() => value = new T();

        public Variable(T value) => this.value = value;
    }

    [Serializable]
    public sealed class Variables<T> : IVariable where T : struct
    {
        public List<T> value;

        public Variables() => value = new List<T>();

        public Variables(List<T> value) => this.value = value;
    }

    [Serializable]
    public sealed class Timer
    {
        private bool running;
        private bool unscale;
        [SerializeField] private int count;
        [SerializeField] private float interval;
        [SerializeField] private float duration;
        private event Action OnUpdate;
        private float seconds => unscale ? Time.unscaledTime : Time.time;

        public Timer Invoke(Action OnUpdate)
        {
            this.OnUpdate = OnUpdate;
            return this;
        }

        public Timer Loops(int count = 0)
        {
            this.count = count;
            return this;
        }

        public Timer Unscale()
        {
            unscale = true;
            duration = seconds + interval;
            return this;
        }

        public Timer Set(float interval)
        {
            this.interval = interval;
            duration = seconds + interval;
            return this;
        }

        public Timer Add(float duration)
        {
            this.duration += duration;
            return this;
        }

        internal Timer Pop(float interval)
        {
            count = 1;
            running = true;
            unscale = false;
            this.interval = interval;
            duration = seconds + interval;
            return this;
        }

        internal void Update()
        {
            if (!running || seconds <= duration)
            {
                return;
            }

            duration = seconds + interval;
            try
            {
                count--;
                OnUpdate?.Invoke();
                if (count == 0)
                {
                    GlobalManager.Time.Push(this);
                }
            }
            catch (Exception e)
            {
                GlobalManager.Time.Push(this);
                Debug.LogWarning("计时器无法执行方法：\n" + e);
            }
        }

        internal void Push()
        {
            running = false;
            unscale = false;
            OnUpdate = null;
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
            foreach (var (index, grid) in grids)
            {
                grid.Dispose();
                if (index < items.Count && items[index] != null)
                {
                    grid.SetItem(items[index]);
                }
            }

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
                            GlobalManager.Pool.Push(grid.gameObject);
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
                            GlobalManager.Pool.Push(grid.gameObject);
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
                var obj = await GlobalManager.Pool.Pop(path);
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
                    GlobalManager.Pool.Push(obj);
                }
            }
        }
    }
}