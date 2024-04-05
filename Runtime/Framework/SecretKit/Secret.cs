// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: jinyijie
// # Version: 1.0.0
// # History: 2024-4-4  3:18
// # Copyright: 2024, jinyijie
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using UnityEngine;

namespace JFramework
{
    public static class Secret
    {
        public static event Action OnAntiCheat;

        public static void AntiCheat()
        {
            Debug.LogWarning("检查到作弊！");
            OnAntiCheat?.Invoke();
        }
    }
}