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
using Random = System.Random;

namespace JFramework
{
    public static class Secret
    {
        private static readonly Random random = new Random();
        public static event Action OnAntiCheat;

        public static int Random(int min, int max)
        {
            return random.Next(0, max < 0 ? 1024 : max);
        }

        public static void AntiCheat()
        {
            Debug.LogWarning("检查到作弊！");
            // OnAntiCheat?.Invoke();
            // Application.Quit();
        }
    }
}