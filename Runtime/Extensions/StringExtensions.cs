// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-02-04  22:58
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

namespace JFramework
{
    public static partial class Extensions
    {
        public static bool IsEmpty(this string s) => string.IsNullOrEmpty(s);
        
        public static string Color(this string s, int color) => $"<color=#{color:X6}>{s}</color>";

        public static string Green(this string s) => s.Color(0x00FF00);

        public static string Yellow(this string s) => s.Color(0xFFFF00);

        public static string Orange(this string s) => s.Color(0xFFAA00);

        public static string Pink(this string s) => s.Color(0xFFAACC);

        public static string Blue(this string s) => s.Color(0x00CCFF);

        public static string Red(this string s) => s.Color(0xFF0000);

        public static string Sky(this string s) => s.Color(0x00FFFF);
    }
}