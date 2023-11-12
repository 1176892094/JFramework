// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-10-24  23:41
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

// ReSharper disable All

namespace JFramework
{
    /// <summary>
    /// 字符串方法拓展
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 是否为null或者""
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsEmpty(this string s) => string.IsNullOrEmpty(s);

        /// <summary>
        /// 设置富文本颜色 (绿色)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Green(this string s) => s.Color(0x00FF00);

        /// <summary>
        /// 设置富文本颜色 (黄色)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Yellow(this string s) => s.Color(0xFFFF00);

        /// <summary>
        /// 设置富文本颜色 (橙色)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Orange(this string s) => s.Color(0xFFAA00);

        /// <summary>
        /// 设置富文本颜色 (粉色)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Pink(this string s) => s.Color(0xFFAACC);

        /// <summary>
        /// 设置富文本颜色 (蓝色)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Blue(this string s) => s.Color(0x00CCFF);

        /// <summary>
        /// 设置富文本颜色 (红色)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Red(this string s) => s.Color(0xFF0000);

        /// <summary>
        /// 设置富文本颜色 (天蓝色)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Sky(this string s) => s.Color(0x00FFFF);

        /// <summary>
        /// 设置富文本颜色 (自定义)
        /// </summary>
        /// <param name="s"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string Color(this string s, int color) => $"<color=#{color:X6}>{s}</color>";
    }
}