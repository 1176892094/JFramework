using System.Text;

namespace JFramework
{
    public class ParseNormal : IParser
    {
        private readonly bool isKey;
        private readonly string name;
        private readonly string type;

        public ParseNormal(string name, string type)
        {
            isKey = ParseSetting.IsKeyField(name, type);
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
            builder.AppendFormat("\t\tpublic {0} {1};\n", type, name);
            return builder.ToString();
        }

        public string GetParse()
        {
            return $"\t\t\tsheet[row, column++].TryParse(out {name});\n";
        }
    }
}