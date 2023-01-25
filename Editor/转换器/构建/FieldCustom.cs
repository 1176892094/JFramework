using System;
using System.Collections.Generic;
using JFramework.Interface;
using UnityEngine;

namespace JFramework
{
    internal struct ExcelType
    {
        public const string KeyValue = ":key";
        public const string Int = "int";
        public const string Bool = "bool";
        public const string Long = "long";
        public const string Enum = "enum";
        public const string Float = "float";
        public const string String = "string";
        public const string Double = "double";
        public static readonly string[] TypeArray = { Int, Bool, Enum, Long, Float, Double, String };
    }

    internal class FieldCustom : Field
    {
        public List<IField> fieldList;
        public string typeName => name + "Data";
        public string fieldName => name;

        private bool isArray;

        public static FieldCustom Parse(string name, string type)
        {
            int endIndex = type.IndexOf("}", StringComparison.Ordinal);
            string values = type.Substring(1, endIndex - 1);
            string[] valueArray = values.Split(',');
            var fields = new List<IField>();
            foreach (var value in valueArray)
            {
                string str = value.Trim();
                int spaceIndex = str.IndexOf(" ", StringComparison.Ordinal);
                string localName = str.Substring(spaceIndex).Trim();
                string localType = str.Substring(0, spaceIndex);
                var field = FieldCustomParser.TryParse(localName, localType);
                if (field != null)
                {
                    fields.Add(field);
                }
                else
                {
                    Debug.LogError($"列的格式设置错误{name}: {str}.");
                    break;
                }
            }

            return new FieldCustom { name = name, isArray = type.EndsWith("[]"), fieldList = fields };
        }

        protected override string GetFieldLine()
        {
            var stringBuilder = FieldBuilder.Borrow();
            string arr = isArray ? "[]" : "";
            stringBuilder.AppendFormat("\t\tpublic {0}{1} {2};\n", typeName, arr, fieldName);
            return FieldBuilder.Return(stringBuilder);
        }

        public string GetClassLine()
        {
            var stringBuilder = FieldBuilder.Borrow();
            stringBuilder.AppendFormat("\t\t[Serializable]\n");
            stringBuilder.AppendFormat("\t\tpublic class {0}\n\t\t{{\n", typeName);
            foreach (var field in fieldList)
            {
                stringBuilder.AppendFormat("\t\t\t{0}\n", field.GetFieldLine());
            }

            stringBuilder.Append("\t\t}\n");
            return FieldBuilder.Return(stringBuilder);
        }

        private static class FieldCustomParser
        {
            private delegate IField ParseFunc(string name, string type);

            private static readonly Dictionary<string, ParseFunc> parserDict = new Dictionary<string, ParseFunc>
            {
                { ExcelType.Int, FieldSystem.Parse },
                { ExcelType.Bool, FieldSystem.Parse },
                { ExcelType.Enum, FieldSystem.Parse },
                { ExcelType.Long, FieldSystem.Parse },
                { ExcelType.Float, FieldSystem.Parse },
                { ExcelType.Double, FieldSystem.Parse },
                { ExcelType.String, FieldSystem.Parse },
            };

            public static IField TryParse(string name, string type)
            {
                type = type.Trim();
                if (!FieldParser.IsSystemType(type)) return null;
                parserDict.TryGetValue(type, out var func);
                return func?.Invoke(name, type);
            }
        }
    }
}