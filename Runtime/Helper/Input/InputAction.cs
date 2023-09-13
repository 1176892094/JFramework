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