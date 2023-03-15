using System.Text;

namespace JFramework
{
    internal class ParseNormal : IParser
    {
        private readonly bool isKey;
        private readonly string name;
        private readonly string type;
        private readonly bool isEnum;

        public ParseNormal(string name, string type)
        {
            isKey = ExcelSetting.IsKeyField(name);
            isEnum = ExcelSetting.IsEnumField(type);
            this.name = isKey ? name.Split(':')[0].Trim() : name.Trim();
            this.type = type.Trim().ToLower();
            if (this.type is Const.Vector2 or Const.Vector3)
            {
                this.type = this.type.Replace('v', 'V');
            }
        }

        public string GetField()
        {
            var builder = new StringBuilder(1024);
            if (isKey) builder.Append("\t\t[Key]\n");
            builder.AppendFormat("\t\tpublic {0} {1};\n", isEnum ? name : type, name);
            return builder.ToString();
        }

        public string GetParse()
        {
            return $"\t\t\tsheet[row, column++].TryParse(out {name});\n";
        }
    }
}