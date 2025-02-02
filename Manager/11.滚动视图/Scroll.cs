// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-09 16:01:50
// # Recently: 2025-01-10 20:01:59
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using JFramework.Common;
using UnityEngine;

namespace JFramework
{
    [Serializable]
    internal sealed class Scroll<TPanel, TItem, TGrid> where TPanel : UIPanel, IScroll where TGrid : Component, IGrid<TPanel, TItem>
    {
        private Dictionary<int, TGrid> grids = new Dictionary<int, TGrid>();
        private List<TItem> items;
        private int oldMaxIndex = -1;
        private int oldMinIndex = -1;
        private TPanel owner;
        private int row => (int)owner.rect.y;
        private int column => (int)owner.rect.x;
        private float width => owner.rect.width;
        private float height => owner.rect.height;
        private string prefab => owner.prefab;

        public Scroll(Component owner)
        {
            this.owner = (TPanel)owner;
            this.owner.content.pivot = Vector2.up;
            this.owner.content.anchorMin = Vector2.up;
            this.owner.content.anchorMax = Vector2.one;
        }

        public void Dispose()
        {
            items = null;
            foreach (var i in grids.Keys)
            {
                if (grids.TryGetValue(i, out var grid))
                {
                    if (grid != null)
                    {
                        grid.Dispose();
                        PoolManager.Hide(grid.gameObject);
                    }
                }
            }

            grids.Clear();
            oldMinIndex = -1;
            oldMaxIndex = -1;
        }

        public void SetItem(List<TItem> items)
        {
            Dispose();
            this.items = items;
            owner.content.anchoredPosition = Vector2.zero;
            owner.content.sizeDelta = new Vector2(0, Mathf.Ceil((float)items.Count / column * height + 1));
        }

        public void Update()
        {
            if (items == null) return;
            var position = owner.content.anchoredPosition.y;
            var minIndex = Mathf.Max(0, (int)(position / height) * column);
            var maxIndex = Mathf.Min((int)((position + row * height) / height) * column + column - 1, items.Count - 1);

            if (minIndex != oldMinIndex || maxIndex != oldMaxIndex)
            {
                for (var i = oldMinIndex; i < minIndex; ++i)
                {
                    if (grids.TryGetValue(i, out var grid))
                    {
                        if (grid != null)
                        {
                            grid.Dispose();
                            PoolManager.Hide(grid.gameObject);
                        }

                        grids.Remove(i);
                    }
                }

                for (var i = maxIndex + 1; i <= oldMaxIndex; ++i)
                {
                    if (grids.TryGetValue(i, out var grid))
                    {
                        if (grid != null)
                        {
                            grid.Dispose();
                            PoolManager.Hide(grid.gameObject);
                        }

                        grids.Remove(i);
                    }
                }
            }

            oldMinIndex = minIndex;
            oldMaxIndex = maxIndex;
            for (var i = minIndex; i <= maxIndex; ++i)
            {
                if (!grids.ContainsKey(i))
                {
                    var index = i;
                    grids[index] = default;
                    var posX = index % column * width + width / 2;
                    var posY = -(index / column) * height - height / 2;
                    PoolManager.Show(prefab, obj =>
                    {
                        var grid = obj.GetComponent<TGrid>();
                        if (grid == null)
                        {
                            grid = obj.AddComponent<TGrid>();
                        }

                        var transform = grid.transform;
                        transform.SetParent(owner.content);
                        transform.localScale = Vector3.one;
                        transform.localPosition = new Vector3(posX, posY, 0);
                        ((RectTransform)transform).sizeDelta = new Vector2(width, height);

                        if (!grids.ContainsKey(index))
                        {
                            grid.Dispose();
                            PoolManager.Hide(grid.gameObject);
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