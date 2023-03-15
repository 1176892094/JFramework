using System.Text;

namespace JFramework
{
    internal class ParseStructArray : IParser
    {
        private readonly StructParser custom;

        public ParseStructArray(string name, string type) => custom = StructParser.Parse(name, type);

        public string GetField()
        {
            if (custom == null) return null;
            var builder = new StringBuilder(1024);
            builder.Append(custom.GetParse());
            builder.Append(custom.GetField());
            return builder.ToString();
        }

        public string GetParse()
        {
            if (custom == null) return null;
            var builder = new StringBuilder(1024);
            builder.AppendFormat("\t\t\tvar {0}Values = sheet[row, column++] ?? \"\";\n", custom.name);
            builder.AppendFormat("\t\t\tvar {0}Group = {0}Values.Split(';');\n", custom.name);
            builder.AppendFormat("\t\t\t{0}s = new {0}[{0}Group.Length];\n", custom.name);
            builder.AppendFormat("\t\t\tfor (int j = 0; j < {0}Group.Length; ++j)\n", custom.name);
            builder.Append("\t\t\t{\n");
            builder.AppendFormat("\t\t\t\tvar {0}Value = new {0}();\n", custom.name);
            builder.AppendFormat("\t\t\t\tvar {0}Array = {0}Group[j].Split(',');\n", custom.name);
            builder.AppendFormat("\t\t\t\tfor (int i = 0; i < {0}Array.Length; ++i)\n", custom.name);
            builder.Append("\t\t\t\t{\n");
            builder.AppendFormat("\t\t\t\t\tvar strValue = {0}Array[i];\n", custom.name);
            for (int i = 0; i < custom.fieldList.Count; ++i)
            {
                builder.AppendFormat("\t\t\t\t\t{0}if (i == {1})\n", i == 0 ? "" : "else ", i);
                builder.Append("\t\t\t\t\t{\n");
                builder.AppendFormat("\t\t\t\t\t\tstrValue.TryParse(out {0}Value.{1});\n", custom.name, custom.fieldList[i].name);
                builder.Append("\t\t\t\t\t}\n");
            }

            builder.Append("\t\t\t\t}\n");
            builder.AppendFormat("\t\t\t\t{0}s[j] = {0}Value;\n", custom.name);
            builder.Append("\t\t\t}\n");
            return builder.ToString();
        }
    }
}