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
using UnityEngine;

namespace JFramework.Core
{
    public sealed class EventManager : ScriptableObject
    {
        [ShowInInspector, LabelText("事件列表")] private Dictionary<Type, IEvent> observers = new();

        public void Listen<T>(IEvent<T> obj) where T : struct, IEvent
        {
            if (!observers.TryGetValue(typeof(T), out var observer))
            {
                observers.Add(typeof(T), observer = new Event<T>());
            }

            ((Event<T>)observer).OnExecute += obj.Execute;
        }

        public void Remove<T>(IEvent<T> obj) where T : struct, IEvent
        {
            if (observers.TryGetValue(typeof(T), out var observer))
            {
                ((Event<T>)observer).OnExecute -= obj.Execute;
            }
        }

        public void Invoke<T>(T obj = default) where T : struct, IEvent
        {
            if (observers.TryGetValue(typeof(T), out var observer))
            {
                ((Event<T>)observer).OnExecute?.Invoke(obj);
            }
        }

        internal void OnDisable()
        {
            observers.Clear();
        }

        private class Event<T> : IEvent where T : struct, IEvent
        {
            public Action<T> OnExecute;
        }
    }
}