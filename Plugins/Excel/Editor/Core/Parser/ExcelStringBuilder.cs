using System.Text;
using System.Collections.Generic;

namespace JFramework.Excel
{
    internal static class ExcelStringBuilder
    {
        private static readonly List<StringBuilder> stringBuilders = new List<StringBuilder>();

        public static void Reset()
        {
            stringBuilders.Clear();
        }

        public static StringBuilder Borrow()
        {
            if (stringBuilders.Count == 0)
            {
                return new StringBuilder(1024);
            }

            var first = stringBuilders[0];
            stringBuilders.RemoveAt(0);
            return first;
        }

        public static string Return(StringBuilder builder)
        {
            if (builder == null) return null;
            if (!stringBuilders.Contains(builder))
            {
                stringBuilders.Add(builder);
            }
            var str = builder.ToString();
            builder.Clear();
            return str;
        }
    }
}