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
using Random = System.Random;

namespace JFramework
{
    public static class Secret
    {
        private static readonly Random random = new Random();
        public static event Action OnAntiCheat;

        public static int Random(int min, int max)
        {
            return random.Next(min, max);
        }

        public static void AntiCheat()
        {
            OnAntiCheat?.Invoke();
        }

        public static bool IsEquals(byte[] origin, byte[] target)
        {
            if (origin == null || target == null || origin.Length != target.Length)
            {
                return false;
            }

            for (int i = 0; i < origin.Length; i++)
            {
                if (origin[i] != target[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}