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
using System.Linq;
using UnityEngine;

namespace JFramework.Core
{
    public static class TimerManager
    {
        internal static readonly List<Timer> timers = new();
        private static readonly List<Timer> copies = new();

        internal static void Register()
        {
            GlobalManager.OnFixedUpdate += OnFixedUpdate;
        }

        private static void OnFixedUpdate()
        {
            copies.Clear();
            copies.AddRange(timers);
            foreach (var timer in copies.Where(timer => !timer.Update()))
            {
                Push(timer);
            }
        }

        public static Timer Pop(float waitTime)
        {
            if (!GlobalManager.Instance) return null;
            var timer = PoolManager.Dequeue<Timer>();
            timers.Add(timer.Pop(waitTime));
            return timer;
        }

        public static void Push(Timer timer)
        {
            if (!GlobalManager.Instance) return;
            PoolManager.Enqueue(timer);
            timers.Remove(timer);
        }

        internal static void UnRegister()
        {
            copies.Clear();
            timers.Clear();
        }
    }
}

namespace JFramework
{
    [Serializable]
    public sealed class Timer
    {
        private int count;
        private bool unscale;
        [SerializeField] private float waitTime;
        [SerializeField] private float stayTime;
        private event Action OnUpdate;
        private float seconds => unscale ? Time.fixedUnscaledTime : Time.fixedTime;

        public Timer Invoke(Action OnUpdate)
        {
            this.OnUpdate = OnUpdate;
            return this;
        }

        public Timer Invoke(Action<Timer> OnUpdate)
        {
            this.OnUpdate = () => OnUpdate(this);
            return this;
        }

        public Timer Set(float waitTime)
        {
            this.waitTime = waitTime;
            stayTime = seconds + waitTime;
            return this;
        }

        public Timer Add(float stayTime)
        {
            this.stayTime += stayTime;
            return this;
        }

        public Timer Loops(int count = 0)
        {
            this.count = count;
            return this;
        }

        public Timer Unscale(bool unscale = true)
        {
            this.unscale = unscale;
            stayTime = seconds + waitTime;
            return this;
        }

        internal Timer Pop(float duration)
        {
            count = 1;
            unscale = false;
            waitTime = duration;
            stayTime = seconds + duration;
            return this;
        }

        internal bool Update()
        {
            if (seconds <= stayTime)
            {
                return true;
            }

            stayTime = seconds + waitTime;
            try
            {
                count--;
                OnUpdate?.Invoke();
                return count != 0;
            }
            catch (Exception e)
            {
                Debug.LogWarning("计时器无法执行方法：\n" + e);
                return false;
            }
        }
    }
}