using System;
using JFramework.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JFramework
{
    /// <summary>
    /// 计时器状态
    /// </summary>
    internal enum TimeState
    {
        /// <summary>
        /// 运行状态
        /// </summary>
        Run = 0,

        /// <summary>
        /// 暂停状态
        /// </summary>
        Stop = 1,

        /// <summary>
        /// 完成状态
        /// </summary>
        Finish = 2,
    }

    /// <summary>
    /// 计时器
    /// </summary>
    public class TimeTick
    {
        /// <summary>
        /// 当前循环次数
        /// </summary>
        [ShowInInspector] private int count;

        /// <summary>
        /// 最大循环次数
        /// </summary>
        private int maxCount;

        /// <summary>
        /// 是否受TimeScale影响
        /// </summary>
        [ShowInInspector] private bool unscaled;

        /// <summary>
        /// 是否随单位死亡而停止
        /// </summary>
        [ShowInInspector] private bool followed;

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
        [ShowInInspector] private TimeState state;

        /// <summary>
        /// 跟随单位
        /// </summary>
        [ShowInInspector] private GameObject target;

        /// <summary>
        /// 循环时执行的事件
        /// </summary>
        private Action OnLoop;

        /// <summary>
        /// 完成时执行的事件
        /// </summary>
        private Action OnFinish;

        /// <summary>
        /// 计时器初始化时调用
        /// </summary>
        /// <param name="keepTime">持续时间</param>
        /// <param name="OnFinish">完成时执行的事件</param>
        internal void Open(float keepTime, Action OnFinish)
        {
            count = 0;
            maxCount = 1;
            unscaled = false;
            followed = false;
            state = TimeState.Run;
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
                OnLoop?.Invoke();
                if (maxCount < 0) return;
                if (count < maxCount) return;
                OnFinish?.Invoke();
                TimeManager.Instance.Remove(this);
            }
        }

        /// <summary>
        /// 根据受TimeScale影响来执行不同的计时
        /// </summary>
        internal void OnUpdate()
        {
            if (state != TimeState.Run) return;
            if (followed && target == null)
            {
                TimeManager.Instance.Remove(this);
                return;
            }

            OnUpdate(unscaled ? Time.unscaledTime : Time.time);
        }

        /// <summary>
        /// 计时器开始计时
        /// </summary>
        /// <returns>返回自身</returns>
        public TimeTick Play()
        {
            state = TimeState.Run;
            return this;
        }

        /// <summary>
        /// 计时器暂停计时
        /// </summary>
        /// <returns>返回自身</returns>
        public TimeTick Stop()
        {
            state = TimeState.Stop;
            return this;
        }

        /// <summary>
        /// 设置计时器的循环
        /// </summary>
        /// <param name="count">计时器循环次数</param>
        /// <param name="OnLoop">循环时执行的事件</param>
        /// <returns>返回自身</returns>
        public TimeTick SetLoop(int count, Action OnLoop)
        {
            maxCount = count;
            this.OnLoop = OnLoop;
            return this;
        }

        /// <summary>
        /// 设置计时器是否受TimeScale影响
        /// </summary>
        /// <returns>返回自身</returns>
        public TimeTick Unscale()
        {
            unscaled = true;
            waitTime = Time.unscaledTime + keepTime;
            return this;
        }

        /// <summary>
        /// 设置随目标死亡而停止
        /// </summary>
        /// <param name="target">绑定传入的目标</param>
        public void SetTarget(GameObject target)
        {
            followed = true;
            this.target = target;
        }

        /// <summary>
        /// 关闭计时器
        /// </summary>
        internal void Close()
        {
            target = null;
            OnLoop = null;
            OnFinish = null;
            unscaled = false;
            followed = false;
            state = TimeState.Finish;
        }
    }
}