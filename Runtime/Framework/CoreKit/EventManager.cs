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

namespace JFramework.Core
{
    public static class EventManager
    {
        private static readonly Dictionary<Type, IEvent> events = new();

        public static void Listen<T>(Action<T> action) where T : struct, IEvent
        {
            if (!GlobalManager.Instance) return;
            if (!events.TryGetValue(typeof(T), out var @event))
            {
                events.Add(typeof(T), @event = new Event<T>());
            }

            ((Event<T>)@event).action += action;
        }

        public static void Remove<T>(Action<T> action) where T : struct, IEvent
        {
            if (!GlobalManager.Instance) return;
            if (events.TryGetValue(typeof(T), out var @event))
            {
                ((Event<T>)@event).action -= action;
            }
        }

        public static void Invoke<T>(T obj = default) where T : struct, IEvent
        {
            if (!GlobalManager.Instance) return;
            if (events.TryGetValue(typeof(T), out var @event))
            {
                ((Event<T>)@event).action?.Invoke(obj);
            }
        }

        internal static void UnRegister()
        {
            events.Clear();
        }

        private class Event<T> : IEvent where T : struct, IEvent
        {
            public Action<T> action;
        }
    }
}