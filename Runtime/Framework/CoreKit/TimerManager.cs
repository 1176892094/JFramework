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
using JFramework.Core;
using UnityEngine;

namespace JFramework.Core
{
    internal static class TimerManager
    {
        private static readonly Dictionary<int, List<Timer>> timers = new();
        private static readonly List<int> copies = new();

        internal static void Register()
        {
            GlobalManager.OnFixedUpdate += OnFixedUpdate;
        }

        private static void OnFixedUpdate()
        {
            copies.Clear();
            copies.AddRange(timers.Keys.ToList());
            foreach (var id in copies)
            {
                if (timers.TryGetValue(id, out var runs))
                {
                    for (int i = runs.Count - 1; i >= 0; i--)
                    {
                        if (!runs[i].FixedUpdate())
                        {
                            runs.RemoveAt(i);
                            if (runs.Count == 0)
                            {
                                timers.Remove(id);
                            }
                        }
                    }
                }
            }
        }

        public static Timer Pop(GameObject entity, float waitTime)
        {
            if (!GlobalManager.Instance) return null;
            var id = entity.GetInstanceID();
            if (!timers.TryGetValue(id, out var runs))
            {
                runs = new List<Timer>();
                timers.Add(id, runs);
            }

            var timer = PoolManager.Dequeue<Timer>();
            timer.Start(entity, waitTime);
            runs.Add(timer);
            return timer;
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
        private float interval;
        private float duration;
        [SerializeField] private GameObject owner;
        private event Action OnUpdate;
        private float seconds => unscale ? Time.fixedUnscaledTime : Time.fixedTime;

        public Timer Invoke(Action OnUpdate)
        {
            this.OnUpdate = OnUpdate;
            return this;
        }

        public Timer Set(float duration)
        {
            this.duration = duration;
            interval = seconds + duration;
            return this;
        }

        public Timer Add(float interval)
        {
            this.interval += interval;
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
            interval = seconds + duration;
            return this;
        }

        public void Dispose()
        {
            owner = null;
            PoolManager.Enqueue(this);
        }

        internal void Start(GameObject owner, float duration)
        {
            count = 1;
            unscale = false;
            this.owner = owner;
            this.duration = duration;
            interval = seconds + duration;
        }

        internal bool FixedUpdate()
        {
            if (owner == null)
            {
                Dispose();
                return false;
            }

            if (seconds <= interval)
            {
                return true;
            }

            interval = seconds + duration;
            try
            {
                count--;
                OnUpdate?.Invoke();
                if (count != 0)
                {
                    return true;
                }

                Dispose();
                return false;
            }
            catch (Exception e)
            {
                Dispose();
                Debug.Log("计时器无法执行方法：\n" + e);
                return false;
            }
        }
    }
}