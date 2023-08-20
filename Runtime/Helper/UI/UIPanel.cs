using JFramework.Interface;
using UnityEngine;

// ReSharper disable All
namespace JFramework
{
    /// <summary>
    /// UI面板的抽象类
    /// </summary>
    public abstract class UIPanel : MonoBehaviour, IPanel, IInject
    {
        /// <summary>
        /// UI层级
        /// </summary>
        public UILayer layer { get; protected set; } = UILayer.Normal;

        /// <summary>
        /// UI隐藏类型
        /// </summary>
        public UIState state { get; protected set; } = UIState.Default;

        /// <summary>
        /// 开始时查找所有控件
        /// </summary>
        protected virtual void Awake() => GetComponent<IInject>()?.Inject(this);

        /// <summary>
        /// 实体启用
        /// </summary>
        protected virtual void OnEnable() => GetComponent<IUpdate>()?.Listen();

        /// <summary>
        /// 实体禁用
        /// </summary>
        protected virtual void OnDisable() => GetComponent<IUpdate>()?.Remove();

        /// <summary>
        /// 显示UI面板
        /// </summary>
        public virtual void Show() => gameObject.SetActive(true);

        /// <summary>
        /// 隐藏UI面板
        /// </summary>
        public virtual void Hide() => gameObject.SetActive(false);
    }
}