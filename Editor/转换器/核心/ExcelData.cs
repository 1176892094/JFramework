namespace JFramework
{
    internal class ExcelData
    {
        public readonly int row;
        public readonly int column;
        public readonly string value;

        public ExcelData(int row, int column, string value)
        {
            this.row = row;
            this.column = column;
            this.value = value;
        }
    }
}