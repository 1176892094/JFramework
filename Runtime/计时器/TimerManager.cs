using System;
using System.Collections.Generic;
using JFramework.Interface;
using UnityEngine;

namespace JFramework.Core
{
    /// <summary>
    /// 计时器管理器
    /// </summary>
    public sealed class TimerManager : Singleton<TimerManager>
    {
        /// <summary>
        /// 存储已经完成的计时器
        /// </summary>
        internal Queue<Timer> timerQueue;

        /// <summary>
        /// 存储正在执行的计时器
        /// </summary>
        internal List<Timer> timerList;

        /// <summary>
        /// 构造函数初始化数据
        /// </summary>
        internal override void Awake()
        {
            timerQueue = new Queue<Timer>();
            timerList = new List<Timer>();
            if (GlobalManager.Instance == null) return;
            GlobalManager.Instance.UpdateAction += OnUpdate;
        }

        /// <summary>
        /// 计时器管理器进行计时器更新
        /// </summary>
        private void OnUpdate()
        {
            if (timerList == null) return;
            for (int i = timerList.Count - 1; i >= 0; i--)
            {
                timerList[i]?.OnUpdate();
            }
        }

        /// <summary>
        /// 计时器管理器侦听计时器
        /// </summary>
        /// <param name="time">持续时间</param>
        /// <param name="action">完成回调</param>
        public ITimer Pop(float time, Action action = null)
        {
            if (timerList == null)
            {
                Debug.Log("计时器管理器没有初始化!");
                return null;
            }

            Timer tick = timerQueue.Count > 0 ? timerQueue.Dequeue() : new Timer();
            tick.Open(time, action);
            timerList.Add(tick);
            return tick;
        }

        /// <summary>
        /// 计时器管理器移除计时器
        /// </summary>
        /// <param name="timer">传入需要移除的计时器</param>
        public void Push(ITimer timer)
        {
            if (timerList == null)
            {
                Debug.Log("计时器管理器没有初始化!");
                return;
            }

            var tick = (Timer)timer;
            if (timerList.Contains(tick))
            {
                tick.Close();
                timerList?.Remove(tick);
                timerQueue?.Enqueue(tick);
            }
        }

        /// <summary>
        /// 计时器管理器清除计时器
        /// </summary>
        internal override void Destroy()
        {
            base.Destroy();
            timerQueue = null;
            timerList = null;
        }
    }
}