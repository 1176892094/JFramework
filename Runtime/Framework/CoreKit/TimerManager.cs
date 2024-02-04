// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-02-04  18:09
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace JFramework.Core
{
    public sealed class TimerManager : Component<GlobalManager>
    {
        private LinkedListNode<Timer> next;
        private LinkedListNode<Timer> first;
        [ShowInInspector] private readonly Queue<Timer> stopList = new Queue<Timer>();
        [ShowInInspector] private readonly LinkedList<Timer> playList = new LinkedList<Timer>();

        private void Awake() => GlobalManager.OnUpdate += Update;

        private void Update()
        {
            if (playList.Count <= 0) return;
            first = playList.First;
            while (first != null)
            {
                next = first.Next;
                first.Value?.Update();
                first = next;
            }
        }

        public Timer Pop(float time)
        {
            if (!GlobalManager.Instance) return null;
            if (!stopList.TryDequeue(out var timer))
            {
                timer = new Timer();
            }

            timer.Start(time);
            playList.AddLast(timer);
            return timer;
        }

        public void Push(Timer timer)
        {
            if (!GlobalManager.Instance) return;
            if (playList.Remove(timer))
            {
                timer.Close();
                stopList.Enqueue(timer);
            }
        }

        private void OnDestroy()
        {
            next = null;
            first = null;
            playList.Clear();
            stopList.Clear();
        }
    }
}