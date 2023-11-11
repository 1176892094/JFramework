using System;
using System.Collections.Generic;
using JFramework.Core;
using JFramework.Interface;
using UnityEngine;

namespace JFramework
{
    /// <summary>
    /// 自定义滚动条
    /// </summary>
    /// <typeparam name="TItem">数据类</typeparam>
    /// <typeparam name="TGrid">格子类</typeparam>
    public class UIScroll<TItem, TGrid> where TGrid : IGrid<TItem> where TItem : IItem
    {
        /// <summary>
        /// 存储格子的字典
        /// </summary>
        private readonly Dictionary<int, GameObject> grids = new Dictionary<int, GameObject>();

        /// <summary>
        /// 履带对象
        /// </summary>
        private RectTransform content;

        /// <summary>
        /// 数据来源
        /// </summary>
        private List<TItem> items;

        /// <summary>
        /// 记录最上方显示的索引
        /// </summary>
        private int oldMinIndex = -1;

        /// <summary>
        /// 记录最下方显示的索引
        /// </summary>
        private int oldMaxIndex = -1;

        /// <summary>
        /// 矩阵
        /// </summary>
        private Rect rect;

        /// <summary>
        /// 格子的行数
        /// </summary>
        private int row;

        /// <summary>
        /// 格子的列数
        /// </summary>
        private int column;

        /// <summary>
        /// 预设体资源的路径
        /// </summary>
        private string path;

        /// <summary>
        /// 初始化Content
        /// </summary>
        /// <param name="content"></param>
        public void SetContent(RectTransform content)
        {
            this.content = content;
        }

        /// <summary>
        /// 初始化数据来源 并且把content的高初始化
        /// </summary>
        /// <param name="items"></param>
        /// <param name="path"></param>
        public void InitItems(List<TItem> items, string path)
        {
            this.path = path;
            this.items = items;
            content.sizeDelta = new Vector2(0, Mathf.CeilToInt((float)items.Count / column) * (rect.height + rect.y) + 1);
        }

        /// <summary>
        /// 初始化格子间隔大小
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        public void InitGrids(Rect rect, int row, int column)
        {
            this.rect = rect;
            this.column = column;
            this.row = (int)(row * rect.height);
        }

        /// <summary>
        /// 更新格子显示的方法
        /// </summary>
        public void OnUpdate()
        {
            var position = content.anchoredPosition;
            int minIndex = Math.Max(0, (int)(position.y / (rect.height + rect.y)) * column);
            int maxIndex = Math.Min((int)((position.y + row) / (rect.height + rect.y)) * column + column - 1, items.Count - 1);

            if (minIndex != oldMinIndex || maxIndex != oldMaxIndex)
            {
                for (int i = oldMinIndex; i < minIndex; ++i)
                {
                    if (grids.TryGetValue(i, out var grid))
                    {
                        if (grid != null)
                        {
                            PoolManager.Push(grid);
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
                            PoolManager.Push(grid);
                        }

                        grids.Remove(i);
                    }
                }
            }

            oldMinIndex = minIndex;
            oldMaxIndex = maxIndex;

            for (int i = minIndex; i <= maxIndex; ++i)
            {
                if (grids.ContainsKey(i)) continue;

                var index = i;
                grids.Add(index, null);
                PoolManager.PopAsync(path, obj =>
                {
                    obj.transform.SetParent(content);
                    obj.transform.localScale = Vector3.one;
                    var posX = index % column * (rect.width + rect.x) + rect.width / 2;
                    var posY = -(index / column) * (rect.height + rect.y) - rect.height / 2;
                    obj.transform.localPosition = new Vector3(posX, posY, 0);
                    obj.GetComponent<TGrid>().SetItem(items[index]);
                    if (grids.ContainsKey(index))
                    {
                        grids[index] = obj;
                    }
                    else
                    {
                        PoolManager.Push(obj);
                    }
                });
            }
        }
    }
}