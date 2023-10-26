// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-10-25  00:04
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using JFramework.Interface;

namespace JFramework
{
    /// <summary>
    /// 输入事件
    /// </summary>
    public class InputAction : IInput
    {
        /// <summary>
        /// 按下
        /// </summary>
        public event Action OnEnter;

        /// <summary>
        /// 弹起
        /// </summary>
        public event Action OnExit;

        /// <summary>
        /// 按下
        /// </summary>
        void IEnter.OnEnter() => OnEnter?.Invoke();

        /// <summary>
        /// 弹起
        /// </summary>
        void IExit.OnExit() => OnExit?.Invoke();

        /// <summary>
        /// 清空
        /// </summary>
        internal void Clear()
        {
            OnEnter = null;
            OnExit = null;
        }
    }
}