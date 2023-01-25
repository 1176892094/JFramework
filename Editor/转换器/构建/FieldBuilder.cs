using System.Collections.Generic;
using System.Text;

namespace JFramework
{
    internal static class FieldBuilder
    {
        private static readonly List<StringBuilder> stringList = new List<StringBuilder>();

        public static void Reset()
        {
            stringList.Clear();
        }

        public static StringBuilder Borrow()
        {
            if (stringList.Count == 0)
            {
                return new StringBuilder(1024);
            }

            var first = stringList[0];
            stringList.RemoveAt(0);
            return first;
        }

        public static string Return(StringBuilder builder)
        {
            if (builder == null) return null;
            if (!stringList.Contains(builder))
            {
                stringList.Add(builder);
            }
            var str = builder.ToString();
            builder.Clear();
            return str;
        }
    }
}