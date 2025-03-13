// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2025-01-08 17:01:41
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;

namespace JFramework
{
    public static partial class Extensions
    {
        public static bool IsNullOrEmpty(this string result)
        {
            return string.IsNullOrEmpty(result);
        }

        public static bool IsNullOrWhiteSpace(this string result)
        {
            return string.IsNullOrWhiteSpace(result);
        }

        public static string Bold(this string result)
        {
            return Service.Text.Format("<b>{0}</b>", result);
        }

        public static string Line(this string result)
        {
            return Service.Text.Format("<u>{0}</u>", result);
        }

        public static string Link(this string result, string format)
        {
            return Service.Text.Format("<a href=\"{0}\">{1}</a>", format, result);
        }

        public static string Color(this string result, string format)
        {
            return Service.Text.Format("<color=#{0}>{1}</color>", format, result);
        }

        public static T ToEnum<T>(this string result) where T : struct, Enum
        {
            return Enum.TryParse(result, out T value) ? value : default;
        }

        public static T ToNext<T>(this T current) where T : struct, Enum
        {
            var enumArray = (T[])Enum.GetValues(typeof(T));
            var currIndex = Array.IndexOf(enumArray, current);
            var nextIndex = (currIndex + 1) % enumArray.Length;
            return enumArray[nextIndex];
        }

        public static T ToLast<T>(this T current) where T : struct, Enum
        {
            var enumArray = (T[])Enum.GetValues(typeof(T));
            var currIndex = Array.IndexOf(enumArray, current);
            var lastIndex = (currIndex - 1 + enumArray.Length) % enumArray.Length;
            return enumArray[lastIndex];
        }

        public static T ToRandom<T>(this T current) where T : struct, Enum
        {
            var enumArray = (T[])Enum.GetValues(typeof(T));
            var enumFlags = new List<T>();
            foreach (var item in enumArray)
            {
                if (current.HasFlag(item))
                {
                    enumFlags.Add(item);
                }
            }

            return enumFlags[Service.Random.Next(enumFlags.Count)];
        }
    }
}