namespace JFramework
{
    internal class ParseNormalArray : Parse
    {
        private readonly string fieldName;
        private readonly string fieldType;

        public ParseNormalArray(string name, string type)
        {
            fieldName = name.Trim();
            int startIndex = type.IndexOf('[');
            fieldType = type.Substring(0, startIndex).Trim();
        }

        public override string GetFieldLine()
        {
            var file = ExcelBuilder.Borrow();
            file.AppendFormat("\t\tpublic {0}[] {1};\n", fieldType, fieldName);
            return ExcelBuilder.Return(file);
        }

        public override string GetParseLine()
        {
            var file = ExcelBuilder.Borrow();
            file.AppendFormat("\t\t\tstring[] {0}Array = sheet[row][column++].Split(\',\');" + "\n", fieldName);
            file.AppendFormat("\t\t\tint {0}Length = {0}Array.Length;" + "\n", fieldName);
            file.AppendFormat("\t\t\t{0} = new {1}[{0}Length];\n", fieldName, fieldType);
            file.AppendFormat("\t\t\tfor(int i = 0; i < {0}Length; i++)\n", fieldName);
            file.Append("\t\t\t{\n");
            file.AppendFormat("\t\t\t\t{0}Array[i].TryParse(out {0}[i]);\n", fieldName);
            file.Append("\t\t\t}\n");
            return ExcelBuilder.Return(file);
        }

        public override string GetInitLine() => null;
    }
}