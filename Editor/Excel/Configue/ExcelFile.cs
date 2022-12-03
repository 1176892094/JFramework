using System;
using System.Collections.Generic;
using System.IO;
using JFramework.Basic;
using OfficeOpenXml;

namespace JFramework.Excel
{
    internal class ExcelFile
    {
        public readonly List<ExcelSheet> sheetList = new List<ExcelSheet>();

        private ExcelFile(ExcelWorkbook workbook)
        {
            try
            {
                foreach (var sheet in workbook.Worksheets)
                {
                    sheetList.Add(new ExcelSheet(sheet));
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
            }
        }

        public static ExcelFile Load(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    Logger.LogError("无法找到Excel文件:" + path);
                    return null;
                }

                FileInfo fileInfo = new FileInfo(path);
                using ExcelPackage excelPackage = new ExcelPackage(fileInfo);
                ExcelFile file = new ExcelFile(excelPackage.Workbook);
                return file;
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
            }

            return null;
        }
    }
}