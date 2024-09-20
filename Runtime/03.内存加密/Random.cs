// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: jinyijie
// # Version: 1.0.0
// # History: 2024-09-08  18:09
// # Copyright: 2024, jinyijie
// # Description: This is an automatically generated comment.
// *********************************************************************************

namespace JFramework
{
    internal static class Random
    {
        private static readonly System.Random random = new System.Random();

        public static int Range(int min, int max)
        {
            return random.Next(min, max);
        }
    }
}