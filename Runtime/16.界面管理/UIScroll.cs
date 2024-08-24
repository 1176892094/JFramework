// *********************************************************************************
// # Project: Forest
// # Unity: 2022.3.5f1c1
// # Author: jinyijie
// # Version: 1.0.0
// # History: 2024-08-25  01:08
// # Copyright: 2024, jinyijie
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using JFramework.Core;
using JFramework.Interface;
using UnityEngine;

namespace JFramework
{
    [Serializable]
    public class UIScroll<TItem, TGrid> where TGrid : Component, IGrid<TItem> where TItem : new()
    {
        private readonly Dictionary<int, TGrid> grids = new Dictionary<int, TGrid>();
        private int oldMinIndex = -1;
        private int oldMaxIndex = -1;
        private int row;
        private int column;
        private float width;
        private float height;
        private string prefab;
        private List<TItem> items;
        private RectTransform content;

        public UIScroll(float width, float height, int row, int column, string prefab, RectTransform content)
        {
            this.row = row;
            this.width = width;
            this.height = height;
            this.column = column;
            this.prefab = prefab;
            this.content = content;
            content.pivot = Vector2.up;
            content.anchorMin = Vector2.up;
            content.anchorMax = Vector2.one;
        }

        public void SetItem(List<TItem> items)
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

        public void Update()
        {
            if (items == null) return;
            var position = content.anchoredPosition;
            var minIndex = Math.Max(0, (int)(position.y / height) * column);
            var maxIndex = Math.Min((int)((position.y + row * height) / height) * column + column - 1, items.Count - 1);

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
            for (int i = minIndex; i <= maxIndex; ++i)
            {
                if (grids.TryAdd(i, default))
                {
                    var index = i;
                    PoolManager.Pop(prefab, obj =>
                    {
                        obj.transform.SetParent(content);
                        obj.transform.localScale = Vector3.one;
                        var x = index % column * width + width / 2;
                        var y = -(index / column) * height - height / 2;
                        obj.transform.localPosition = new Vector3(x, y, 0);
                        var grid = obj.GetComponent<TGrid>() ?? obj.AddComponent<TGrid>();
                        if (!grids.ContainsKey(index))
                        {
                            grid.Dispose();
                            PoolManager.Push(obj);
                            return;
                        }

                        grids[index] = grid;
                        grid.SetItem(items[index]);
                    });
                }
            }
        }
    }
}