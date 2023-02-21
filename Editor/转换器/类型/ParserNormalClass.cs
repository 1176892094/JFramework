namespace JFramework
{
    internal class ParserNormalClass : Parser
    {
        public static ParserNormalClass Parse(string name, string type)
        {
            return new ParserNormalClass()
            {
                name = name, type = type
            };
        }

        public override string GetFieldLine()
        {
            var stringBuilder = ExcelBuilder.Borrow();
            stringBuilder.AppendFormat("public {0} {1};", type, name);
            return ExcelBuilder.Return(stringBuilder);
        }
    }
}