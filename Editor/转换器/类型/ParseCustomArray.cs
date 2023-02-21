using UnityEngine;

namespace JFramework
{
    internal class ParseCustomArray : Parse
    {
        private readonly ParserCustomClass custom;

        public ParseCustomArray(string name, string type) => custom = ParserCustomClass.Parse(name, type);

        public override string GetFieldLine()
        {
            if (custom == null) return null;
            var file = ExcelBuilder.Borrow();
            file.Append(custom.GetClassLine());
            file.Append(custom.GetFieldLine());
            return ExcelBuilder.Return(file);
        }

        public override string GetParseLine()
        {
            if (custom == null) return null;
            var file = ExcelBuilder.Borrow();
            file.AppendFormat("\t\t\tstring {0}Values = sheet[row][column++];\n", custom.fieldName);
            file.AppendFormat("\t\t\tstring[] {0}Group = {0}Values.Split(';');\n", custom.fieldName);
            file.AppendFormat("\t\t\t{0}s = new {0}[{0}Group.Length];\n", custom.fieldName);
            file.AppendFormat("\t\t\tfor (int j = 0; j < {0}Group.Length; ++j)\n", custom.fieldName);
            file.Append("\t\t\t{\n");
            file.AppendFormat("\t\t\t\tvar {0}Value = new {0}();\n", custom.fieldName);
            file.AppendFormat("\t\t\t\tstring[] {0}Array = {0}Group[j].Split(',');\n", custom.fieldName);
            file.AppendFormat("\t\t\t\tfor (int i = 0; i < {0}Array.Length; ++i)\n", custom.fieldName);
            file.Append("\t\t\t\t{\n");
            file.AppendFormat("\t\t\t\t\tvar strValue = {0}Array[i];\n", custom.fieldName);

            for (int i = 0; i < custom.fieldList.Count; ++i)
            {
                file.AppendFormat("\t\t\t\t\t{0}if (i == {1})\n", (i == 0 ? "" : "else "), i);
                file.Append("\t\t\t\t\t{\n");
                file.AppendFormat("\t\t\t\t\t\tstrValue.TryParse(out {0}Value.{1});\n", custom.fieldName, custom.fieldList[i].name);
                file.Append("\t\t\t\t\t}\n");
            }

            file.Append("\t\t\t\t}\n");
            file.AppendFormat("\t\t\t\t{0}s[j] = {0}Value;\n", custom.fieldName);
            file.Append("\t\t\t}\n");
            return ExcelBuilder.Return(file);
        }

        public override string GetInitLine() => null;
    }
}