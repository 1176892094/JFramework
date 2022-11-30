namespace JFramework.Excel
{
	internal class ExcelCustomArray : ExcelBaseType
	{
		private readonly ExcelCustomValue custom;

		public ExcelCustomArray(int id, string name, string type): base(id, name, type)
		{
			custom = ExcelCustomValue.Parse(name, type);
		}
		
		public override string GetFieldLine()
		{
			if (custom == null) return null;
			var file = ExcelStringBuilder.Borrow();
			file.Append(custom.GetClassLine());
			file.Append(custom.GetFieldLine());
			return ExcelStringBuilder.Return(file);
		}

		public override string GetParseLine()
		{
			if (custom == null) return null;
			var file = ExcelStringBuilder.Borrow();
			file.AppendFormat("\t\t\tstring {0}Values = sheet[row][column++];\n", custom.FieldName);
			file.AppendFormat("\t\t\tstring[] {0}Group = {0}Values.Split(';');\n", custom.FieldName);
			file.AppendFormat("\t\t\t{0} = new {1}[{2}Group.Length];\n", custom.FieldName, custom.TypeName, custom.FieldName);
			file.AppendFormat("\t\t\tfor (int j = 0; j < {0}Group.Length; ++j)\n", custom.FieldName);
			file.Append("\t\t\t{\n");
			file.AppendFormat("\t\t\t\tvar {0}Value = new {1}();\n", custom.FieldName, custom.TypeName);
			file.AppendFormat("\t\t\t\t{0}[j] = {0}Value;\n", custom.FieldName);
			file.AppendFormat("\t\t\t\tstring[] {0}Array = {0}Group[j].Split(',');\n", custom.FieldName);
			file.AppendFormat("\t\t\t\tfor (int i = 0; i < {0}Array.Length; ++i)\n", custom.FieldName);
			file.Append("\t\t\t\t{\n");
			file.AppendFormat("\t\t\t\t\tvar strValue = {0}Array[i];\n", custom.FieldName);
			for (int i = 0; i < custom.fields.Count; ++i)
			{
				file.AppendFormat("\t\t\t\t\t{0}if (i == {1})\n", (i == 0 ? "" : "else "), i);
				file.Append("\t\t\t\t\t{\n");
				file.AppendFormat("\t\t\t\t\t\tTryParse(strValue, out {0}Value.{1});\n", custom.FieldName, custom.fields[i].Name);
				file.Append("\t\t\t\t\t}\n");
			}
			file.Append("\t\t\t\t}\n");
			file.Append("\t\t\t}\n");
			return ExcelStringBuilder.Return(file);
		}
		
	}
}