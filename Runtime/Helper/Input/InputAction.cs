using System;

namespace JFramework
{
    /// <summary>
    /// 输入事件
    /// </summary>
    public class InputAction
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
        internal void OnDownEvent() => OnEnter?.Invoke();

        /// <summary>
        /// 退出
        /// </summary>
        internal void OnUpEvent() => OnExit?.Invoke();

        /// <summary>
        /// 清除
        /// </summary>
        internal void Clear()
        {
            OnEnter = null;
            OnExit = null;
        }
    }
}