using JFramework.Interface;

namespace JFramework
{
    internal class CustomData : DataField
    {
        private readonly FieldCustom custom;

        public CustomData(int id, string name, string type) : base(id, name, type)
        {
            custom = FieldCustom.Parse(name, type);
        }

        public override string GetFieldLine()
        {
            if (custom == null) return null;
            var file = FieldBuilder.Borrow();
            file.Append(custom.GetClassLine());
            file.Append(((IField)custom).GetFieldLine());
            return FieldBuilder.Return(file);
        }

        public override string GetParseLine()
        {
            if (custom == null) return null;
            var file = FieldBuilder.Borrow();
            file.AppendFormat("\t\t\t{0} = new {1}();\n", custom.fieldName, custom.typeName);
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
                file.AppendFormat("\t\t\t\t\tTryParse(strValue, out {0}.{1});\n", custom.fieldName, field.Name);
                file.Append("\t\t\t\t}\n");
            }

            file.Append("\t\t\t}\n");

            return FieldBuilder.Return(file);
        }

        public override string GetInitLine()
        {
            return null;
        }
    }
}