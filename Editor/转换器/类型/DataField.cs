
namespace JFramework
{
	internal abstract class DataField
	{
		private readonly int ID;
		private readonly string Name;
		private readonly string Type;
		protected readonly bool IsKey;
		
		protected DataField(int ID, string Name, string Type)
		{
			this.ID = ID;
			this.Name = Name;
			this.Type = Type;
			IsKey = FieldParser.GetKeyValue(Name, Type);
		}
		
		public abstract string GetFieldLine();

		public abstract string GetParseLine();

		public abstract string GetInitLine();
	}
}