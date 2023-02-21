using System;
using System.Collections.Generic;
using UnityEngine;

namespace JFramework
{
    internal class ParserCustomClass : Parser
    {
        public List<Parser> fieldList;
        public string typeName => name + "Data";
        public string fieldName => name;

        private bool isArray;

        public override string GetFieldLine()
        {
            var file = ExcelBuilder.Borrow();
            var array = isArray ? "[]" : "";
            file.AppendFormat("\t\tpublic {0}{1} {0}s;\n", fieldName, array);
            return ExcelBuilder.Return(file);
        }

        public string GetClassLine()
        {
            var file = ExcelBuilder.Borrow();
            file.AppendFormat("\t\t[Serializable]\n");
            file.AppendFormat("\t\tpublic struct {0}\n\t\t{{\n", fieldName);
            
            foreach (var field in fieldList)
            {
                file.AppendFormat("\t\t\t{0}\n", field.GetFieldLine());
            }

            file.Append("\t\t}\n");
            return ExcelBuilder.Return(file);
        }
        
        public static ParserCustomClass Parse(string name, string type)
        {
            var endIndex = type.IndexOf("}", StringComparison.Ordinal);
            var values = type.Substring(1, endIndex - 1);
            var valueArray = values.Split(',');
            var fields = new List<Parser>();
            foreach (var value in valueArray)
            {
                var str = value.Trim();
                var spaceIndex = str.IndexOf(" ", StringComparison.Ordinal);
                var localName = str.Substring(spaceIndex).Trim();
                var localType = str.Substring(0, spaceIndex);
                var field = CustomParser.TryParse(localName, localType);
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

            return new ParserCustomClass { name = name, isArray = type.EndsWith("[]"), fieldList = fields };
        }

        private static class CustomParser
        {
            private delegate Parser ParseFunc(string name, string type);

            private static readonly Dictionary<string, ParseFunc> parserDict = new Dictionary<string, ParseFunc>
            {
                { EditorConst.Int, ParserNormalClass.Parse },
                { EditorConst.Bool, ParserNormalClass.Parse },
                { EditorConst.Enum, ParserNormalClass.Parse },
                { EditorConst.Long, ParserNormalClass.Parse },
                { EditorConst.Float, ParserNormalClass.Parse },
                { EditorConst.Double, ParserNormalClass.Parse },
                { EditorConst.String, ParserNormalClass.Parse },
            };

            public static Parser TryParse(string name, string type)
            {
                type = type.Trim();
                if (!ExcelBuilder.IsNormalData(type)) return null;
                parserDict.TryGetValue(type, out var func);
                return func?.Invoke(name, type);
            }
        }
    }
}