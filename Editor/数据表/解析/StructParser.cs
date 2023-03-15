using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace JFramework
{
    internal class StructParser : Parser
    {
        public List<Parser> fieldList;
        private bool isArray;

        public override string GetField()
        {
            var builder = new StringBuilder(1024);
            var array = isArray ? "[]" : "";
            builder.AppendFormat("\t\tpublic {0}{1} {0}s;\n", name, array);
            return builder.ToString();
        }

        public override string GetParse()
        {
            var builder = new StringBuilder(1024);
            builder.AppendFormat("\n\t\t[Serializable]\n");
            builder.AppendFormat("\t\tpublic struct {0}\n\t\t{{\n", name);

            foreach (var field in fieldList)
            {
                builder.AppendFormat("\t\t\t{0}\n", field.GetField());
            }

            builder.Append("\t\t}\n\n");
            return builder.ToString();
        }

        public static StructParser Parse(string name, string type)
        {
            var index = type.IndexOf("}", StringComparison.Ordinal);
            var content = type.Substring(1, index - 1);
            var values = content.Split(',');
            var fields = new List<Parser>();
            foreach (var value in values)
            {
                var data = value.Trim();
                var space = data.IndexOf(" ", StringComparison.Ordinal);
                var fieldName = data.Substring(space).Trim();
                var fieldType = data.Substring(0, space);
                var field = ParseAction.TryParse(fieldName, fieldType);
                if (field != null)
                {
                    fields.Add(field);
                }
                else
                {
                    Debug.LogError($"格式错误{name}=>{data}");
                    break;
                }
            }

            return new StructParser { name = name, isArray = type.EndsWith("[]"), fieldList = fields };
        }

        private static class ParseAction
        {
            private delegate Struct Action(string name, string type);

            private static readonly Dictionary<string, Action> dataDict = new Dictionary<string, Action>
            {
                { Const.Int, Struct.Create },
                { Const.Bool, Struct.Create },
                { Const.Long, Struct.Create },
                { Const.Float, Struct.Create },
                { Const.Double, Struct.Create },
                { Const.String, Struct.Create },
                { Const.Vector2, Struct.Create },
                { Const.Vector3, Struct.Create },
            };

            public static Struct TryParse(string name, string type)
            {
                type = type.Trim();
                if (!Const.Array.Any(type.Equals)) return null;
                dataDict.TryGetValue(type, out var action);
                return action?.Invoke(name, type);
            }
        }

        private class Struct : Parser
        {
            public static Struct Create(string name, string type) => new Struct() { name = name, type = type };

            public override string GetField()
            {
                var builder = new StringBuilder(1024);
                builder.AppendFormat("public {0} {1};", type, name);
                return builder.ToString();
            }

            public override string GetParse() => null;
        }
    }
}