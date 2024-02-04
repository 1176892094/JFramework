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
        public int count;
        private bool unscaled;
        private float duration;
        private float waitTime;
        private TimerState state;
        private event Action OnComplete;

        public Timer Invoke(Action OnComplete)
        {
            this.OnComplete = OnComplete;
            return this;
        }

        public Timer Invoke(Action<Timer> OnComplete)
        {
            this.OnComplete = () => OnComplete(this);
            return this;
        }

        public Timer Play()
        {
            state = TimerState.Run;
            return this;
        }

        public Timer Stop()
        {
            state = TimerState.Stop;
            return this;
        }

        public Timer Loops(int count = 0)
        {
            this.count = count;
            return this;
        }

        public Timer Unscale()
        {
            unscaled = true;
            waitTime = Time.unscaledTime + duration;
            return this;
        }

        public Timer Set(float duration)
        {
            this.duration = duration;
            waitTime = unscaled ? Time.unscaledTime : Time.time;
            waitTime += duration;
            return this;
        }

        public Timer Add(float duration)
        {
            waitTime += duration;
            return this;
        }

        internal void Start(float duration)
        {
            count = 1;
            unscaled = false;
            state = TimerState.Run;
            this.duration = duration;
            waitTime = Time.time + duration;
        }

        internal void Update()
        {
            if (state != TimerState.Run)
            {
                return;
            }

            var current = unscaled ? Time.unscaledTime : Time.time;
            if (current <= waitTime)
            {
                return;
            }

            waitTime = current + duration;
            try
            {
                count--;
                OnComplete?.Invoke();
                if (count == 0)
                {
                    GlobalManager.Time.Push(this);
                }
            }
            catch (Exception)
            {
                GlobalManager.Time.Push(this);
            }
        }

        internal void Close()
        {
            unscaled = false;
            OnComplete = null;
            state = TimerState.Complete;
        }
    }

    [Serializable]
    public class UIScroll<TItem, TGrid> where TGrid : IGrid<TItem>, IUpdate where TItem : new()
    {
        private readonly Dictionary<int, TGrid> grids = new Dictionary<int, TGrid>();
        private RectTransform content;
        private List<TItem> items;
        private int oldMinIndex = -1;
        private int oldMaxIndex = -1;
        private Rect rect;
        private int row;
        private int column;
        private string path;

        public void SetContent(RectTransform content, string path)
        {
            this.path = path;
            this.content = content;
        }

        public void InitGrids(Rect rect, int row, int column)
        {
            this.rect = rect;
            this.column = column;
            this.row = (int)(row * (rect.height + rect.y));
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
            content.sizeDelta = new Vector2(0, Mathf.CeilToInt((float)items.Count / column) * (rect.height + rect.y) + 1);
        }

        public async void OnUpdate()
        {
            if (items == null) return;
            var height = rect.height + rect.y;
            var position = content.anchoredPosition;
            var minIndex = Math.Max(0, (int)(position.y / height) * column);
            var maxIndex = Math.Min((int)((position.y + row) / height) * column + column - 1, items.Count - 1);

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
                if (grids.ContainsKey(index)) continue;
                grids.Add(index, default);
                var obj = await GlobalManager.Pool.Pop(path);
                obj.transform.SetParent(content);
                obj.transform.localScale = Vector3.one;
                var posX = index % column * (rect.width + rect.x);
                var posY = -(index / column) * height - rect.height / 2;
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