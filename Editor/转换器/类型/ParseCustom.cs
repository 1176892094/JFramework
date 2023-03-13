namespace JFramework
{
    internal class ParseCustom : IParser
    {
        private readonly ParserCustomClass custom;

        public ParseCustom(string name, string type) => custom = ParserCustomClass.Parse(name, type);

        public string GetFieldLine()
        {
            if (custom == null) return null;
            var file = ExcelBuilder.Borrow();
            file.Append(custom.GetParseLine());
            file.Append(custom.GetFieldLine());
            return ExcelBuilder.Return(file);
        }

        public string GetParseLine()
        {
            if (custom == null) return null;
            var file = ExcelBuilder.Borrow();
            file.AppendFormat("\t\t\t{0}s = new {0}();\n", custom.fieldName);
            file.AppendFormat("\t\t\tstring {0}Data = sheet[row][column++];\n", custom.fieldName);
            file.AppendFormat("\t\t\tstring[] {0}Array = {0}Data.Split(',');\n", custom.fieldName);
            file.AppendFormat("\t\t\tfor (int i = 0; i < {0}Array.Length; ++i)\n", custom.fieldName);
            file.Append("\t\t\t{\n");
            file.AppendFormat("\t\t\t\tvar strValue = {0}Array[i];\n", custom.fieldName);
            for (int i = 0; i < custom.fieldList.Count; ++i)
            {
                var field = custom.fieldList[i];
                file.AppendFormat("\t\t\t\t{0}if (i == {1})\n", (i == 0 ? "" : "else "), i);
                file.Append("\t\t\t\t{\n");
                file.AppendFormat("\t\t\t\t\tstrValue.TryParse(out {0}s.{1});\n", custom.fieldName, field.name);
                file.Append("\t\t\t\t}\n");
            }

            file.Append("\t\t\t}\n");

            return ExcelBuilder.Return(file);
        }
    }
}