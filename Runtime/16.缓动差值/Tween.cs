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
    public sealed class Tween
    {
        private float fadeTime;
        private float duration;
        private float progress;
        private GameObject owner;
        private event Action<float> OnUpdate;
        private event Action OnFinish;

        public Tween Invoke(Action<float> OnUpdate)
        {
            this.OnUpdate = OnUpdate;
            return this;
        }

        public void OnComplete(Action OnFinish)
        {
            this.OnFinish += OnFinish;
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
            fadeTime = 0;
            progress = 0;
            this.owner = owner;
            this.duration = duration;
            this.OnFinish = OnFinish;
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

                if (!owner.activeInHierarchy)
                {
                    Dispose();
                    return;
                }

                fadeTime += Time.deltaTime;
                progress = Math.Clamp(fadeTime / duration, 0, 1);
                OnUpdate?.Invoke(progress);
                if (progress >= 1)
                {
                    Dispose();
                }
            }
            catch (Exception e)
            {
                Dispose();
                Debug.Log("缓动差值无法执行方法：\n" + e);
            }
        }
    }
}