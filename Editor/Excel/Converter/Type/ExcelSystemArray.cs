namespace JFramework.Excel
{
    internal class ExcelSystemArray : ExcelBaseType
    {
        private readonly string fieldName;
        private readonly string fieldType;

        public ExcelSystemArray(int id, string name, string type): base(id, name, type)
        {
            fieldName = name.Trim();
            int startIndex = type.IndexOf('[');
            fieldType = type.Substring(0, startIndex).Trim();
        }
		
        public override string GetFieldLine()
        {
            var file = ExcelStringBuilder.Borrow();
            file.AppendFormat("\t\tpublic {0}[] {1};\n", fieldType, fieldName);
            return ExcelStringBuilder.Return(file);
        }

        public override string GetParseLine()
        {
            var file = ExcelStringBuilder.Borrow();
            file.Append("\t\t\tstring[] " + fieldName + "Array = sheet[row][column++].Split(\',\');" + "\n");
            file.Append("\t\t\tint " + fieldName + "Count = " + fieldName + "Array.Length;" + "\n");
            file.Append("\t\t\t" + fieldName + " = new " + fieldType + "[" + fieldName + "Count];\n");
            file.Append("\t\t\tfor(int i = 0; i < " + fieldName + "Count; i++)\n");
            file.Append("\t\t\t{\n");
            file.Append("\t\t\t\tTryParse(" + fieldName + "Array[i], out " + fieldName + "[i]);\n");
            file.Append("\t\t\t}\n");
            return ExcelStringBuilder.Return(file);
        }
    }
}