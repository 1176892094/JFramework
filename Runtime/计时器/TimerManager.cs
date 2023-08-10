using System;
using System.Collections.Generic;
using JFramework.Interface;

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
        internal static readonly HashSet<Timer> finishList = new HashSet<Timer>();

        /// <summary>
        /// 存储正在执行的计时器
        /// </summary>
        internal static readonly LinkedList<Timer> timerList = new LinkedList<Timer>();

        /// <summary>
        /// 当前计时器节点
        /// </summary>
        private static LinkedListNode<Timer> currentNode;

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
            if (timerList.Count <= 0) return;
            currentNode = timerList.First;
            while (currentNode != null)
            {
                var nextNode = currentNode.Next;
                currentNode.Value?.OnUpdate();
                currentNode = nextNode;
            }
        }

        /// <summary>
        /// 计时器管理器侦听计时器
        /// </summary>
        /// <param name="time">持续时间</param>
        /// <param name="action">完成回调</param>
        public static ITimer Pop(float time, Action action = null)
        {
            if (!GlobalManager.Runtime) return null;
            if (!finishList.TryPop(out var timer))
            {
                timer = new Timer();
            }

            timer.Open(time, action);
            timerList.AddLast(timer);
            return timer;
        }

        /// <summary>
        /// 计时器管理器侦听计时器
        /// </summary>
        /// <param name="time">持续时间</param>
        /// <param name="action">完成回调</param>
        public static ITimer Pop(float time, Action<ITimer> action = null)
        {
            if (!GlobalManager.Runtime) return null;
            if (!finishList.TryPop(out var timer))
            {
                timer = new Timer();
            }

            timer.Open(time, action);
            timerList.AddLast(timer);
            return timer;
        }

        /// <summary>
        /// 计时器管理器移除计时器
        /// </summary>
        /// <param name="timer">传入需要移除的计时器</param>
        public static void Push(ITimer timer)
        {
            if (!GlobalManager.Runtime) return;
            var tick = (Timer)timer;
            if (timerList.Remove(tick))
            {
                tick?.Close();
                finishList.Add(tick);
            }
        }

        /// <summary>
        /// 取消注册
        /// </summary>
        internal static void UnRegister()
        {
            timerList.Clear();
            finishList.Clear();
        }
    }
}