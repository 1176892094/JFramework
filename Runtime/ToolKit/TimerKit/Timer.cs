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
using UnityEngine;

namespace JFramework
{
    /// <summary>
    /// 计时器
    /// </summary>
    [Serializable]
    public sealed partial class Timer
    {
        /// <summary>
        /// 剩余循环次数
        /// </summary>
        public int count;

        /// <summary>
        /// 持续时间
        /// </summary>
        private float duration;

        /// <summary>
        /// 当前时间+持续时间
        /// </summary>
        private float waitTime;

        /// <summary>
        /// 是否受TimeScale影响
        /// </summary>
        private bool unscaled;

        /// <summary>
        /// 计时器的状态
        /// </summary>
        private TimerState state;

        /// <summary>
        /// 完成时执行的事件
        /// </summary>
        private event Action OnComplete;

        /// <summary>
        /// 计时器执行方法
        /// </summary>
        /// <param name="OnComplete"></param>
        public Timer Invoke(Action OnComplete)
        {
            this.OnComplete = OnComplete;
            return this;
        }

        /// <summary>
        /// 计时器执行方法
        /// </summary>
        /// <param name="OnComplete"></param>
        public Timer Invoke(Action<Timer> OnComplete)
        {
            this.OnComplete = () => OnComplete(this);
            return this;
        }

        /// <summary>
        /// 计时器开始计时
        /// </summary>
        /// <returns>返回自身</returns>
        public Timer Play()
        {
            state = TimerState.Run;
            return this;
        }

        /// <summary>
        /// 计时器暂停计时
        /// </summary>
        /// <returns>返回自身</returns>
        public Timer Stop()
        {
            state = TimerState.Stop;
            return this;
        }

        /// <summary>
        /// 设置计时器的循环
        /// </summary>
        /// <param name="count">计时器循环次数</param>
        /// <returns>返回自身</returns>
        public Timer Loops(int count = 0)
        {
            this.count = count;
            return this;
        }

        /// <summary>
        /// 设置计时器是否受TimeScale影响
        /// </summary>
        /// <returns>返回自身</returns>
        public Timer Unscale()
        {
            unscaled = true;
            waitTime = Time.unscaledTime + duration;
            return this;
        }

        /// <summary>
        /// 重新设置间隔
        /// </summary>
        /// <returns></returns>
        public Timer Set(float duration)
        {
            this.duration = duration;
            waitTime = unscaled ? Time.unscaledTime : Time.time;
            waitTime += duration;
            return this;
        }

        /// <summary>
        /// 重新设置间隔
        /// </summary>
        /// <returns></returns>
        public Timer Add(float duration)
        {
            waitTime += duration;
            return this;
        }
    }

    /// <summary>
    /// 计时器内部方法
    /// </summary>
    public sealed partial class Timer
    {
        /// <summary>
        /// 开启计时器
        /// </summary>
        /// <param name="duration">延迟时间</param>
        internal void Start(float duration)
        {
            count = 1;
            unscaled = false;
            state = TimerState.Run;
            this.duration = duration;
            waitTime = Time.time + duration;
        }

        /// <summary>
        /// 计时器更新
        /// </summary>
        internal void Update()
        {
            if (state != TimerState.Run)
            {
                return;
            }

            var current = unscaled ? Time.unscaledTime : Time.time;
            if (current <= waitTime)
            {
                return;
            }

            waitTime = current + duration;
            try
            {
                count--;
                OnComplete?.Invoke();
                if (count == 0)
                {
                    GlobalManager.Time.Push(this);
                }
            }
            catch (Exception)
            {
                GlobalManager.Time.Push(this);
            }
        }

        /// <summary>
        /// 计时器回收
        /// </summary>
        internal void Close()
        {
            unscaled = false;
            OnComplete = null;
            state = TimerState.Complete;
        }
    }
}