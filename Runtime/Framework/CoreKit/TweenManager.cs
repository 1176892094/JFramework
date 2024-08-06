// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-02-05  18:09
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
    public static class TweenManager
    {
        private static readonly Dictionary<int, List<Tween>> motions = new();
        private static readonly List<int> copies = new List<int>();

        internal static void Register()
        {
            GlobalManager.OnUpdate += OnUpdate;
        }

        private static void OnUpdate()
        {
            copies.Clear();
            copies.AddRange(motions.Keys.ToList());
            foreach (var id in copies)
            {
                if (motions.TryGetValue(id, out var runs))
                {
                    for (int i = runs.Count - 1; i >= 0; i--)
                    {
                        if (runs[i].Update())
                        {
                            runs.RemoveAt(i);
                            if (runs.Count == 0)
                            {
                                motions.Remove(id);
                            }
                        }
                    }
                }
            }
        }

        public static Tween Tween(GameObject entity, float duration)
        {
            var id = entity.GetInstanceID();
            if (!motions.TryGetValue(id, out var runs))
            {
                runs = new List<Tween>();
                motions.Add(id, runs);
            }

            var motion = PoolManager.Dequeue<Tween>();
            motion.Start(entity, duration);
            runs.Add(motion);
            return motion;
        }

        internal static void UnRegister()
        {
            copies.Clear();
            motions.Clear();
        }
    }
}

namespace JFramework
{
    [Serializable]
    public sealed class Tween
    {
        private float timer;
        private float duration;
        private event Action OnFinish;
        private event Action<float> OnUpdate;
        [SerializeField] private GameObject owner;

        public Tween Invoke(Action<float> OnUpdate)
        {
            this.OnUpdate = OnUpdate;
            return this;
        }

        public void OnComplete(Action OnFinish)
        {
            this.OnFinish = OnFinish;
        }

        internal void Start(GameObject owner, float duration)
        {
            timer = 0;
            OnFinish = null;
            this.owner = owner;
            this.duration = duration;
        }

        internal bool Update()
        {
            if (owner == null)
            {
                PoolManager.Enqueue(this);
                return true;
            }

            timer += Time.deltaTime;
            OnUpdate?.Invoke(timer);
            if (timer / duration > 1)
            {
                PoolManager.Enqueue(this);
                OnFinish?.Invoke();
                return true;
            }

            return false;
        }
    }
}