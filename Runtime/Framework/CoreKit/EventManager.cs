// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-02-04  18:12
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using JFramework.Interface;
using Sirenix.OdinInspector;

namespace JFramework.Core
{
    public sealed class EventManager : Component<GlobalManager>
    {
        [ShowInInspector] private readonly Dictionary<Type, IEvent> observers = new Dictionary<Type, IEvent>();

        public void Listen<T>(IEvent<T> obj) where T : struct, IEvent
        {
            if (!observers.ContainsKey(typeof(T)))
            {
                observers.Add(typeof(T), new Event<T>());
            }

            ((Event<T>)observers[typeof(T)]).Listen(obj);
        }

        public void Remove<T>(IEvent<T> obj) where T : struct, IEvent
        {
            if (observers.ContainsKey(typeof(T)))
            {
                ((Event<T>)observers[typeof(T)]).Remove(obj);
            }
        }

        public void Invoke<T>(T obj = default) where T : struct, IEvent
        {
            if (observers.ContainsKey(typeof(T)))
            {
                ((Event<T>)observers[typeof(T)]).Invoke(obj);
            }
        }

        private void OnDestroy()
        {
            observers.Clear();
        }
    }
}