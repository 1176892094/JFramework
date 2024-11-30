// *********************************************************************************
// # Project: Forest
// # Unity: 2022.3.5f1c1
// # Author: jinyijie
// # Version: 1.0.0
// # History: 2024-11-30  15:11
// # Copyright: 2024, jinyijie
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;

namespace JFramework
{
    public static class VariableUtils
    {
        private static readonly Random random = new Random();
        public static event Action OnExecute;

        public static double Next()
        {
            return random.NextDouble();
        }

        public static int Next(int min, int max)
        {
            return random.Next(min, max);
        }

        public static void Invoke()
        {
            OnExecute?.Invoke();
        }
    }
}