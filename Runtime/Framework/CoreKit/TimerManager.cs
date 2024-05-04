// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-02-04  18:09
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using JFramework.Core;
using UnityEngine;

namespace JFramework.Core
{
    public static class TimerManager
    {
        internal static readonly List<Timer> timers = new();

        internal static void Register()
        {
            GlobalManager.OnUpdate += OnUpdate;
        }

        private static void OnUpdate()
        {
            for (int i = timers.Count - 1; i >= 0; i--)
            {
                timers[i]?.Update();
            }
        }

        public static Timer Pop(float interval)
        {
            if (!GlobalManager.Instance) return null;
            var timer = StreamPool.Pop<Timer>();
            timers.Add(timer);
            return timer.Pop(interval);
        }

        public static void Push(Timer timer)
        {
            if (!GlobalManager.Instance) return;
            if (!timers.Remove(timer)) return;
            StreamPool.Push(timer);
            timer.Push();
        }

        internal static void UnRegister()
        {
            timers.Clear();
        }
    }
}

namespace JFramework
{
    [Serializable]
    public sealed class Timer
    {
        private bool running;
        private bool unscale;
        [SerializeField] private int count;
        [SerializeField] private float interval;
        [SerializeField] private float duration;
        private event Action callback;
        private event Func<bool> condition;
        private float seconds => unscale ? Time.unscaledTime : Time.time;

        public Timer Invoke(Action callback)
        {
            this.callback = callback;
            return this;
        }

        public Timer Invoke(Action<Timer> callback)
        {
            this.callback = () => callback(this);
            return this;
        }

        public Timer When(Func<bool> condition)
        {
            this.condition = condition;
            return this;
        }

        public Timer When(Func<Timer, bool> condition)
        {
            this.condition = () => condition(this);
            return this;
        }

        public Timer Loops(int count = 0)
        {
            this.count = count;
            return this;
        }

        public Timer Unscale()
        {
            unscale = true;
            duration = seconds + interval;
            return this;
        }

        public Timer Set(float interval)
        {
            this.interval = interval;
            duration = seconds + interval;
            return this;
        }

        public Timer Add(float duration)
        {
            this.duration += duration;
            return this;
        }

        internal Timer Pop(float interval)
        {
            count = 1;
            running = true;
            unscale = false;
            this.interval = interval;
            duration = seconds + interval;
            return this;
        }

        internal void Update()
        {
            if (!running || seconds <= duration)
            {
                return;
            }

            duration = seconds + interval;
            try
            {
                if (condition != null)
                {
                    if (condition())
                    {
                        callback?.Invoke();
                    }
                    else
                    {
                        TimerManager.Push(this);
                    }
                }
                else
                {
                    count--;
                    callback?.Invoke();
                    if (count == 0)
                    {
                        TimerManager.Push(this);
                    }
                }
            }
            catch (Exception e)
            {
                TimerManager.Push(this);
                Debug.LogWarning("计时器无法执行方法：\n" + e);
            }
        }

        internal void Push()
        {
            running = false;
            unscale = false;
            callback = null;
            condition = null;
        }
    }
}