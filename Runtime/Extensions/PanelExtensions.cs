// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-12-21  21:56
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using JFramework.Interface;
using UnityEngine;
using UnityEngine.EventSystems;
using EventEntry = UnityEngine.EventSystems.EventTrigger.Entry;
using Object = UnityEngine.Object;

namespace JFramework
{
    public static partial class Extensions
    {
        /// <summary>
        /// 手动注册到UI管理器
        /// </summary>
        /// <param name="panel"></param>
        /// <typeparam name="T"></typeparam>
        public static void Register<T>(this T panel) where T : Component, IPanel
        {
            GlobalManager.UI.panels.Add(typeof(T), panel);
        }

        /// <summary>
        /// 手动注册到UI管理器
        /// </summary>
        /// <param name="panel"></param>
        /// <typeparam name="T"></typeparam>
        public static void UnRegister<T>(this T panel) where T : Component, IPanel
        {
            Object.Destroy(panel.gameObject);
            GlobalManager.UI.panels.Remove(typeof(T));
        }

        /// <summary>
        /// 更新格子对象
        /// </summary>
        /// <param name="grids"></param>
        /// <param name="items"></param>
        /// <typeparam name="TGird"></typeparam>
        /// <typeparam name="TItem"></typeparam>
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

        /// <summary>
        /// 添加UI事件数据
        /// </summary>
        /// <param name="target"></param>
        /// <param name="type">事件触发类型</param>
        /// <param name="action">事件触发后的回调</param>
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