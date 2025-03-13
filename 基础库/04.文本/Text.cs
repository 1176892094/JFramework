// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-10 21:01:21
// # Recently: 2025-01-11 18:01:36
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Text;

namespace JFramework
{
    public static partial class Service
    {
        public static class Text
        {
            [ThreadStatic] private static StringBuilder builder;

            public static string Format<T>(string format, T arg1)
            {
                builder ??= new StringBuilder(1024);
                builder.Length = 0;
                builder.AppendFormat(format, arg1);
                return builder.ToString();
            }

            public static string Format<T1, T2>(string format, T1 arg1, T2 arg2)
            {
                builder ??= new StringBuilder(1024);
                builder.Length = 0;
                builder.AppendFormat(format, arg1, arg2);
                return builder.ToString();
            }

            public static string Format<T1, T2, T3>(string format, T1 arg1, T2 arg2, T3 arg3)
            {
                builder ??= new StringBuilder(1024);
                builder.Length = 0;
                builder.AppendFormat(format, arg1, arg2, arg3);
                return builder.ToString();
            }

            public static string Format<T1, T2, T3, T4>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
            {
                builder ??= new StringBuilder(1024);
                builder.Length = 0;
                builder.AppendFormat(format, arg1, arg2, arg3, arg4);
                return builder.ToString();
            }
        }
    }
}