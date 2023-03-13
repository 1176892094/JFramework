using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using JFramework.Core;
using OfficeOpenXml;
using UnityEditor;
using UnityEngine;

namespace JFramework
{
    public static class ExcelGenerator
    {
        public static void GenerateAsset()
        {
            AssetDatabase.Refresh();
            var filesPath = Directory.GetFiles(ExcelSetting.PathDataKey);
            var excelsPath = filesPath.Where(ExcelSetting.IsSupported).ToArray();
            if (!Directory.Exists(Const.AssetsPath))
            {
                Directory.CreateDirectory(Const.AssetsPath);
            }

            foreach (var excelPath in excelsPath)
            {
                var excelData = GetExcelData(excelPath);
                foreach (var (sheetName, sheetData) in excelData)
                {
                    var tableName = ExcelSetting.GetTableFullName(sheetName);
                    
                    var dataTable = ScriptableObject.CreateInstance(tableName);
                    if (dataTable == null)
                    {
                        Debug.LogWarning($"创建 {tableName} 失败!");
                        continue;
                    }
                    
                    var fullName = ExcelSetting.GetDataFullName(sheetName);
                    var dataType = ExcelSetting.GetTypeByString(fullName);

                    var constructor = dataType.GetConstructor(new[] { typeof(string[,]), typeof(int), typeof(int) });
                    if (constructor == null) return;
                    var row = sheetData.GetLength(0);
                    for (var y = Const.Data - 1; y < row; ++y)
                    {
                        var data = (IData)constructor.Invoke(new object[] { sheetData, y, 0 });
                        var key = DataManager.KeyValue(data);
                        if (key is 0 or "") continue;
                        ((IDataTable)dataTable).AddData(data);
                    }

                    var soSavePath = ExcelSetting.GetAssetsPath(sheetName);
                    AssetDatabase.CreateAsset(dataTable, soSavePath);
                }

                AssetDatabase.Refresh();
            }
        }

        private static List<(string, string[,])> GetExcelData(string excelPath)
        {
            var fileInfo = new FileInfo(excelPath);
            using var excelPackage = new ExcelPackage(fileInfo);
            {
                var workbook = excelPackage.Workbook;
                var sheets = workbook.Worksheets;
                var results = new List<(string, string[,])>();
                foreach (var sheet in sheets)
                {
                    var row = sheet.Dimension.End.Row;
                    var column = sheet.Dimension.End.Column;
                    var names = sheet.Cells[Const.Name, 1, Const.Name, column].Select(_ => _.Value?.ToString()).ToArray();
                    var types = sheet.Cells[Const.Type, 1, Const.Type, column].Select(_ => _.Value?.ToString()).ToArray();
                    var data = new string[row, column];
                    for (var y = 1; y <= row; y++)
                    {
                        for (var x = 1; x <= column; x++)
                        {
                            var value = sheet.Cells[y, x].Value;
                            var valueStr = value?.ToString();
                            data[y - 1, x - 1] = valueStr;
                        }
                    }

                    if (names.Length == 0 || types.Length == 0) continue;
                    results.Add((sheet.Name, data));
                }

                return results;
            }
        }
        
        public static void GenerateCode()
        {
            var filesPath = Directory.GetFiles(ExcelSetting.PathDataKey);
            var excelsPath = filesPath.Where(ExcelSetting.IsSupported).ToArray();
            var scriptContents = new List<(string, string)>();
            foreach (var excelPath in excelsPath)
            {
                scriptContents.AddRange(GenerateScriptText(excelPath)); //所有Excel表格
            }

            foreach (var (sheetName, scriptText) in scriptContents)
            {
                var csFileSavePath = ExcelSetting.GetScriptPath(sheetName);
                if (!Directory.Exists(Const.ScriptPath))
                {
                    Directory.CreateDirectory(Const.ScriptPath);
                }

                if (!File.Exists(csFileSavePath))
                {
                    File.Create(csFileSavePath).Close();
                }

                File.WriteAllText(csFileSavePath, scriptText);
            }

            ExcelSetting.SaveDataKey = true;
            AssetDatabase.Refresh();
        }

