// *********************************************************************************
// # Project: Forest
// # Unity: 2022.3.5f1c1
// # Author: jinyijie
// # Version: 1.0.0
// # History: 2024-08-25  03:08
// # Copyright: 2024, jinyijie
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using JFramework.Interface;

namespace JFramework.Core
{
    public static partial class EventManager
    {
        private class Event<T> : IEvent where T : struct, IEvent
        {
            public Action<T> Execute;
        }
    }
}