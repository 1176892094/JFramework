// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-10-24  23:59
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;

namespace JFramework.Interface
{
    /// <summary>
    /// 计时器接口
    /// </summary>
    public partial interface ITimer
    {
        /// <summary>
        /// 剩余循环次数(每次循环都会减去1)
        /// </summary>
        int count { get; }
        
        /// <summary>
        /// 计时器执行方法
        /// </summary>
        /// <param name="action">传入委托</param>
        /// <returns>返回自身</returns>
        ITimer Invoke(Action action);
        
        /// <summary>
        /// 计时器执行方法
        /// </summary>
        /// <param name="action">传入委托</param>
        /// <returns>返回自身</returns>
        ITimer Invoke(Action<ITimer> action);
        
        /// <summary>
        /// 计时器播放
        /// </summary>
        /// <returns>返回自身</returns>
        ITimer Play();

        /// <summary>
        /// 计时器暂停
        /// </summary>
        /// <returns>返回自身</returns>
        ITimer Stop();

        /// <summary>
        /// 计时器忽视TimeScale
        /// </summary>
        /// <returns>返回自身</returns>
        ITimer Unscale();

        /// <summary>
        /// 计时器循环
        /// </summary>
        /// <param name="count">循环次数</param>
        /// <returns>返回自身</returns>
        ITimer Loops(int count = 0);
    }

    /// <summary>
    /// 计时器接口(内部)
    /// </summary>
    public partial interface ITimer
    {
        /// <summary>
        /// 计时器开启
        /// </summary>
        /// <param name="duration"></param>
        internal void Start(float duration);

        /// <summary>
        /// 计时器更新
        /// </summary>
        internal void Update();

        /// <summary>
        /// 计时器回收
        /// </summary>
        internal void Close();
    }
}