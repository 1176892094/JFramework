// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-07 14:01:12
// # Recently: 2025-01-07 14:01:47
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace JFramework
{
    public abstract class UIGrid<T, TItem> : Selectable, IGrid<T, TItem>, IPointerClickHandler, ISubmitHandler where T : IPanel
    {
        public TItem item { get; private set; }
        public T panel => Service.Panel.Find<T>();
        public IScroll scroll => (IScroll)panel;
        public object content => scroll.content;

        public virtual void Dispose() => item = default;

        public virtual void SetItem(TItem item) => this.item = item;
        public abstract void OnPointerClick(PointerEventData eventData);
        public abstract void OnSubmit(BaseEventData eventData);
    }
}