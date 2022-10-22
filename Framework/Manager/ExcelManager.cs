using System;
using System.IO;
using OfficeOpenXml;
using UnityEngine;

namespace JYJFramework
{
    public static class ExcelManager
    {
        public static void Writer(string excelPath, int sheetId, Action<ExcelWorksheet> callback)
        {
            FileInfo fileInfo = new FileInfo(excelPath);
            using ExcelPackage excelPackage = new ExcelPackage(fileInfo);
            if (File.Exists(excelPath))
            {
                if (excelPackage.Workbook.Worksheets.Count > sheetId)
                {
                    ExcelWorksheet sheet = excelPackage.Workbook.Worksheets[sheetId];
                    if (sheet != null)
                    {
                        callback?.Invoke(sheet);
                        excelPackage.Save();
                        Debug.Log("写入成功！");
                        return;
                    }
                }
            }

            Debug.LogWarning("无法写入不存在的表格！");
        }

        public static ExcelWorksheet Reader(string excelPath, int sheetId = 0)
        {
            FileInfo fileInfo = new FileInfo(excelPath);
            using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
            {
                if (File.Exists(excelPath))
                {
                    if (excelPackage.Workbook.Worksheets.Count > sheetId)
                    {
                        ExcelWorksheet sheet = excelPackage.Workbook.Worksheets[sheetId];
                        Debug.Log("读取成功---"+sheet);
                        return sheet;
                    }
                }
            }

            Debug.LogWarning("无法读取不存在的表格！");
            return null;
        }

        public static void Create(string excelPath, string sheetName)
        {
            FileInfo fileInfo = new FileInfo(excelPath);
            using ExcelPackage excelPackage = new ExcelPackage(fileInfo);
            if (!File.Exists(excelPath))
            {
                ExcelWorksheet sheet = excelPackage.Workbook.Worksheets.Add(sheetName);
                excelPackage.Save();
                Debug.Log("创建成功---" + sheet);
            }
            else
            {
                Debug.LogWarning("无法创建相同名称的表格---" + sheetName);
            }
        }

        public static void Delete(string excelPath, string sheetName)
        {
            FileInfo fileInfo = new FileInfo(excelPath);
            using ExcelPackage excelPackage = new ExcelPackage(fileInfo);
            if (File.Exists(excelPath))
            {
                if (excelPackage.Workbook.Worksheets.Count > 1)
                {
                    excelPackage.Workbook.Worksheets.Delete(sheetName);
                    excelPackage.Save();
                    Debug.Log("删除成功---" + sheetName);
                }
                else
                {
                    Debug.LogWarning("请至少保留一个表格---" + sheetName);
                }
            }
            else
            {
                Debug.LogWarning("无法删除不存在的表格---" + sheetName);
            }
        }
    }
}
