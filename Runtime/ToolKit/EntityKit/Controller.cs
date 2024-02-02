// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-12-21  17:47
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using JFramework.Interface;
using UnityEngine;

namespace JFramework
{
    /// <summary>
    /// 控制器
    /// </summary>
    public abstract class Controller<T> : ScriptableObject, IController where T : IEntity
    {
        /// <summary>
        /// 所有者
        /// </summary>
        private T instance;

        /// <summary>
        /// 所有者的游戏对象
        /// </summary>
        public T owner => instance ??= (T)GlobalManager.Entity.instance;

        /// <summary>
        /// 注册实体单位
        /// </summary>
        void IController.Awake() => instance ??= (T)GlobalManager.Entity.instance;
    }
}