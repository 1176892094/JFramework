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
    public static class ParseGenerator
    {
        public static void GenerateScripts()
        {
            var filesPath = Directory.GetFiles(ParseSetting.PathDataKey);
            var excelsPath = filesPath.Where(ParseSetting.IsSupported).ToArray();
            var scriptContents = new List<(string, string)>();
            foreach (var excelPath in excelsPath)
            {
                scriptContents.AddRange(GenerateScriptText(excelPath)); //所有Excel表格
            }

            foreach (var (sheetName, scriptText) in scriptContents)
            {
                var csFileSavePath = ParseSetting.GetScriptPath(sheetName);
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

            AssetDatabase.Refresh();
            GenerateAssets();
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
                    var row = sheet.Dimension.End.Row;
                    var column = sheet.Dimension.End.Column;
                    var nameList = new List<int>();
                    for (int i = 1; i <= column; i++)
                    {
                        if (sheet.Cells[Const.Name, i].Value != null)
                        {
                            nameList.Add(i);
                        }
                    }

                    if (nameList.Count == 0) continue;

                    var typeList = new List<int>();
                    foreach (var type in nameList)
                    {
                        var value = sheet.Cells[Const.Type, type].Value?.ToString();
                        if (!value.IsEmpty() && value is Const.Enum or Const.Struct)
                        {
                            typeList.Add(type);
                        }
                    }

                    if (typeList.Count == 0) continue;

                    var writer = string.Empty;
                    foreach (var x in typeList)
                    {
                        var dataList = new List<string>();
                        for (int y = Const.Data; y <= row; y++)
                        {
                            var field = sheet.Cells[y, x].Value;
                            var value = field?.ToString();
                            if (!value.IsEmpty())
                            {
                                dataList.Add(value);
                            }
                        }

                        var name = sheet.Cells[Const.Name, x].Value.ToString();
                        var type = sheet.Cells[Const.Type, x].Value.ToString();
                        writer += WriteEnumOrStruct(name, type, dataList);
                    }

                    var names = sheet.Cells[Const.Name, 1, Const.Name, column].Select(_ => _.Value?.ToString())
                        .ToArray();
                    var types = sheet.Cells[Const.Type, 1, Const.Type, column].Select(_ => _.Value?.ToString())
                        .ToArray();
                    var fileContent = WriteScriptsFile(sheet.Name, types, names, writer);
                    contents.Add((sheet.Name, fileContent));
                }

                return contents;
            }
        }

        private static string WriteScriptsFile(string className, string[] types, string[] names, string value)
        {
            var builder = new StringBuilder(1024);
            var dataName = ParseSetting.GetDataName(className);
            var tableName = ParseSetting.GetTableName(className);
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
                var parser = ParseSetting.Parse(name, type);
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

        private static void GenerateAssets()
        {
            var filesPath = Directory.GetFiles(ParseSetting.PathDataKey);
            var excelsPath = filesPath.Where(ParseSetting.IsSupported).ToArray();
            if (!Directory.Exists(Const.AssetsPath))
            {
                Directory.CreateDirectory(Const.AssetsPath);
            }

            foreach (var excelPath in excelsPath)
            {
                var excelData = GetExcelData(excelPath);
                foreach (var (sheetName, sheetData) in excelData)
                {
                    var tableName = ParseSetting.GetTableFullName(sheetName);

                    var dataTable = ScriptableObject.CreateInstance(tableName);
                    if (dataTable == null)
                    {
                        Debug.LogWarning($"创建 {tableName.Red()} 失败!");
                        continue;
                    }

                    var fullName = ParseSetting.GetDataFullName(sheetName);
                    var dataType = ParseSetting.GetTypeByString(fullName);

                    var constructor = dataType.GetConstructor(new[] { typeof(string[,]), typeof(int), typeof(int) });
                    if (constructor == null) return;
                    var row = sheetData.GetLength(0);
                    for (var y = 0; y < row; ++y)
                    {
                        if (sheetData[y, 0].IsEmpty()) continue;
                        var data = (IData)constructor.Invoke(new object[] { sheetData, y, 0 });
                        var key = DataManager.GetKeyField(data);
                        if (key is 0 or "") continue;
                        ((IDataTable)dataTable).AddData(data);
                    }

                    var soSavePath = ParseSetting.GetAssetsPath(sheetName);
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
                    var nameList = new List<int>();
                    for (int i = 1; i <= column; i++)
                    {
                        if (sheet.Cells[Const.Name, i].Value != null)
                        {
                            nameList.Add(i);
                        }
                    }

                    if (nameList.Count == 0) continue;

                    var typeList = new List<int>();
                    foreach (var type in nameList)
                    {
                        if (sheet.Cells[Const.Type, type].Value != null)
                        {
                            typeList.Add(type);
                        }
                    }

                    var dataList = new List<int>();
                    for (int i = Const.Data; i <= row; i++)
                    {
                        if (sheet.Cells[i, 1].Value != null)
                        {
                            dataList.Add(i);
                        }
                    }
                    
                    if (typeList.Count == 0) continue;

                    var data = new string[row, typeList.Count];
                    for (int y = 0; y < dataList.Count; y++)
                    {
                        for (int x = 0; x < typeList.Count; x++)
                        {
                            var field = sheet.Cells[dataList[y], typeList[x]].Value;
                            var value = field?.ToString();
                            data[y, x] = value;
                        }
                    }

                    results.Add((sheet.Name, data));
                }

                return results;
            }
        }
    }
}