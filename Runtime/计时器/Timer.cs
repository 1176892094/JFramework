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
        /// 当前循环次数
        /// </summary>
        private int count;

        /// <summary>
        /// 最大循环次数
        /// </summary>
        private int maxCount;

        /// <summary>
        /// 是否受TimeScale影响
        /// </summary>
        private bool unscaled;

        /// <summary>
        /// 是否随单位死亡而停止
        /// </summary>
        private bool followed;

        /// <summary>
        /// 持续时间
        /// </summary>
        private float keepTime;

        /// <summary>
        /// 当前时间+持续时间
        /// </summary>
        private float waitTime;

        /// <summary>
        /// 计时器的状态
        /// </summary>
        [ShowInInspector] private TimerState state;

        /// <summary>
        /// 跟随单位
        /// </summary>
        [ShowInInspector] private object target;

        /// <summary>
        /// 循环时执行的事件
        /// </summary>
        private Action<ITimer> OnLoop;

        /// <summary>
        /// 完成时执行的事件
        /// </summary>
        private Action OnFinish;
        
        /// <summary>
        /// 计时器初始化时调用
        /// </summary>
        /// <param name="keepTime">持续时间</param>
        /// <param name="OnFinish">完成时执行的事件</param>
        public void Open(float keepTime, Action OnFinish)
        {
            count = 0;
            maxCount = 1;
            unscaled = false;
            followed = false;
            state = TimerState.Run;
            this.keepTime = keepTime;
            this.OnFinish = OnFinish;
            waitTime = Time.time + keepTime;
        }

        /// <summary>
        /// 当更新时执行的方法
        /// </summary>
        /// <param name="time">传入的游戏时间</param>
        private void OnUpdate(float time)
        {
            if (waitTime < time)
            {
                count++;
                waitTime += keepTime;
                OnLoop?.Invoke(this);
                if (maxCount < 0) return;
                if (count < maxCount) return;
                OnFinish?.Invoke();
                TimerManager.Instance.Push(this);
            }
        }

        /// <summary>
        /// 根据受TimeScale影响来执行不同的计时
        /// </summary>
        public void OnUpdate()
        {
            if (state != TimerState.Run) return;
            if (followed && target == null)
            {
                TimerManager.Instance.Push(this);
                return;
            }

            OnUpdate(unscaled ? Time.unscaledTime : Time.time);
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
        /// <param name="OnLoop">循环时执行的事件</param>
        /// <returns>返回自身</returns>
        public ITimer SetLoop(int count, Action<ITimer> OnLoop)
        {
            maxCount = count;
            this.OnLoop = OnLoop;
            return this;
        }

        /// <summary>
        /// 设置计时器是否受TimeScale影响
        /// </summary>
        /// <returns>返回自身</returns>
        public ITimer Unscale()
        {
            unscaled = true;
            waitTime = Time.unscaledTime + keepTime;
            return this;
        }

        /// <summary>
        /// 设置随目标死亡而停止
        /// </summary>
        /// <param name="target">绑定传入的目标</param>
        public ITimer SetTarget(object target)
        {
            followed = true;
            this.target = target;
            return this;
        }

        /// <summary>
        /// 关闭计时器
        /// </summary>
        public void Close()
        {
            target = null;
            OnLoop = null;
            OnFinish = null;
            unscaled = false;
            followed = false;
            state = TimerState.Finish;
        }
    }
}