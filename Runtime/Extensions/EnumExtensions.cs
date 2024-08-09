// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-4-4  18:12
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;

namespace JFramework
{
    public static partial class Extensions
    {
        public static T ToNext<T>(this T current) where T : Enum
        {
            var enumArray = (T[])Enum.GetValues(typeof(T));
            var currIndex = System.Array.IndexOf(enumArray, current);
            var nextIndex = (currIndex + 1) % enumArray.Length;
            return enumArray[nextIndex];
        }

        public static T ToLast<T>(this T current) where T : Enum
        {
            var enumArray = (T[])Enum.GetValues(typeof(T));
            var currIndex = System.Array.IndexOf(enumArray, current);
            var lastIndex = (currIndex - 1 + enumArray.Length) % enumArray.Length;
            return enumArray[lastIndex];
        }
    }
}