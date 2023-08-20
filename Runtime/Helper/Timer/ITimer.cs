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
        /// <param name="OnFinish"></param>
        internal void Start(float duration, Action OnFinish);

        /// <summary>
        /// 计时器开启(包含自身)
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="OnFinish"></param>
        internal void Start(float duration, Action<ITimer> OnFinish);

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