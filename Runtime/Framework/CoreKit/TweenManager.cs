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

namespace JFramework
{
    [Serializable]
    public sealed class Tween
    {
        private float fadeTime;
        private float duration;
        private float progress;
        private GameObject owner;
        private event Action<float> OnUpdate;
        private event Action OnDispose;
        private event Action OnFinish;

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
            owner = null;
            OnFinish = null;
            OnUpdate = null;
            OnDispose?.Invoke();
            OnDispose = null;
        }

        internal void Start(GameObject owner, float duration, Action OnDispose)
        {
            fadeTime = 0;
            progress = 0;
            this.owner = owner;
            this.duration = duration;
            this.OnDispose = OnDispose;
        }

        internal void LateUpdate()
        {
            try
            {
                if (owner == null)
                {
                    Dispose();
                    return;
                }

                fadeTime += Time.deltaTime;
                progress = Math.Clamp(fadeTime / duration, 0, 1);
                OnUpdate?.Invoke(progress);
                if (progress >= 1)
                {
                    OnFinish?.Invoke();
                    Dispose();
                }
            }
            catch (Exception e)
            {
                Dispose();
                Debug.Log("差值动画无法执行方法：\n" + e);
            }
        }
    }
}