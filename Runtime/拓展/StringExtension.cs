using System;
using UnityEngine;


namespace JFramework
{
    public static class StringExtension
    {
        /// <summary>
        /// 判断自身是否有文本
        /// </summary>
        /// <param name="self">传递自身</param>
        /// <returns></returns>
        public static bool IsEmpty(this string self) => string.IsNullOrEmpty(self);

        /// <summary>
        /// string转string
        /// </summary>
        /// <param name="reword">改写string类型</param>
        /// <param name="result">输出string类型</param>
        /// <returns>返回改写是否成功</returns>
        public static bool TryParse(this string reword, out string result) => (result = reword).IsEmpty();

        /// <summary>
        /// string转int
        /// </summary>
        /// <param name="reword">改写string类型</param>
        /// <param name="result">输出int类型</param>
        /// <returns>返回改写是否成功</returns>
        public static bool TryParse(this string reword, out int result) => int.TryParse(reword, out result);

        /// <summary>
        /// string转float
        /// </summary>
        /// <param name="reword">改写string类型</param>
        /// <param name="result">输出float类型</param>
        /// <returns>返回改写是否成功</returns>
        public static bool TryParse(this string reword, out float result) => float.TryParse(reword, out result);

        /// <summary>
        /// string转double
        /// </summary>
        /// <param name="reword">改写string类型</param>
        /// <param name="result">输出double类型</param>
        /// <returns>返回改写是否成功</returns>
        public static bool TryParse(this string reword, out double result) => double.TryParse(reword, out result);

        /// <summary>
        /// string转bool
        /// </summary>
        /// <param name="reword">改写string类型</param>
        /// <param name="result">输出bool类型</param>
        /// <returns>返回改写是否成功</returns>
        public static bool TryParse(this string reword, out bool result) => bool.TryParse(reword, out result);

        /// <summary>
        /// string转long
        /// </summary>
        /// <param name="reword">改写string类型</param>
        /// <param name="result">输出long类型</param>
        /// <returns>返回改写是否成功</returns>
        public static bool TryParse(this string reword, out long result) => long.TryParse(reword, out result);

        /// <summary>
        /// string转枚举
        /// </summary>
        /// <param name="reword">改写string类型</param>
        /// <param name="result">输出enum类型</param>
        /// <returns>返回改写是否成功</returns>
        public static void TryParse<T>(this string reword, out T result) where T : struct => Enum.TryParse(reword, out result);

        /// <summary>
        /// string转Vector2
        /// </summary>
        /// <param name="reword">改写string类型</param>
        /// <param name="result">输出Vector2类型</param>
        /// <returns>返回改写是否成功</returns>
        public static void TryParse(this string reword, out Vector2 result)
        {
            var values = reword.Split(",");
            if (values.Length < 2)
            {
                result = Vector2.zero;
                return;
            }

            var x = float.Parse(values[0]);
            var y = float.Parse(values[1]);
            result = new Vector2(x, y);
        }

        /// <summary>
        /// string转Vector3
        /// </summary>
        /// <param name="reword">改写string类型</param>
        /// <param name="result">输出Vector3类型</param>
        /// <returns>返回改写是否成功</returns>
        public static void TryParse(this string reword, out Vector3 result)
        {
            var values = reword.Split(",");
            if (values.Length < 3)
            {
                result = Vector3.zero;
                return;
            }

            var x = float.Parse(values[0]);
            var y = float.Parse(values[1]);
            var z = float.Parse(values[2]);
            result = new Vector3(x, y, z);
        }
    }
}