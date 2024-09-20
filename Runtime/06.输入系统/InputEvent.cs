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

namespace JFramework
{
    public static partial class InputManager
    {
        [Serializable]
        private class InputEvent<T> : InputData where T : struct, IEvent
        {
            private event Action<T> Execute;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override void Listen() => Execute += EventManager.Invoke;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override void Remove() => Execute -= EventManager.Invoke;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override void Invoke() => Execute?.Invoke(default);
        }
    }
}