using System.Text;

namespace JFramework
{
    public class ParseStruct : IParser
    {
        private readonly StructParser custom;

        public ParseStruct(string name, string type) => custom = StructParser.Parse(name, type);

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
            builder.AppendFormat("\t\t\t{0}s = new {0}();\n", custom.name);
            builder.AppendFormat("\t\t\tvar {0}Data = sheet[row, column++] ?? \"\";\n", custom.name);
            builder.AppendFormat("\t\t\tvar {0}Array = {0}Data.Split(',');\n", custom.name);
            builder.AppendFormat("\t\t\tfor (int i = 0; i < {0}Array.Length; ++i)\n", custom.name);
            builder.Append("\t\t\t{\n");
            builder.AppendFormat("\t\t\t\tvar strValue = {0}Array[i];\n", custom.name);
            for (int i = 0; i < custom.fieldList.Count; ++i)
            {
                var field = custom.fieldList[i];
                builder.AppendFormat("\t\t\t\t{0}if (i == {1})\n", i == 0 ? "" : "else ", i);
                builder.Append("\t\t\t\t{\n");
                builder.AppendFormat("\t\t\t\t\tstrValue.TryParse(out {0}s.{1});\n", custom.name, field.name);
                builder.Append("\t\t\t\t}\n");
            }

            builder.Append("\t\t\t}\n");
            return builder.ToString();
        }
    }
}