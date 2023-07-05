namespace JFramework.Interface
{
    /// <summary>
    /// 计时器接口
    /// </summary>
    public interface ITimer
    {
        /// <summary>
        /// 剩余循环次数(每次循环都会减去1)
        /// </summary>
        int Count { get; }

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
        ITimer Loop(int count);

        /// <summary>
        /// 计时器推入
        /// </summary>
        void Push();
    }
}