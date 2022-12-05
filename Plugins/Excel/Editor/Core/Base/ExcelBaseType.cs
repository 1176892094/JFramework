
namespace JFramework.Excel
{
	internal abstract class ExcelBaseType
	{
		private readonly int ID;
		private readonly string Name;
		private readonly string Type;
		protected readonly bool isKeyField;
		
		protected ExcelBaseType(int ID, string Name, string Type)
		{
			this.ID = ID;
			this.Name = Name;
			this.Type = Type;
			isKeyField = ExcelValueParser.IsKeyValue(Name, Type);
		}
		
		public abstract string GetFieldLine();

		public abstract string GetParseLine();

		public virtual string GetInitLine()
		{
			return null;
		}
	}
}