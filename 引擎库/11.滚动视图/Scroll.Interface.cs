using System.Collections.Generic;
using UnityEngine;

namespace JFramework.Common
{
    public interface IScroll<TItem> : IAgent
    {
        public Rect assetRect { get; set; }
        public string assetPath { get; set; }
        public UIState direction { get; set; }
        void SetItem(List<TItem> items);
    }
}