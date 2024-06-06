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
        internal static readonly Dictionary<Type, IEvent> observers = new();

        public static void Listen<T>(Action<T> action) where T : struct, IEvent
        {
            if (!GlobalManager.Instance) return;
            if (!observers.TryGetValue(typeof(T), out var observer))
            {
                observers.Add(typeof(T), observer = new Event<T>());
            }

            ((Event<T>)observer).action += action;
        }

        public static void Remove<T>(Action<T> action) where T : struct, IEvent
        {
            if (!GlobalManager.Instance) return;
            if (observers.TryGetValue(typeof(T), out var observer))
            {
                ((Event<T>)observer).action -= action;
            }
        }

        public static void Invoke<T>(T obj = default) where T : struct, IEvent
        {
            if (!GlobalManager.Instance) return;
            if (observers.TryGetValue(typeof(T), out var observer))
            {
                ((Event<T>)observer).action?.Invoke(obj);
            }
        }

        internal static void UnRegister()
        {
            observers.Clear();
        }

        private class Event<T> : IEvent where T : struct, IEvent
        {
            public Action<T> action;
        }
    }
}