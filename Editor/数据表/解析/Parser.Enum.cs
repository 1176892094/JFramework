namespace JFramework
{
    public class ParseEnum : IParser
    {
        private readonly string name;
        private readonly bool isField;

        public ParseEnum(string name, string type)
        {
            isField = ParseSetting.IsEnumField(type);
            this.name = isField ? name.Split(':')[0].Trim() : name.Trim();
        }

        public string GetField() => isField ? $"\t\tpublic {name} {name};\n" : null;

        public string GetParse() => isField ? $"\t\t\tsheet[row, column++].TryParse(out {name});\n" : "\t\t\tcolumn++;\n";
    }
}