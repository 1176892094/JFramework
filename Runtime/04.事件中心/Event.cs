// *********************************************************************************
// # Project: Forest
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-08-25  03:08
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Runtime.CompilerServices;
using JFramework.Interface;

namespace JFramework.Core
{
    public static partial class EventManager
    {
        private class Event<T> : IEvent where T : struct, IEvent
        {
            private event Action<T> Execute;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Listen(IEvent<T> obj) => Execute += obj.Execute;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Remove(IEvent<T> obj) => Execute -= obj.Execute;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Invoke(T obj) => Execute?.Invoke(obj);
        }
    }
}