using System;
using UnityEngine;


namespace JFramework.Utility
{
    public static class StringExtension
    {
        /// <summary>
        /// 判断自身是否有文本
        /// </summary>
        /// <param name="s">传递自身</param>
        /// <returns></returns>
        public static bool IsEmpty(this string s) => string.IsNullOrEmpty(s);

        /// <summary>
        /// string转string
        /// </summary>
        /// <param name="s">改写string类型</param>
        /// <param name="r">输出string类型</param>
        /// <returns>返回改写是否成功</returns>
        public static bool TryParse(this string s, out string r) => (r = s).IsEmpty();

        /// <summary>
        /// string转int
        /// </summary>
        /// <param name="s">改写string类型</param>
        /// <param name="r">输出int类型</param>
        /// <returns>返回改写是否成功</returns>
        public static bool TryParse(this string s, out int r) => int.TryParse(s, out r);

        /// <summary>
        /// string转float
        /// </summary>
        /// <param name="s">改写string类型</param>
        /// <param name="r">输出float类型</param>
        /// <returns>返回改写是否成功</returns>
        public static bool TryParse(this string s, out float r) => float.TryParse(s, out r);

        /// <summary>
        /// string转double
        /// </summary>
        /// <param name="s">改写string类型</param>
        /// <param name="r">输出double类型</param>
        /// <returns>返回改写是否成功</returns>
        public static bool TryParse(this string s, out double r) => double.TryParse(s, out r);

        /// <summary>
        /// string转bool
        /// </summary>
        /// <param name="s">改写string类型</param>
        /// <param name="r">输出bool类型</param>
        /// <returns>返回改写是否成功</returns>
        public static bool TryParse(this string s, out bool r) => bool.TryParse(s, out r);

        /// <summary>
        /// string转long
        /// </summary>
        /// <param name="s">改写string类型</param>
        /// <param name="r">输出long类型</param>
        /// <returns>返回改写是否成功</returns>
        public static bool TryParse(this string s, out long r) => long.TryParse(s, out r);

        /// <summary>
        /// string转枚举
        /// </summary>
        /// <param name="s">改写string类型</param>
        /// <param name="r">输出enum类型</param>
        /// <returns>返回改写是否成功</returns>
        public static void TryParse<T>(this string s, out T r) where T : struct => Enum.TryParse(s, out r);

        /// <summary>
        /// string转Vector2
        /// </summary>
        /// <param name="s">改写string类型</param>
        /// <param name="r">输出Vector2类型</param>
        /// <returns>返回改写是否成功</returns>
        public static void TryParse(this string s, out Vector2 r)
        {
            var values = s.Split(",");
            if (values.Length < 2)
            {
                r = Vector2.zero;
                return;
            }

            var x = float.Parse(values[0]);
            var y = float.Parse(values[1]);
            r = new Vector2(x, y);
        }

        /// <summary>
        /// string转Vector3
        /// </summary>
        /// <param name="s">改写string类型</param>
        /// <param name="r">输出Vector3类型</param>
        /// <returns>返回改写是否成功</returns>
        public static void TryParse(this string s, out Vector3 r)
        {
            var values = s.Split(",");
            if (values.Length < 3)
            {
                r = Vector3.zero;
                return;
            }

            var x = float.Parse(values[0]);
            var y = float.Parse(values[1]);
            var z = float.Parse(values[2]);
            r = new Vector3(x, y, z);
        }
    }
}