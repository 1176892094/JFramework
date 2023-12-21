// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-12-21  20:54
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using JFramework;
using JFramework.Interface;
using UnityEngine;

namespace JFramework
{
    /// <summary>
    /// 实体的抽象类
    /// </summary>
    [Serializable]
    public abstract class Entity : MonoBehaviour, IInject
    {
        /// <summary>
        /// 实体初始化注入
        /// </summary>
        protected virtual void Awake() => this.Inject();

        /// <summary>
        /// 实体启用
        /// </summary>
        protected virtual void OnEnable() => GetComponent<IUpdate>()?.Listen();

        /// <summary>
        /// 实体禁用
        /// </summary>
        protected virtual void OnDisable() => GetComponent<IUpdate>()?.Remove();

        /// <summary>
        /// 实体销毁 (销毁控制器)
        /// </summary>
        protected virtual void OnDestroy() => gameObject.UnRegister();
    }
}