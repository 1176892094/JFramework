using System.Text;

namespace JFramework
{
    public class ParseNormalArray : IParser
    {
        private readonly string name;
        private readonly string type;
        private readonly bool isVector;

        public ParseNormalArray(string name, string type)
        {
            this.name = name.Trim();
            var fixType =type.Substring(0, type.IndexOf('[')).Trim().ToLower();
            if (fixType is Support.Vector2 or Support.Vector3) isVector = true;
            this.type = fixType.Replace('v', 'V');
        }

        public string GetField()
        {
            var builder = new StringBuilder(1024);
            builder.AppendFormat("\t\tpublic {0}[] {1};\n", type, name);
            return builder.ToString();
        }

        public string GetParse()
        {
            var builder = new StringBuilder(1024);
            builder.AppendFormat("\t\t\tvar {0}Data = sheet[row, column++] ?? \"\";\n",name);
            if (isVector)
            {
              
                builder.AppendFormat("\t\t\tvar {0}Array = {0}Data.Split(\';\');" + "\n", name);
            }
            else
            {
                builder.AppendFormat("\t\t\tvar {0}Array = {0}Data.Split(\',\');" + "\n", name);
            }

            builder.AppendFormat("\t\t\tvar {0}Length = {0}Array.Length;" + "\n", name);
            builder.AppendFormat("\t\t\t{0} = new {1}[{0}Length];\n", name, type);
            builder.AppendFormat("\t\t\tfor(int i = 0; i < {0}Length; i++)\n", name);
            builder.Append("\t\t\t{\n");
            builder.AppendFormat("\t\t\t\t{0}Array[i].TryParse(out {0}[i]);\n", name);
            builder.Append("\t\t\t}\n");
            return builder.ToString();
        }
    }
}