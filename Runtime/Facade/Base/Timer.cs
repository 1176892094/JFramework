using System;
using UnityEngine;

namespace JFramework.Basic
{
    public class Timer
    {
        private int curLoop;
        private int maxLoop;
        private bool isStop;
        private bool isRunning;
        private float curTime;
        private float stopTime;
        private float delayTime;
        private Action OnFinished;

        public void Open(float delayTime, Action OnFinished = null)
        {
            maxLoop = 1;
            this.delayTime = delayTime;
            this.OnFinished = OnFinished;
            Start();
        }

        public void Open(float delayTime, int maxLoop, Action OnFinished = null)
        {
            this.maxLoop = maxLoop;
            this.delayTime = delayTime;
            this.OnFinished = OnFinished;
            Start();
        }

        private void Start()
        {
            curLoop = -1;
            isRunning = true;
            curTime = Time.time;
            TimerManager.Instance.Add(this);
        }

        public void Update()
        {
            if (!isRunning || Time.time <= curTime) return;
            curTime += delayTime;
            curLoop++;
            if (curLoop == 0) return;
            OnFinished.Invoke();
            if (curLoop <= maxLoop) return;
            Close();
        }

        public void Stop()
        {
            if (isStop) return;
            isStop = true;
            isRunning = false;
            stopTime = Time.time;
        }

        public void Play()
        {
            if (!isStop) return;
            isStop = false;
            isRunning = true;
            curTime += Time.time - stopTime;
        }

        public void Close()
        {
            TimerManager.Instance.Remove(this);
            isRunning = false;
            OnFinished = null;
        }
    }
}