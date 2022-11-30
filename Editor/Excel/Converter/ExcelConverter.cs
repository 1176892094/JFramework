using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace JFramework.Excel
{
    internal static partial class ExcelConverter
    {
        public const string excelPathKey = "EasyExcelExcelPath";
        public const string csChangedKey = "EasyExcelCSChanged";
        private static bool isComplete;

        private static ExcelData GetSheetData(ExcelSheet sheet)
        {
            ExcelData excelData = new ExcelData();
            for (var i = 0; i < sheet.RowCount; i++)
            {
                List<string> rowData = new List<string>();
                for (var j = 0; j < sheet.ColumnCount; j++)
                {
                    ExcelItem item = sheet.GetItem(i, j);
                    rowData.Add(item != null ? item.value : "");
                }

                excelData.ExcelSheet.Add(rowData);
            }

            excelData.RowCount = sheet.RowCount;
            excelData.ColumnCount = sheet.ColumnCount;
            return excelData;
        }

        private static ExcelData RemoveEmptyColumn(ExcelSheet sheet)
        {
            List<int> nameList = new List<int>();
            for (var column = 0; column < sheet.ColumnCount; column++)
            {
                string name = sheet.GetData(ExcelSetting.Instance.NameIndex, column);
                if (!string.IsNullOrEmpty(name))
                {
                    nameList.Add(column);
                }
            }

            List<int> typeList = new List<int>();
            foreach (var column in nameList)
            {
                string type = sheet.GetData(ExcelSetting.Instance.TypeIndex, column);
                if (ExcelValueParser.IsSupportedType(type))
                {
                    typeList.Add(column);
                }
            }

            ExcelData excelData = new ExcelData();
            for (var i = 0; i < sheet.RowCount; i++)
            {
                List<string> rowData = new List<string>();
                foreach (var c in typeList)
                {
                    ExcelItem item = sheet.GetItem(i, c);
                    rowData.Add(item != null ? item.value : "");
                }

                excelData.ExcelSheet.Add(rowData);
            }

            excelData.RowCount = sheet.RowCount;
            excelData.ColumnCount = typeList.Count;
            return excelData;
        }

        private static bool IsConvert(ExcelSheet sheet)
        {
            if (sheet == null || sheet.RowCount <= ExcelSetting.Instance.TypeIndex || sheet.ColumnCount < 1) return false;
            int columnCount = 0;
            for (int column = 0; column < sheet.ColumnCount; column++)
            {
                string type = sheet.GetData(ExcelSetting.Instance.TypeIndex, column);
                if (string.IsNullOrEmpty(type) || type.Equals(" ") || type.Equals("\r")) continue;

                if (ExcelValueParser.IsSupportedType(type))
                {
                    string varName = sheet.GetData(ExcelSetting.Instance.NameIndex, column);
                    if (!string.IsNullOrEmpty(varName))
                    {
                        columnCount++;
                    }
                }
            }

            return columnCount > 0;
        }
        
        private static void UpdateProgress(int progress, int progressMax)
        {
            string title = "ExcelData importing...[" + progress + " / " + progressMax + "]";
            float value = progress / (float)progressMax;
            EditorUtility.DisplayProgressBar(title, "", value);
            isComplete = true;
        }

        private static void ClearProgress()
        {
            if (!isComplete) return;
            try
            {
                EditorUtility.ClearProgressBar();
            }
            catch (Exception)
            {
                // ignored
            }

            isComplete = false;
        }

        private class ExcelData
        {
            private readonly List<List<string>> excelSheet = new List<List<string>>();
            public int ColumnCount;
            public int RowCount;

            public List<List<string>> ExcelSheet => excelSheet;

            public string GetData(int row, int column)
            {
                return excelSheet[row][column];
            }

            public void SetData(int row, int column, string value)
            {
                excelSheet[row][column] = value;
            }
        }
    }
}