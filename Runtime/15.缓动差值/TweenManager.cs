// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-02-05  18:09
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JFramework
{
    public static class TweenManager
    {
        private static readonly Dictionary<int, List<Tween>> motions = new();
        private static readonly List<int> copies = new();

        internal static void Register()
        {
            GlobalManager.OnLateUpdate += OnLateUpdate;
        }

        private static void OnLateUpdate()
        {
            copies.Clear();
            copies.AddRange(motions.Keys.ToList());
            foreach (var id in copies)
            {
                if (motions.TryGetValue(id, out var runs))
                {
                    for (int i = runs.Count - 1; i >= 0; i--)
                    {
                        runs[i].LateUpdate();
                    }
                }
            }
        }

        public static Tween Tween(GameObject entity, float duration)
        {
            if (!GlobalManager.Instance) return null;
            var id = entity.GetInstanceID();
            if (!motions.TryGetValue(id, out var runs))
            {
                runs = new List<Tween>();
                motions.Add(id, runs);
            }

            var tween = PoolManager.Dequeue<Tween>();
            tween.Start(entity, duration, Dispose);
            runs.Add(tween);
            return tween;

            void Dispose()
            {
                runs.Remove(tween);
                if (runs.Count == 0)
                {
                    motions.Remove(id);
                }

                PoolManager.Enqueue(tween);
            }
        }

        internal static void UnRegister()
        {
            copies.Clear();
            motions.Clear();
        }
    }
}