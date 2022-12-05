using System;

namespace JFramework.Excel
{
	internal class ExcelCustomDict : ExcelBaseType
	{
		private readonly string fieldName;
		private readonly string keyType;
		private readonly string valueType;
		private readonly string fieldType;
		private readonly ExcelCustomValue custom;

		public ExcelCustomDict(int id, string name, string type): base(id, name, type)
		{
			fieldName = name.Trim();
			int startIndex = type.IndexOf('<');
			int stepIndex = type.IndexOf(',');
			int endIndex = type.IndexOf('>');
			keyType = type.Substring(startIndex+1, stepIndex - startIndex - 1).Trim();
			valueType = type.Substring(stepIndex + 1, endIndex - stepIndex - 1).Trim();
			int startCustom = type.IndexOf("{", StringComparison.Ordinal);
			int endCustom = type.IndexOf("}", StringComparison.Ordinal);
			string customType = type.Substring(startCustom, endCustom - startCustom + 1);
			custom = ExcelCustomValue.Parse(name, customType);
			valueType = custom.TypeName;
		}
		
		public override string GetFieldLine()
		{
			var file = ExcelStringBuilder.Borrow();
			file.AppendFormat("{0}", custom.GetClassLine());
			file.Append("\n");
			file.AppendFormat("\t\t[SerializeField] private {0}[] {1}Keys;\n", keyType, fieldName);
			file.AppendFormat("\t\t[SerializeField] private {0}[] {1}Values;\n", valueType, fieldName);
			file.AppendFormat("\t\tpublic Dictionary<{0},{1}> {2} = new Dictionary<{0},{1}>();\n", keyType, valueType, fieldName);
			return ExcelStringBuilder.Return(file);
		}

		public override string GetParseLine()
		{
			var file = ExcelStringBuilder.Borrow();
			file.AppendFormat("\t\t\tstring {0}RawData = sheet[row][column++];\n", fieldName);
			file.AppendFormat("\t\t\tstring[] {0}Pairs = {0}RawData.Split(';');\n", fieldName);
			file.AppendFormat("\t\t\tList<{1}> {0}KeyList = new List<{1}>();\n", fieldName, keyType);
			file.AppendFormat("\t\t\tList<{1}> {0}ValueList = new List<{1}>();\n", fieldName, valueType);
			file.AppendFormat("\t\t\tforeach (var {0}Pair in {0}Pairs)\n", fieldName);
			file.Append("\t\t\t{\n");
			file.AppendFormat("\t\t\t\tstring[] {0}PairArray = {0}Pair.Split(':');\n", fieldName);
			file.AppendFormat("\t\t\t\tif ({0}PairArray.Length < 2) continue;\n", fieldName);
			file.AppendFormat("\t\t\t\t{0} {1}Key;\n", keyType, fieldName);
			file.AppendFormat("\t\t\t\tTryParse({0}PairArray[0], out {0}Key);\n", fieldName);
			file.AppendFormat("\t\t\t\t{0} {1}Value;\n", valueType, fieldName);
			file.AppendFormat("\t\t\t\t{0}Value = new {1}();\n", fieldName, custom.TypeName);
			file.AppendFormat("\t\t\t\tstring {0}Group = {0}PairArray[1];\n", fieldName);
			file.AppendFormat("\t\t\t\tstring[] {0}Array = {1}Group.Split(',');\n", custom.FieldName, fieldName);
			file.AppendFormat("\t\t\t\tfor (int j = 0; j < {0}Array.Length; ++j)\n", custom.FieldName);
			file.Append("\t\t\t\t{\n");
			file.AppendFormat("\t\t\t\t\tvar strValue = {0}Array[j];\n", custom.FieldName);
			for (int i = 0; i < custom.fields.Count; ++i)
			{
				file.AppendFormat("\t\t\t\t\t{0}if (j == {1})\n", (i == 0 ? "" : "else "), i);
				file.AppendFormat("\t\t\t\t\t\tTryParse(strValue, out {0}Value.{1});\n", fieldName, custom.fields[i].Name);
			}
			file.Append("\t\t\t\t}\n");
			file.AppendFormat("\t\t\t\t{0}KeyList.Add({0}Key);\n", fieldName);
			file.AppendFormat("\t\t\t\t{0}ValueList.Add({0}Value);\n", fieldName);
			file.Append("\t\t\t}\n");
			file.AppendFormat("\t\t\t{0}Keys = {0}KeyList.ToArray();\n", fieldName);
			file.AppendFormat("\t\t\t{0}Values = {0}ValueList.ToArray();\n", fieldName);
			
			return ExcelStringBuilder.Return(file);
		}

		public override string GetInitLine()
		{
			var file = ExcelStringBuilder.Borrow();
			file.AppendFormat("\t\t\tfor (int i = 0; i < {0}Keys.Length; ++i)\n", fieldName);
			file.Append("\t\t\t{\n");
			file.AppendFormat("\t\t\t\tvar k = {0}Keys[i];\n", fieldName);
			file.AppendFormat("\t\t\t\tvar v = {0}Values[i];\n", fieldName);
			file.AppendFormat("\t\t\t\tif ({0}.ContainsKey(k))\n", fieldName);
			file.Append("\t\t\t\t{\n");
			file.AppendFormat("\t\t\t\t\tLogger.LogError(\"Dictionary {0} already has the key \" + k + \".\");\n", fieldName);
			file.Append("\t\t\t\t}\n");
			file.Append("\t\t\t\telse\n");
			file.Append("\t\t\t\t{\n");
			file.AppendFormat("\t\t\t\t\t{0}.Add(k, v);\n", fieldName);
			file.Append("\t\t\t\t}\n");
			file.Append("\t\t\t}\n");
			return ExcelStringBuilder.Return(file);
		}
	}
}