#if UNITY_EDITOR
using System;
using System.IO;
using OfficeOpenXml;
using UnityEngine;

namespace JYJFramework
{
    public static class ExcelManager
    {
        private const string extra = ".xlsx";

        public static void Writer(string excelPath, string sheetName, Action<ExcelWorksheet> callback)
        {
            FileInfo fileInfo = new FileInfo(excelPath + extra);
            using ExcelPackage excelPackage = new ExcelPackage(fileInfo);
            if (!File.Exists(excelPath + extra))
            {
                Debug.LogWarning("路径中不存在Excel文件");
            }
            else
            {
                if (excelPackage.Workbook.Worksheets[sheetName] == null)
                {
                    Debug.LogWarning("Excel中不存在" + sheetName + "表格");
                }
                else
                {
                    ExcelWorksheet sheet = excelPackage.Workbook.Worksheets[sheetName];
                    callback?.Invoke(sheet);
                    excelPackage.Save();
                    Debug.Log("写入成功---" + sheet);
                    return;
                }
            }

            Debug.LogWarning("无法写入不存在的表格！");
        }

        public static ExcelWorksheet Reader(string excelPath, string sheetName)
        {
            FileInfo fileInfo = new FileInfo(excelPath + extra);
            using ExcelPackage excelPackage = new ExcelPackage(fileInfo);
            if (!File.Exists(excelPath + extra))
            {
                Debug.LogWarning("路径中不存在Excel文件");
            }
            else
            {
                if (excelPackage.Workbook.Worksheets[sheetName] == null)
                {
                    Debug.LogWarning("Excel中不存在" + sheetName + "表格");
                }
                else
                {
                    ExcelWorksheet sheet = excelPackage.Workbook.Worksheets[sheetName];
                    Debug.Log("读取成功---" + sheet);
                    return sheet;
                }
            }

            return null;
        }

        public static void Create(string excelPath, string sheetName)
        {
            FileInfo fileInfo = new FileInfo(excelPath + extra);
            using ExcelPackage excelPackage = new ExcelPackage(fileInfo);
            if (!File.Exists(excelPath + extra))
            {
                ExcelWorksheet sheet = excelPackage.Workbook.Worksheets.Add(sheetName);
                excelPackage.Save();
                Debug.Log("创建成功---" + sheet);
            }
            else if (excelPackage.Workbook.Worksheets[sheetName] == null)
            {
                ExcelWorksheet sheet = excelPackage.Workbook.Worksheets.Add(sheetName);
                excelPackage.Save();
                Debug.Log("创建成功---" + sheet);
            }
            else
            {
                Debug.LogWarning("Excel中已存在表格---" + sheetName);
            }
        }

        public static void Delete(string excelPath, string sheetName)
        {
            FileInfo fileInfo = new FileInfo(excelPath + extra);
            using ExcelPackage excelPackage = new ExcelPackage(fileInfo);
            if (!File.Exists(excelPath + extra))
            {
                Debug.LogWarning("路径中不存在Excel文件");
            }
            else
            {
                if (excelPackage.Workbook.Worksheets[sheetName] == null)
                {
                    Debug.LogWarning("Excel中不存在表格---" + sheetName);
                    return;
                }

                if (excelPackage.Workbook.Worksheets.Count > 1)
                {
                    excelPackage.Workbook.Worksheets.Delete(sheetName);
                    excelPackage.Save();
                    Debug.Log("删除成功---" + sheetName);
                }
                else
                {
                    Debug.LogWarning("请至少保留一个表格！");
                }
            }
        }
    }
}
#endif