        private static IEnumerable<(string, string)> GenerateScriptText(string excelPath)
        {
            var fileInfo = new FileInfo(excelPath);
            using var excelPackage = new ExcelPackage(fileInfo);
            {
                var workbook = excelPackage.Workbook;
                var sheets = workbook.Worksheets;
                var contents = new List<(string, string)>();
                foreach (var sheet in sheets)
                {
                    var writer = string.Empty;
                    var row = sheet.Dimension.End.Row;
                    var column = sheet.Dimension.End.Column;
                    var names = sheet.Cells[Const.Name, 1, Const.Name, column].Select(_ => _.Value?.ToString()).ToArray();
                    var types = sheet.Cells[Const.Type, 1, Const.Type, column].Select(_ => _.Value?.ToString()).ToArray();
                    if (names.Length == 0 || types.Length == 0)
                    {
                        Debug.Log($"跳过 {sheet.Name} 表生成！");
                        continue;
                    }
                    
                    Debug.Log($"生成 {sheet.Name} 表成功。");

                    for (int x = 1; x <= column; x++)
                    {
                        if (sheet.Cells[Const.Type, x].Value != null)
                        {
                            var name = sheet.Cells[Const.Name, x].Value.ToString();
                            var type = sheet.Cells[Const.Type, x].Value.ToString();
                            if (!type.IsEmpty() && type is Const.Enum or Const.Struct)
                            {
                                var valueList = new List<string>();
                                for (int y = Const.Data; y <= row; y++)
                                {
                                    var data = sheet.Cells[y, x].Value?.ToString();
                                    if (data != null)
                                    {
                                        valueList.Add(data);
                                    }
                                }

                                writer = WriteEnumOrStruct(name, type, valueList);
                            }
                        }
                    }

                    var fileContent = WriteScriptsFile(sheet.Name, types, names, writer);
                    contents.Add((sheet.Name, fileContent));
                }

                return contents;
            }
        }

        private static string WriteScriptsFile(string className, string[] types, string[] names, string value)
        {
            var builder = new StringBuilder(1024);
            var dataName = ExcelSetting.GetDataName(className);
            var tableName = ExcelSetting.GetTableName(className);
            builder.Append("using System;\n");
            builder.Append("using UnityEngine;\n\n");
            builder.AppendFormat("namespace {0}\n", Const.Namespace);
            builder.Append("{\n");
            builder.AppendFormat("\tpublic class {0} : DataTable<{1}> {{ }}\n\n", tableName, dataName);
            builder.Append("\t[Serializable]\n");
            builder.AppendFormat("\tpublic struct {0} : IData" + "\n", dataName);
            builder.Append("\t{\n");

            var count = types.Length;
            var columnField = new IParser[count];
            for (var i = 0; i < count; i++)
            {
                var name = names.ElementAt(i);
                var type = types.ElementAt(i);
                if (type == null) continue;
                var parser = ExcelSetting.Parse(name, type);
                columnField[i] = parser;
            }

            for (var i = 0; i < count; i++)
            {
                var field = columnField[i];
                if (field == null) continue;
                builder.Append(field.GetField());
            }

            builder.Append("\n#if UNITY_EDITOR\n");
            builder.AppendFormat("\t\tpublic {0}(string[,] sheet, int row, int column)\n", dataName);
            builder.Append("\t\t{\n");

            for (var i = 0; i < count; i++)
            {
                var field = columnField[i];
                if (field == null) continue;
                builder.Append(field.GetParse());
            }

            builder.Append("\t\t}\n");
            builder.Append("#endif\n");

            builder.Append("\t}\n");
            builder.Append(value);
            builder.Append("}\n");
            return builder.ToString();
        }
        
        private static string WriteEnumOrStruct(string name, string type, List<string> writer)
        {
            var builder = new StringBuilder(1024);
            builder.AppendFormat("\n\tpublic {0} {1}\n", type, name);
            builder.Append("\t{\n");
            foreach (var value in writer)
            {
                var data = value.Split(',');
                switch (type)
                {
                    case Const.Enum when data.Length != 2:
                        return "";
                    case Const.Enum:
                        builder.AppendFormat("\t\t{0} = {1},\n", data[0], data[1]);
                        break;
                    case Const.Struct when data.Length != 3:
                        return "";
                    case Const.Struct:
                        builder.AppendFormat("\t\tpublic const {0} {1} = {2};\n", data[0], data[1], data[2]);
                        break;
                }
            }

            builder.Append("\t}\n");

            return builder.ToString();
        }
    }
}