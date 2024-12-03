// *********************************************************************************
// # Project: Forest
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-08-25  01:08
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using UnityEngine;

namespace JFramework
{
    [Serializable]
    public sealed class Timer
    {
        private int count;
        private bool unscale;
        private float interval;
        private float duration;
        private GameObject owner;
        private event Action OnUpdate;
        private event Action OnFinish;
        private float seconds => unscale ? Time.fixedUnscaledTime : Time.fixedTime;

        public Timer Invoke(Action OnUpdate)
        {
            this.OnUpdate = OnUpdate;
            return this;
        }

        public void Finish(Action OnFinish = null)
        {
            if (OnFinish == null)
            {
                this.OnFinish += OnUpdate;
                return;
            }

            this.OnFinish += OnFinish;
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
            OnUpdate = null;
            OnFinish?.Invoke();
            OnFinish = null;
        }

        internal void Start(GameObject owner, float duration, Action OnFinish)
        {
            count = 1;
            unscale = false;
            this.owner = owner;
            this.duration = duration;
            interval = seconds + duration;
            this.OnFinish = OnFinish;
        }

        internal void FixedUpdate()
        {
            try
            {
                if (owner == null)
                {
                    Dispose();
                    return;
                }

                if (!owner.activeInHierarchy)
                {
                    Dispose();
                    return;
                }

                if (seconds <= interval)
                {
                    return;
                }

                count--;
                interval = seconds + duration;
                OnUpdate?.Invoke();
                if (count == 0)
                {
                    Dispose();
                }
            }
            catch (Exception e)
            {
                Dispose();
                Debug.Log("计时器无法执行方法：\n" + e);
            }
        }
    }
}