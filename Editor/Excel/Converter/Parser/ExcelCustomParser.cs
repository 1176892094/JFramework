using System;
using System.Collections.Generic;
using JFramework.Basic;

namespace JFramework.Excel
{
    internal abstract class ExcelValue
    {
        protected string name;
        protected string type;
        public string Name => name;
        public abstract string GetFieldLine();
    }

    internal class ExcelBaseValue : ExcelValue
    {
        public static ExcelBaseValue Parse(string name, string type)
        {
            return new ExcelBaseValue() { name = name, type = type };
        }

        public override string GetFieldLine()
        {
            var stringBuilder = ExcelStringBuilder.Borrow();
            stringBuilder.AppendFormat("public {0} {1};", type, name);
            return ExcelStringBuilder.Return(stringBuilder);
        }
    }

    internal class ExcelCustomValue : ExcelValue
    {
        public string TypeName => name + "Data";
        public string FieldName => name;
        public List<ExcelValue> fields;
        private bool isArray;

        public static ExcelCustomValue Parse(string name, string type)
        {
            int endIndex = type.IndexOf("}", StringComparison.Ordinal);
            string values = type.Substring(1, endIndex - 1);
            string[] valueArray = values.Split(',');
            var fields = new List<ExcelValue>();
            foreach (var value in valueArray)
            {
                string str = value.Trim();
                int spaceIndex = str.IndexOf(" ", StringComparison.Ordinal);
                string localName = str.Substring(spaceIndex).Trim();
                string localType = str.Substring(0, spaceIndex);
                var field = ExcelValueParser.TryParse(localName, localType);
                if (field != null)
                {
                    fields.Add(field);
                }
                else
                {
                    Debugger.LogError($"列的格式设置错误{name}: {str}.");
                    break;
                }
            }

            return new ExcelCustomValue { name = name, isArray = type.EndsWith("[]"), fields = fields };
        }
        
        public override string GetFieldLine()
        {
            var stringBuilder = ExcelStringBuilder.Borrow();
            string arr = isArray ? "[]" : "";
            stringBuilder.AppendFormat("\t\tpublic {0}{1} {2};\n", TypeName, arr, FieldName);
            return ExcelStringBuilder.Return(stringBuilder);
        }

        public string GetClassLine()
        {
            var stringBuilder = ExcelStringBuilder.Borrow();
            stringBuilder.AppendFormat("\t\t[Serializable]\n");
            stringBuilder.AppendFormat("\t\tpublic class {0}\n\t\t{{\n", TypeName);
            foreach (var field in fields)
            {
                stringBuilder.AppendFormat("\t\t\t{0}\n", field.GetFieldLine());
            }

            stringBuilder.Append("\t\t}\n");
            return ExcelStringBuilder.Return(stringBuilder);
        }

        private static class ExcelValueParser
        {
            private delegate ExcelValue ParseFunc(string name, string type);

            private static readonly Dictionary<string, ParseFunc> parserDict = new Dictionary<string, ParseFunc>
            {
                { ExcelType.Int, ExcelBaseValue.Parse },
                { ExcelType.Float, ExcelBaseValue.Parse },
                { ExcelType.Double, ExcelBaseValue.Parse },
                { ExcelType.Long, ExcelBaseValue.Parse },
                { ExcelType.Bool, ExcelBaseValue.Parse },
                { ExcelType.String, ExcelBaseValue.Parse },
            };

            public static ExcelValue TryParse(string name, string type)
            {
                type = type.Trim();
                if (!Excel.ExcelValueParser.IsSystemType(type)) return null;
                parserDict.TryGetValue(type, out var func);
                return func?.Invoke(name, type);
            }
        }
    }
}