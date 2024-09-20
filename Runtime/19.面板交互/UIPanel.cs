// *********************************************************************************
// # Project: Forest
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-08-25  01:08
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using JFramework.Interface;
using UnityEngine;

namespace JFramework
{
    [Serializable]
    public abstract class UIPanel : MonoBehaviour, IEntity
    {
        public Type group;

        public UILayer layer = UILayer.Normal;

        public UIState state = UIState.Common;

        protected virtual void Awake() => this.Inject();

        public virtual void Show() => gameObject.SetActive(true);

        public virtual void Hide() => gameObject.SetActive(false);
    }
}