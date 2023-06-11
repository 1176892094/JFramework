using System;

namespace JFramework.Interface
{
    /// <summary>
    /// 计时器接口
    /// </summary>
    public interface ITimer
    {
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
        ITimer SetLoop(int count);
    }
}