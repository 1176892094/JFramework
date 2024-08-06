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
using UnityEngine;

namespace JFramework.Core
{
    public static class TweenManager
    {
        private static readonly Dictionary<int, List<Tween>> motions = new();
        private static readonly List<int> copies = new();

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
                        runs[i].Update();
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
                tween.owner = null;
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

namespace JFramework
{
    [Serializable]
    public sealed class Tween
    {
        private float timer;
        private float duration;
        public GameObject owner;
        private event Action OnFinish;
        private event Action<float> OnUpdate;
        private event Action OnDispose;
        private float progress => timer / duration;

        public Tween Invoke(Action<float> OnUpdate)
        {
            this.OnUpdate = OnUpdate;
            return this;
        }

        public void OnComplete(Action OnFinish)
        {
            this.OnFinish = OnFinish;
        }

        public void Dispose()
        {
            OnDispose?.Invoke();
        }

        internal void Start(GameObject owner, float duration, Action OnDispose)
        {
            timer = 0;
            OnFinish = null;
            this.owner = owner;
            this.duration = duration;
            this.OnDispose = OnDispose;
        }

        internal void Update()
        {
            if (owner == null)
            {
                Dispose();
                return;
            }

            try
            {
                timer += Time.deltaTime;
                OnUpdate?.Invoke(progress);
                if (progress < 1)
                {
                    return;
                }

                OnFinish?.Invoke();
                Dispose();
            }
            catch (Exception e)
            {
                Dispose();
                Debug.Log("线性动画无法执行方法：\n" + e);
            }
        }
    }
}