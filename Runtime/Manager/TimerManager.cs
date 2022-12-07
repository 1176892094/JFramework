using System;
using System.Collections.Generic;
using UnityEngine;

namespace JFramework
{
    public class TimerManager: Singleton<TimerManager>
    {
        private readonly Queue<Timer> timerQueue = new Queue<Timer>();
        private readonly LinkedList<Timer> timerList = new LinkedList<Timer>();

        public TimerManager() => MonoManager.Instance.Listen(OnUpdate);

        private void OnUpdate()
        {
            for (LinkedListNode<Timer> cur = timerList.First; cur != null; cur = cur.Next)
            {
                cur.Value.Update();
            }
        }

        public void AddTimer(Timer timer) => timerList.AddLast(timer);

        public void RemoveTimer(Timer timer)
        {
            timerList.Remove(timer);
            timerQueue.Enqueue(timer);
        }

        public Timer GetTimer()
        {
            return timerQueue.Count > 0 ? timerQueue.Dequeue() : new Timer();
        }

        public void Clear() => timerList.Clear();
    }
    
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
            TimerManager.Instance.AddTimer(this);
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
            TimerManager.Instance.RemoveTimer(this);
            isRunning = false;
            OnFinished = null;
        }
    }
}