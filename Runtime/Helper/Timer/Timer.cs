using System;
using JFramework.Core;
using JFramework.Interface;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JFramework
{
    /// <summary>
    /// 计时器
    /// </summary>
    internal sealed partial class Timer : ITimer
    {
        /// <summary>
        /// 剩余循环次数
        /// </summary>
        public int count { get; private set; }

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
        [ShowInInspector] private TimerState state;

        /// <summary>
        /// 完成时执行的事件
        /// </summary>
        [ShowInInspector] private Action OnFinish;

        /// <summary>
        /// 当更新时执行的方法
        /// </summary>
        /// <param name="current">当前游戏时间</param>
        private void Update(float current)
        {
            if (current <= waitTime) return;
            waitTime = current + duration;
            try
            {
                OnFinish?.Invoke();
                if (--count == 0)
                {
                    TimerManager.Push(this);
                }
            }
            catch (Exception)
            {
                TimerManager.Push(this);
            }
        }

        /// <summary>
        /// 计时器开始计时
        /// </summary>
        /// <returns>返回自身</returns>
        public ITimer Play()
        {
            state = TimerState.Run;
            return this;
        }

        /// <summary>
        /// 计时器暂停计时
        /// </summary>
        /// <returns>返回自身</returns>
        public ITimer Stop()
        {
            state = TimerState.Stop;
            return this;
        }

        /// <summary>
        /// 设置计时器的循环
        /// </summary>
        /// <param name="count">计时器循环次数</param>
        /// <returns>返回自身</returns>
        public ITimer Loops(int count)
        {
            this.count = count;
            return this;
        }

        /// <summary>
        /// 设置计时器是否受TimeScale影响
        /// </summary>
        /// <returns>返回自身</returns>
        public ITimer Unscale()
        {
            unscaled = !unscaled;
            waitTime = Time.unscaledTime + duration;
            return this;
        }
    }

    /// <summary>
    /// 计时器内部方法
    /// </summary>
    internal sealed partial class Timer
    {
        /// <summary>
        /// 开启计时器
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="OnFinish"></param>
        void ITimer.Start(float duration, Action OnFinish)
        {
            count = 1;
            unscaled = false;
            state = TimerState.Run;
            this.duration = duration;
            waitTime = Time.time + duration;
            this.OnFinish = OnFinish;
        }

        /// <summary>
        /// 开启计时器
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="OnFinish"></param>
        void ITimer.Start(float duration, Action<ITimer> OnFinish)
        {
            count = 1;
            unscaled = false;
            state = TimerState.Run;
            this.duration = duration;
            waitTime = Time.time + duration;
            this.OnFinish = () => OnFinish(this);
        }
        
        /// <summary>
        /// 计时器更新
        /// </summary>
        void ITimer.Update()
        {
            if (state == TimerState.Run)
            {
                Update(unscaled ? Time.unscaledTime : Time.time);
            }
        }

        /// <summary>
        /// 计时器回收
        /// </summary>
        void ITimer.Close()
        {
            OnFinish = null;
            unscaled = false;
            state = TimerState.Finish;
        }
    }
}