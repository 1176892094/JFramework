using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using UnityEngine;

namespace JFramework
{
    internal class ExcelSource
    {
        public readonly List<ExcelTable> tableList = new List<ExcelTable>();

        private ExcelSource(ExcelWorkbook workbook)
        {
            try
            {
                foreach (var table in workbook.Worksheets)
                {
                    tableList.Add(new ExcelTable(table));
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }

        public static ExcelSource Load(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    Debug.LogError("无法找到Excel文件:" + path);
                    return null;
                }

                var fileInfo = new FileInfo(path);
                using ExcelPackage excelPackage = new ExcelPackage(fileInfo);
                var source = new ExcelSource(excelPackage.Workbook);
                return source;
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }

            return null;
        }
    }
}