// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-09 16:01:50
// # Recently: 2025-01-11 18:01:36
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;

namespace JFramework.Common
{
    public static partial class EventManager
    {
        [Serializable]
        private class Event<T> : IPool where T : struct, IEvent
        {
            private readonly HashSet<IEvent<T>> cached = new HashSet<IEvent<T>>();

            public Event(Type assetType)
            {
                this.assetType = assetType;
            }

            public Type assetType { get; private set; }
            public string assetPath { get; private set; }
            public int acquire => cached.Count;
            public int release { get; private set; }
            public int dequeue { get; private set; }
            public int enqueue { get; private set; }

            void IDisposable.Dispose()
            {
                cached.Clear();
                OnExecute = null;
            }

            private event Action<T> OnExecute;

            public void Listen(IEvent<T> @object)
            {
                dequeue++;
                if (cached.Add(@object))
                {
                    OnExecute += @object.Execute;
                }
            }

            public void Remove(IEvent<T> @object)
            {
                enqueue++;
                if (cached.Remove(@object))
                {
                    OnExecute -= @object.Execute;
                }
            }

            public void Invoke(T message)
            {
                release++;
                OnExecute?.Invoke(message);
            }
        }
    }
}