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

namespace JFramework
{
    public static partial class EventManager
    {
        private static readonly Dictionary<Type, IEvent> events = new();

        public static void Listen<T>(IEvent<T> obj) where T : struct, IEvent
        {
            if (GlobalManager.Instance)
            {
                if (!events.TryGetValue(typeof(T), out var @event))
                {
                    @event = new Event<T>();
                    events.Add(typeof(T), @event);
                }

                ((Event<T>)@event).Listen(obj);
            }
        }

        public static void Remove<T>(IEvent<T> obj) where T : struct, IEvent
        {
            if (GlobalManager.Instance)
            {
                if (events.TryGetValue(typeof(T), out var @event))
                {
                    ((Event<T>)@event).Remove(obj);
                }
            }
        }

        public static void Invoke<T>(T obj) where T : struct, IEvent
        {
            if (GlobalManager.Instance)
            {
                if (events.TryGetValue(typeof(T), out var @event))
                {
                    ((Event<T>)@event).Invoke(obj);
                }
            }
        }
        
        public static void Invoke<T>() where T : struct, IEvent
        {
            if (GlobalManager.Instance)
            {
                if (events.TryGetValue(typeof(T), out var @event))
                {
                    ((Event<T>)@event).Invoke();
                }
            }
        }

        internal static void UnRegister()
        {
            events.Clear();
        }
    }
}