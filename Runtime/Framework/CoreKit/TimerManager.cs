// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-02-04  18:09
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JFramework.Core
{
    public sealed class TimerManager : ScriptableObject
    {
        [ShowInInspector, LabelText("运行计时器")] private readonly LinkedList<Timer> timers = new LinkedList<Timer>();
        private LinkedListNode<Timer> next;
        private LinkedListNode<Timer> first;

        internal void OnEnable()
        {
            if (!GlobalManager.Instance) return;
            GlobalManager.OnUpdate += OnUpdate;
        }

        private void OnUpdate()
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

        public Timer Pop(float time)
        {
            if (!GlobalManager.Instance) return null;
            var timer = StreamPool.Pop<Timer>();
            timer.Start(time);
            timers.AddLast(timer);
            return timer;
        }

        public void Push(Timer timer)
        {
            if (!GlobalManager.Instance) return;
            if (timers.Remove(timer))
            {
                timer.Close();
                StreamPool.Push(timer);
            }
        }

        internal void OnDisable()
        {
            next = null;
            first = null;
            timers.Clear();
        }
    }
}