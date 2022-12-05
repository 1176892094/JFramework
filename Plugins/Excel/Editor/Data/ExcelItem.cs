namespace JFramework.Excel
{
	internal class ExcelItem
	{
		public readonly int row;
		public readonly int column;
		public readonly string value;

		public ExcelItem(int row, int column, string value)
		{
			this.row = row;
			this.column = column;
			this.value = value;
		}
	}
}