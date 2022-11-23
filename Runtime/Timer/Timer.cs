using System;
using UnityEngine;

namespace JFramework
{
    public class Timer
    {
        private bool isStop;
        private bool isRunning;
        private int curLoop;
        private int maxLoop;
        private float curTime;
        private float stopTimer;
        private float interval;
        private Action OnStart;
        private Action OnUpdate;
        private Action OnClose;

        public void Register(float interval, Action OnStart = null, Action OnUpdate = null, Action OnClose = null)
        {
            maxLoop = 1;
            this.interval = interval;
            this.OnStart = OnStart;
            this.OnUpdate = OnUpdate;
            this.OnClose = OnClose;
        }

        public void Register(float interval, int maxLoop, Action OnStart = null, Action OnUpdate = null, Action OnClose = null)
        {
            this.maxLoop = maxLoop;
            this.interval = interval;
            this.OnStart = OnStart;
            this.OnUpdate = OnUpdate;
            this.OnClose = OnClose;
        }

        public void Start()
        {
            OnStart.Invoke();
            TimerManager.Instance.AddTimer(this);
            curTime = Time.time;
            isRunning = true;
            curLoop = 0;
        }

        public void Update()
        {
            if (!isRunning || Time.time <= curTime) return;
            curTime += interval;
            curLoop++;
            OnUpdate.Invoke();
            if (curLoop <= maxLoop - 1) return;
            Close();
        }

        public void Stop()
        {
            if (isStop) return;
            isStop = true;
            isRunning = false;
            stopTimer = Time.time;
        }

        public void Play()
        {
            if (!isStop) return;
            isStop = false;
            isRunning = true;
            curTime += Time.time - stopTimer;
        }

        public void Close()
        {
            isRunning = false;
            TimerManager.Instance.RemoveTimer(this);
            OnClose.Invoke();
        }
    }
}