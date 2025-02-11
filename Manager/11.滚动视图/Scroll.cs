// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-09 16:01:50
// # Recently: 2025-02-12 01:02:28
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
    internal abstract class Scroll<TItem, TGrid> : UIPanel, IScroll where TGrid : Component, IGrid<Scroll<TItem, TGrid>, TItem>
    {
        private Dictionary<int, TGrid> grids;
        private List<TItem> items;
        private int oldMaxIndex = -1;
        private int oldMinIndex = -1;

        [Inject] public RectTransform content;
        [SerializeField] protected Rect assetRect;
        [SerializeField] protected string assetPath;

        protected Scroll(RectTransform content)
        {
            this.content = content;
        }

        private int row => (int)assetRect.y;
        private int column => (int)assetRect.x;

        protected override void Awake()
        {
            base.Awake();
            content.pivot = Vector2.up;
            content.anchorMin = Vector2.up;
            content.anchorMax = Vector2.one;
            grids = new Dictionary<int, TGrid>();
        }

        protected virtual void Update()
        {
            if (items == null) return;
            var position = content.anchoredPosition.y;
            var minIndex = Mathf.Max(0, (int)(position / assetRect.height) * column);
            var maxIndex = Mathf.Min((int)((position + row * assetRect.height) / assetRect.height) * column + column - 1, items.Count - 1);

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
                    var posX = index % column * assetRect.width + assetRect.width / 2;
                    var posY = -(index / column) * assetRect.height - assetRect.height / 2;
                    PoolManager.Show(assetPath, obj =>
                    {
                        var grid = obj.GetComponent<TGrid>();
                        if (grid == null)
                        {
                            grid = obj.AddComponent<TGrid>();
                        }

                        var target = (RectTransform)grid.transform;
                        target.SetParent(content);
                        target.sizeDelta = new Vector2(assetRect.width, assetRect.height);
                        target.localScale = Vector3.one;
                        target.localPosition = new Vector3(posX, posY, 0);

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

        public override void Hide()
        {
            base.Hide();
            Dispose();
        }

        public void SetItem(List<TItem> items)
        {
            Dispose();
            this.items = items;
            content.anchoredPosition = Vector2.zero;
            content.sizeDelta = new Vector2(0, Mathf.Ceil((float)items.Count / column * assetRect.height + 1));
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

        RectTransform IScroll.content => content;
    }
}