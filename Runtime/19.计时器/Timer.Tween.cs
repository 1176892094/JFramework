// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2024-12-24 01:12:32
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;

namespace JFramework
{
    [Serializable]
    public sealed class Tween : ITimer
    {
        private float duration;
        private Action OnFinish;
        private Action<float> OnUpdate;

        private IEntity owner;
        private float progress;
        private float waitTime;

        public void Dispose()
        {
            owner = null;
            progress = 0;
            duration = 0;
            waitTime = 0;
            OnUpdate = null;
            OnFinish = null;
        }

        void ITimer.Start(IEntity owner, float duration, Action OnFinish)
        {
            progress = 0;
            waitTime = 0;
            this.owner = owner;
            this.duration = duration;
            this.OnFinish = OnFinish;
        }

        void IUpdate.Update(float elapsedTime, float unscaleTime)
        {
            try
            {
                if (!Service.IsEntity(owner))
                {
                    OnFinish.Invoke();
                    return;
                }

                if (!Service.IsActive(owner))
                {
                    OnFinish.Invoke();
                    return;
                }

                if (waitTime <= 0)
                {
                    waitTime = elapsedTime;
                }

                progress = (elapsedTime - waitTime) / duration;
                if (progress > 1)
                {
                    progress = 1;
                }

                OnUpdate.Invoke(progress);

                if (progress >= 1)
                {
                    OnFinish.Invoke();
                }
            }
            catch (Exception e)
            {
                OnFinish.Invoke();
                Service.Log(Service.Text.Format("缓动差值无法执行方法：\n{0}", e));
            }
        }

        public Tween Invoke(Action<float> OnUpdate)
        {
            this.OnUpdate = OnUpdate;
            return this;
        }

        public void OnComplete(Action OnFinish)
        {
            this.OnFinish += OnFinish;
        }
    }
}