namespace JFramework
{
    internal class ParseNormal : IParser
    {
        private readonly bool isKey;
        private readonly string fieldName;
        private readonly string fieldType;

        public ParseNormal(string name, string type)
        {
            isKey = ExcelBuilder.GetKeyValue(name, type);
            fieldName = isKey ? name.Split(':')[0].Trim() : name.Trim();
            fieldType = type.Trim();
        }

        public string GetFieldLine()
        {
            var file = ExcelBuilder.Borrow();
            if (isKey) file.Append("\t\t[Key]\n");
            if (fieldType == "enum") return "";
            file.AppendFormat("\t\tpublic {0} {1};\n", fieldType, fieldName);
            return ExcelBuilder.Return(file);
        }

        public string GetParseLine()
        {
            if (fieldType == "enum") return "\t\t\tcolumn++;\n";
            return $"\t\t\tsheet[row][column++].TryParse(out {fieldName});\n";
        }
    }
}