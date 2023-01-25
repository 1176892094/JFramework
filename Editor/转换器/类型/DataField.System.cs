namespace JFramework
{
    internal class ExcelSystemType : DataField
    {
        private readonly string fieldName;
        private readonly string fieldType;

        public ExcelSystemType(int id, string name, string type) : base(id, name, type)
        {
            fieldName = IsKey ? name.Split(':')[0].Trim() : name.Trim();
            fieldType = type.Trim();
        }

        public override string GetFieldLine()
        {
            var file = FieldBuilder.Borrow();
            if (IsKey) file.Append("\t\t[Key]\n");
            if (fieldType == "enum") return "";
            file.AppendFormat("\t\tpublic {0} {1};\n", fieldType, fieldName);
            return FieldBuilder.Return(file);
        }

        public override string GetParseLine()
        {
            if (fieldType == "enum") return "\t\t\tcolumn++;\n";
            return "\t\t\tTryParse(sheet[row][column++], out " + fieldName + ");\n";
        }

        public override string GetInitLine()
        {
            return null;
        }
    }
}