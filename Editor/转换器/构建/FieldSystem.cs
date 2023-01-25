namespace JFramework
{
    internal class FieldSystem : Field
    {
        public static FieldSystem Parse(string name, string type)
        {
            return new FieldSystem()
            {
                name = name, type = type
            };
        }

        protected override string GetFieldLine()
        {
            var stringBuilder = FieldBuilder.Borrow();
            stringBuilder.AppendFormat("public {0} {1};", type, name);
            return FieldBuilder.Return(stringBuilder);
        }
    }
}