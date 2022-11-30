namespace JFramework.Excel
{
	internal class ExcelSystemType : ExcelBaseType
	{
		private readonly string fieldName;
		private readonly string fieldType;

		public ExcelSystemType(int id, string name, string type): base(id, name, type)
		{
			fieldName = isKeyField ? name.Split(':')[0].Trim() : name.Trim();
			fieldType = type.Trim();
		}
		
		public override string GetFieldLine()
		{
			var file = ExcelStringBuilder.Borrow();
			if (isKeyField) file.Append("\t\t[ExcelKey]\n");
			file.AppendFormat("\t\tpublic {0} {1};\n", fieldType, fieldName);
			return ExcelStringBuilder.Return(file);
		}
		
		public override string GetParseLine()
		{
			return "\t\t\tTryParse(sheet[row][column++], out " + fieldName + ");\n";
		}
	}
}