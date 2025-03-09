using System.Collections.Generic;
using JFramework.Common;
using UnityEngine;

namespace JFramework
{
    public interface IScroll<TItem> : IAgent
    {
        public Rect assetRect { get; set; }
        public string assetPath { get; set; }
        public ScrollType direction { get; set; }
        void SetItem(List<TItem> items);
    }
}