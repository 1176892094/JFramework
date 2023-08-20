using System;
using System.Collections.Generic;
using JFramework.Interface;
using UnityEngine;

namespace JFramework.Core
{
    /// <summary>
    /// 计时器管理器
    /// </summary>
    public static class TimerManager
    {
        /// <summary>
        /// 存储已经完成的计时器
        /// </summary>
        internal static readonly Queue<ITimer> queues = new Queue<ITimer>();

        /// <summary>
        /// 存储正在执行的计时器
        /// </summary>
        internal static readonly LinkedList<ITimer> timers = new LinkedList<ITimer>();

        /// <summary>
        /// 一号计时器节点
        /// </summary>
        private static LinkedListNode<ITimer> first;

        /// <summary>
        /// 下个计时器节点
        /// </summary>
        private static LinkedListNode<ITimer> next;

        /// <summary>
        /// 注册
        /// </summary>
        internal static void Register()
        {
            GlobalManager.OnUpdate += OnUpdate;
        }

        /// <summary>
        /// 计时器管理器进行计时器更新
        /// </summary>
        private static void OnUpdate()
        {
            if (timers.Count <= 0) return;
            first = timers.First;
            while (first != null)
            {
                next = first.Next;
                first.Value?.Update();
                first = next;
            }
        }

        /// <summary>
        /// 计时器管理器侦听计时器
        /// </summary>
        /// <param name="time">持续时间</param>
        /// <param name="action">完成回调</param>
        public static ITimer Pop(float time, Action action)
        {
            if (!GlobalManager.Runtime) return null;
            if (!queues.TryDequeue(out var timer))
            {
                timer = new Timer();
            }
            
            timer.Start(time, action);
            timers.AddLast(timer);
            return timer;
        }

        /// <summary>
        /// 计时器管理器侦听计时器
        /// </summary>
        /// <param name="time">持续时间</param>
        /// <param name="action">完成回调</param>
        public static ITimer Pop(float time, Action<ITimer> action)
        {
            if (!GlobalManager.Runtime) return null;
            if (!queues.TryDequeue(out var timer))
            {
                timer = new Timer();
            }

            timer.Start(time, action);
            timers.AddLast(timer);
            return timer;
        }

        /// <summary>
        /// 计时器管理器移除计时器
        /// </summary>
        /// <param name="timer">传入需要移除的计时器</param>
        public static void Push(ITimer timer)
        {
            if (!GlobalManager.Runtime) return;
            if (timers.Remove(timer))
            {
                timer.Close();
                queues.Enqueue(timer);
            }
        }

        /// <summary>
        /// 取消注册
        /// </summary>
        internal static void UnRegister()
        {
            queues.Clear();
            timers.Clear();
        }
    }
}