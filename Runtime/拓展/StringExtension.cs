// ReSharper disable All
namespace JFramework
{
    public static class StringExtension
    {
        public static bool IsEmpty(this string s) => string.IsNullOrEmpty(s);
        public static string Green(this string s) => s.SetColor(TextColor.Green);
        public static string White(this string s) => s.SetColor(TextColor.White);
        public static string Purple(this string s) => s.SetColor(TextColor.Purple);
        public static string Yellow(this string s) => s.SetColor(TextColor.Yellow);
        public static string Orange(this string s) => s.SetColor(TextColor.Orange);
        public static string Pink(this string s) => s.SetColor(TextColor.Pink);
        public static string Blue(this string s) => s.SetColor(TextColor.Blue);
        public static string Red(this string s) => s.SetColor(TextColor.Red);
        public static string Sky(this string s) => s.SetColor(TextColor.Sky);
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