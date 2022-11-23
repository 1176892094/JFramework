using System.Collections.Generic;

namespace JFramework
{
    public class TimerManager : Singleton<TimerManager>
    {
        private readonly Queue<Timer> timerQueue = new Queue<Timer>();
        private readonly LinkedList<Timer> timerList = new LinkedList<Timer>();

        public TimerManager() => MonoManager.Instance.AddEventListener(Update);

        private void Update()
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
}