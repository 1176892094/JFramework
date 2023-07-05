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
    internal class Timer : ITimer
    {
        /// <summary>
        /// 剩余循环次数
        /// </summary>
        private int count;

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
        /// 计时器的所有者
        /// </summary>
        [ShowInInspector]
        private object target => OnFinish?.Target;

        /// <summary>
        /// 计时器的状态
        /// </summary>
        [ShowInInspector] private TimerState state;

        /// <summary>
        /// 完成时执行的事件
        /// </summary>
        private Action OnFinish;

        /// <summary>
        /// 计时器初始化时调用
        /// </summary>
        /// <param name="duration">间隙</param>
        /// <param name="OnFinish">完成时执行的事件</param>
        public void Open(float duration, Action OnFinish)
        {
            count = 1;
            unscaled = false;
            state = TimerState.Run;
            this.duration = duration;
            this.OnFinish = OnFinish;
            waitTime = Time.time + duration;
        }

        /// <summary>
        /// 计时器初始化时调用
        /// </summary>
        /// <param name="duration">持续时间</param>
        /// <param name="OnFinish">完成时执行的事件</param>
        public void Open(float duration, Action<ITimer> OnFinish)
        {
            count = 1;
            unscaled = false;
            state = TimerState.Run;
            this.duration = duration;
            this.OnFinish = () => OnFinish(this);
            waitTime = Time.time + duration;
        }

        /// <summary>
        /// 当更新时执行的方法
        /// </summary>
        /// <param name="currTime">传入的游戏时间</param>
        private void OnUpdate(float currTime)
        {
            if (currTime > waitTime)
            {
                try
                {
                    count--;
                    OnFinish?.Invoke();
                    waitTime += duration;
                    if (count == 0)
                    {
                        Push();
                    }
                }
                catch (Exception e)
                {
                    Log.Info(DebugOption.Timer, $"=> 计时器抛出异常，自动回收计时器。\n{e}");
                    Push();
                }
            }
        }

        /// <summary>
        /// 根据受TimeScale影响来执行不同的计时
        /// </summary>
        public void OnUpdate()
        {
            if (state != TimerState.Run) return;
            OnUpdate(unscaled ? Time.unscaledTime : Time.time);
        }

        /// <summary>
        /// 剩余循环次数
        /// </summary>
        int ITimer.Count => count;

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
        public ITimer Loop(int count)
        {
            this.count = count;
            return this;
        }

        /// <summary>
        /// 计时器推入
        /// </summary>
        public void Push() => TimerManager.Push(this);

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

        /// <summary>
        /// 关闭计时器
        /// </summary>
        public void Close()
        {
            OnFinish = null;
            unscaled = false;
            state = TimerState.Finish;
        }
    }
}