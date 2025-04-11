// // *********************************************************************************
// // # Project: Astraia
// // # Unity: 6000.3.5f1
// // # Author: 云谷千羽
// // # Version: 1.0.0
// // # History: 2025-04-09 21:04:23
// // # Recently: 2025-04-09 21:04:23
// // # Copyright: 2024, 云谷千羽
// // # Description: This is an automatically generated comment.
// // *********************************************************************************

using System;

namespace Astraia
{
    public static partial class Service
    {
        public static class Random
        {
            private static readonly System.Random random = new System.Random(Environment.TickCount);

            public static int Next()
            {
                return random.Next();
            }

            public static int Next(int max)
            {
                return random.Next(max);
            }

            public static int Next(int min, int max)
            {
                return random.Next(min, max);
            }

            public static double NextDouble()
            {
                return random.NextDouble();
            }

            public static void NextBytes(byte[] bytes)
            {
                random.NextBytes(bytes);
            }
        }
    }
}