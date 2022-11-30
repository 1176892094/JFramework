using System.Collections.Generic;
using OfficeOpenXml;

namespace JFramework.Excel
{
    internal class ExcelSheet
    {
        private readonly Dictionary<int, Dictionary<int, ExcelItem>> itemDict = new Dictionary<int, Dictionary<int, ExcelItem>>();
        public readonly string fileName;
        public int ColumnCount;
        public int RowCount;

        public ExcelSheet(ExcelWorksheet sheet)
        {
            fileName = sheet.Name;
            
            if (sheet.Dimension != null)
            {
                RowCount = sheet.Dimension.Rows;
                ColumnCount = sheet.Dimension.Columns;
            }
            else
            {
                RowCount = 0;
                ColumnCount = 0;
            }

            for (var row = 0; row < RowCount; row++)
            {
                for (var column = 0; column < ColumnCount; column++)
                {
                    var data = sheet.Cells[row + 1, column + 1].Value;
                    var value = data == null ? "" : data.ToString();
                    SetData(row, column, value);
                }
            }
        }

        private void SetData(int row, int column, string value)
        {
            if (row < 0 || column < 0) return;
            if (RowCount < row) RowCount = row + 1;
            if (ColumnCount < column) ColumnCount = column + 1;

            if (!itemDict.TryGetValue(row, out var data))
            {
                data = new Dictionary<int, ExcelItem>();
                itemDict.Add(row, data);
            }

            if (!data.TryGetValue(column, out var item))
            {
                item = new ExcelItem(row, column, value);
                data.Add(column, item);
            }
        }

        public string GetData(int row, int column)
        {
            if (row < 0 || column < 0) return null;
            ExcelItem item = GetItem(row, column);
            return item?.value;
        }

        public ExcelItem GetItem(int row, int column)
        {
            if (itemDict.TryGetValue(row, out var data))
            {
                if (data.TryGetValue(column, out var item))
                {
                    return item;
                }
            }

            return null;
        }
    }
}