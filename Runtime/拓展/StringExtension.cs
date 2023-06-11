// ReSharper disable All
namespace JFramework
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
        /// 设置为绿色字符串
        /// </summary>
        public static string Green(this string s) => s.SetColor(TextColor.Green);
        
        /// <summary>
        /// 设置为白色字符串
        /// </summary>
        public static string White(this string s) => s.SetColor(TextColor.White);
        
        /// <summary>
        /// 设置为紫色字符串
        /// </summary>
        public static string Purple(this string s) => s.SetColor(TextColor.Purple);
        
        /// <summary>
        /// 设置为黄色字符串
        /// </summary>
        public static string Yellow(this string s) => s.SetColor(TextColor.Yellow);
        
        /// <summary>
        /// 设置为橙色字符串
        /// </summary>
        public static string Orange(this string s) => s.SetColor(TextColor.Orange);
        
        /// <summary>
        /// 设置为粉色字符串
        /// </summary>
        public static string Pink(this string s) => s.SetColor(TextColor.Pink);
        
        /// <summary>
        /// 设置为蓝色字符串
        /// </summary>
        public static string Blue(this string s) => s.SetColor(TextColor.Blue);
        
        /// <summary>
        /// 设置为红色字符串
        /// </summary>
        public static string Red(this string s) => s.SetColor(TextColor.Red);
        
        /// <summary>
        /// 设置为天蓝字符串
        /// </summary>
        public static string Sky(this string s) => s.SetColor(TextColor.Sky);

        /// <summary>
        /// 设置字符串颜色
        /// </summary>
        public static string SetColor(this string s, TextColor type)
        {
            return type switch
            {
                TextColor.White => "<color=#FFFFFF>" + s + "</color>",
                TextColor.Yellow => "<color=#FFFF00>" + s + "</color>",
                TextColor.Sky => "<color=#00FFFF>" + s + "</color>",
                TextColor.Purple => "<color=#FF00AA>" + s + "</color>",
                TextColor.Orange => "<color=#FFAA00>" + s + "</color>",
                TextColor.Red => "<color=#FF0000>" + s + "</color>",
                TextColor.Blue => "<color=#00CCFF>" + s + "</color>",
                TextColor.Green => "<color=#00FF00>" + s + "</color>",
                TextColor.Pink => "<color=#FFAACC>" + s + "</color>",
                _ => s
            };
        }
    }

    public enum TextColor
    {
        Sky = 0,
        Red = 1,
        Blue = 2,
        Pink = 3,
        Green = 4,
        White = 5,
        Yellow = 6,
        Purple = 7,
        Orange = 8,
    }
}