// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2025-01-08 17:01:28
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using JFramework.Common;
using UnityEngine;

namespace JFramework
{
    internal static class TimerManager
    {
        public static void Update(float elapsedTime, float unscaleTime)
        {
            for (var i = GlobalManager.timerData.Count - 1; i >= 0; i--)
            {
                GlobalManager.timerData[i].Update(elapsedTime, unscaleTime);
            }
        }

        public static T Load<T>(Component entity, float duration) where T : class, ITimer
        {
            if (!GlobalManager.Instance) return default;
            var timerData = Service.Pool.Dequeue<T>();
            timerData.Start(entity, duration, OnComplete);
            GlobalManager.timerData.Add(timerData);
            return timerData;

            void OnComplete()
            {
                GlobalManager.timerData.Remove(timerData);
                timerData.Dispose();
                Service.Pool.Enqueue(timerData, typeof(T));
            }
        }

        internal static void Dispose()
        {
            GlobalManager.timerData.Clear();
        }
    }
}