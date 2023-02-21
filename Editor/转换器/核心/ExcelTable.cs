using System.Collections.Generic;
using OfficeOpenXml;

namespace JFramework
{
    using DataDict = Dictionary<int, string>;
    
    internal class ExcelTable
    {
        private readonly Dictionary<int, DataDict> itemDict;
        public readonly string fileName;
        public int columnCount;
        public int rowCount;

        public ExcelTable(ExcelWorksheet sheet)
        {
            itemDict = new Dictionary<int, DataDict>();
            fileName = sheet.Name;

            if (sheet.Dimension != null)
            {
                rowCount = sheet.Dimension.Rows;
                columnCount = sheet.Dimension.Columns;
            }
            else
            {
                rowCount = 0;
                columnCount = 0;
            }

            for (var row = 0; row < rowCount; row++)
            {
                for (var column = 0; column < columnCount; column++)
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
            if (rowCount < row) rowCount = row + 1;
            if (columnCount < column) columnCount = column + 1;

            if (!itemDict.TryGetValue(row, out DataDict data))
            {
                data = new DataDict();
                itemDict.Add(row, data);
            }

            if (!data.TryGetValue(column, out var item))
            {
                item = value;
                data.Add(column, item);
            }
        }

        public string GetData(int row, int column)
        {
            if (!itemDict.TryGetValue(row, out DataDict data)) return null;
            return data.TryGetValue(column, out var item) ? item : null;
        }

        public string GetValue(int row, int column)
        {
            if (row < 0 || column < 0) return null;
            var item = GetData(row, column);
            return item;
        }
    }
}