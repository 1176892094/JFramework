// *********************************************************************************
// # Project: Forest
// # Unity: 2022.3.5f1c1
// # Author: jinyijie
// # Version: 1.0.0
// # History: 2024-09-09  03:09
// # Copyright: 2024, jinyijie
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace JFramework
{
    public static class ThreadDispatcher
    {
        private static SynchronizationContext Current;

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
        private static void Editor()
        {
            Current = SynchronizationContext.Current;
        }
#endif

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Runtime()
        {
            Current = SynchronizationContext.Current;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Invoke(Action action)
        {
            if (Current != SynchronizationContext.Current)
            {
                Current.Post(Send, null);

                void Send(object state)
                {
                    action.Invoke();
                }
            }
            else
            {
                action.Invoke();
            }
        }
    }
}