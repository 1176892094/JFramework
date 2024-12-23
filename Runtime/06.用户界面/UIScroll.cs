// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-19 02:12:55
// # Recently: 2024-12-22 20:12:35
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using UnityEngine;

namespace JFramework
{
    [Serializable]
    public sealed class UIScroll<T, TItem, TGrid> : Scroll<T, TItem, TGrid> where T : IPanel, IScroll where TGrid : IGrid<T, TItem>
    {
        protected override void Awake()
        {
            base.Awake();
            var content = (RectTransform)owner.content;
            content.pivot = Vector2.up;
            content.anchorMin = Vector2.up;
            content.anchorMax = Vector2.one;
        }

        protected override void SetItem(float height)
        {
            var content = (RectTransform)owner.content;
            content.anchoredPosition = Vector2.zero;
            content.sizeDelta = new Vector2(0, height);
        }

        protected override void SetGrid(TGrid grid, float posX, float posY)
        {
            var transform = grid.GetComponent<RectTransform>();
            transform.SetParent(owner.content as RectTransform);
            transform.sizeDelta = new Vector2(grid.scroll.width, grid.scroll.height);
            transform.localPosition = new Vector3(posX, posY, 0);
            transform.localScale = Vector3.one;
        }
    }
}