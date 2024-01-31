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
using Sirenix.OdinInspector;

namespace JFramework.Core
{
    /// <summary>
    /// 计时器管理器
    /// </summary>
    public sealed class TimerManager : Controller<GlobalManager>
    {
        /// <summary>
        /// 存储已经完成的计时器
        /// </summary>
        [ShowInInspector, LabelText("已完成")] private readonly Queue<Timer> queues = new Queue<Timer>();

        /// <summary>
        /// 存储正在执行的计时器
        /// </summary>
        [ShowInInspector, LabelText("运行中")] private readonly LinkedList<Timer> timers = new LinkedList<Timer>();

        /// <summary>
        /// 下个计时器节点
        /// </summary>
        private LinkedListNode<Timer> next;

        /// <summary>
        /// 一号计时器节点
        /// </summary>
        private LinkedListNode<Timer> first;

        /// <summary>
        /// 管理器醒来
        /// </summary>
        private void Awake()
        {
            GlobalManager.OnUpdate += Update;
        }

        /// <summary>
        /// 计时器管理器进行计时器更新
        /// </summary>
        private void Update()
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
        public Timer Pop(float time)
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
        public void Push(Timer timer)
        {
            if (!GlobalManager.Runtime) return;
            if (timers.Remove(timer))
            {
                timer.Close();
                queues.Enqueue(timer);
            }
        }

        /// <summary>
        /// 销毁管理器
        /// </summary>
        private void OnDestroy()
        {
            next = null;
            first = null;
            queues.Clear();
            timers.Clear();
        }
    }
}