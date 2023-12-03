// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-10-24  23:59
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

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
        public static ITimer Pop(float time)
        {
            if (!GlobalManager.Runtime) return null;
            if (!queues.TryDequeue(out var timer))
            {
                timer = new Timer();
            }
            
            timer.Start(time);
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