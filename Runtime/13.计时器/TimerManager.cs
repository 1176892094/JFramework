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
using System.Linq;
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
                        runs[i].FixedUpdate();
                    }
                }
            }
        }

        public static Timer Pop(GameObject entity, float duration)
        {
            if (!GlobalManager.Instance) return null;
            var id = entity.GetInstanceID();
            if (!timers.TryGetValue(id, out var runs))
            {
                runs = new List<Timer>();
                timers.Add(id, runs);
            }

            var timer = PoolManager.Dequeue<Timer>();
            timer.Start(entity, duration, Dispose);
            runs.Add(timer);
            return timer;

            void Dispose()
            {
                runs.Remove(timer);
                if (runs.Count == 0)
                {
                    timers.Remove(id);
                }

                PoolManager.Enqueue(timer);
            }
        }

        internal static void UnRegister()
        {
            copies.Clear();
            timers.Clear();
        }
    }
}