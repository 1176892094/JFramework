// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-19 21:12:38
// # Recently: 2024-12-22 20:12:38
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using UnityEngine;

namespace JFramework
{
    public abstract class UIPanel : MonoBehaviour, IPanel
    {
        protected virtual void Awake()
        {
            this.Inject();
        }

        protected virtual void OnDestroy()
        {
            this.Destroy();
        }

        public UIState state { get; set; } = UIState.Common;

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}