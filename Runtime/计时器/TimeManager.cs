using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace JFramework.Core
{
    /// <summary>
    /// 计时器管理器
    /// </summary>
    public class TimeManager : Singleton<TimeManager>
    {
        /// <summary>
        /// 存储已经完成的计时器
        /// </summary>
        [ShowInInspector, ReadOnly, LabelText("完成计时队列"), FoldoutGroup("计时管理视图")]
        private Queue<TimeTick> queueList;

        /// <summary>
        /// 存储正在执行的计时器
        /// </summary>
        [ShowInInspector, ReadOnly, LabelText("正在计时列表"), FoldoutGroup("计时管理视图")]
        private List<TimeTick> timerList;

        /// <summary>
        /// 构造函数初始化数据
        /// </summary>
        protected override void OnInit(params object[] args)
        {
            queueList = new Queue<TimeTick>();
            timerList = new List<TimeTick>();
        }

        /// <summary>
        /// 计时器管理器进行计时器更新
        /// </summary>
        protected override void OnUpdate()
        {
            for (int i = timerList.Count - 1; i >= 0; i--)
            {
                timerList[i].OnUpdate();
            }
        }

        /// <summary>
        /// 计时器管理器侦听计时器
        /// </summary>
        /// <param name="time">持续时间</param>
        /// <param name="action">完成回调</param>
        public TimeTick Listen(float time, Action action = null)
        {
            if (queueList.Count > 0)
            {
                var tick = queueList.Dequeue();
                tick.Open(time, action);
                timerList.Add(tick);
                return tick;
            }
            else
            {
                var tick = new TimeTick();
                tick.Open(time, action);
                timerList.Add(tick);
                return tick;
            }
        }

        /// <summary>
        /// 计时器管理器移除计时器
        /// </summary>
        /// <param name="timer">传入需要移除的计时器</param>
        public void Remove(TimeTick timer)
        {
            if (timerList.Contains(timer))
            {
                timer.Close();
                timerList?.Remove(timer);
                queueList?.Enqueue(timer);
            }
        }

        /// <summary>
        /// 计时器管理器清除计时器
        /// </summary>
        public void Clear()
        {
            foreach (var timer in timerList)
            {
                timer.Close();
                timerList.Remove(timer);
            }

            queueList.Clear();
        }
    }
}