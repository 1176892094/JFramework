using System.Collections.Generic;
using JFramework.Basic;

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

        public void Add(Timer timer) => timerList.AddLast(timer);

        public void Remove(Timer timer)
        {
            timerList.Remove(timer);
            timerQueue.Enqueue(timer);
        }

        public Timer Get()
        {
            return timerQueue.Count > 0 ? timerQueue.Dequeue() : new Timer();
        }

        public void Clear() => timerList.Clear();
    }
}