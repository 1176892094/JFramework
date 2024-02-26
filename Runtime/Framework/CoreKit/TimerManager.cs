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
        [SerializeField, LabelText("运行计时器")] private List<Timer> timers = new List<Timer>();

        internal void OnEnable()
        {
            if (!GlobalManager.Instance) return;
            GlobalManager.OnUpdate += OnUpdate;
        }

        private void OnUpdate()
        {
            for (int i = timers.Count - 1; i >= 0; i--)
            {
                timers[i]?.Update();
            }
        }

        public Timer Pop(float time)
        {
            var timer = StreamPool.Pop<Timer>();
            timers.Add(timer);
            return timer.Pop(time);
        }

        public void Push(Timer timer)
        { ;
            if (!timers.Remove(timer)) return;
            StreamPool.Push(timer);
            timer.Push();
        }

        internal void OnDisable()
        {
            timers.Clear();
        }
    }
}