// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-02-04  15:02
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using JFramework.Interface;
using UnityEngine;
using UnityEngine.EventSystems;
using EventEntry = UnityEngine.EventSystems.EventTrigger.Entry;

// ReSharper disable All

namespace JFramework
{
    public static partial class Extensions
    {
        public static TaskAwaiter<T> GetAwaiter<T>(this T request) where T : AsyncOperation
        {
            var completion = new TaskCompletionSource<T>();
            request.completed += operation => completion.SetResult(operation as T);
            return completion.Task.GetAwaiter();
        }

        public static void Listen(this IUpdate entity)
        {
            if (!GlobalManager.Instance) return;
            GlobalManager.OnUpdate += entity.OnUpdate;
        }

        public static void Remove(this IUpdate entity)
        {
            if (!GlobalManager.Instance) return;
            GlobalManager.OnUpdate -= entity.OnUpdate;
        }

        public static T FindComponent<T>(this IEntity entity) where T : ScriptableObject, IComponent
        {
            return (T)GlobalManager.Entity.FindComponent(entity, typeof(T));
        }

        public static void Destroy(this IEntity entity)
        {
            if (!GlobalManager.Instance) return;
            GlobalManager.Entity.Destroy(entity);
        }
        
        public static void Update<TGird, TItem>(this TGird[] grids, List<TItem> items) where TGird : IGrid<TItem>
        {
            for (int i = 0; i < grids.Length; i++)
            {
                if (grids[i] != null && i < items.Count && items[i] != null)
                {
                    grids[i].SetItem(items[i]);
                }
                else
                {
                    grids[i].Dispose();
                }
            }
        }

        public static void AddListener(this UIBehaviour target, EventTriggerType type, Action<PointerEventData> action)
        {
            var trigger = target.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = target.gameObject.AddComponent<EventTrigger>();
            }

            var entry = new EventEntry
            {
                eventID = type
            };
            entry.callback.AddListener(eventData => action?.Invoke((PointerEventData)eventData));
            trigger.triggers.Add(entry);
        }
    }
}