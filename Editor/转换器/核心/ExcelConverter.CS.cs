using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace JFramework
{
    internal static partial class ExcelConverter
    {
        public static void ConvertToScript(string filePath, string csPath)
        {
            try
            {
                filePath = filePath.Replace("\\", "/");
                csPath = csPath.Replace("\\", "/");

                if (!Directory.Exists(filePath))
                {
                    EditorUtility.DisplayDialog("JFramework Tool", "没有找到Excel文件!", "OK");
                    return;
                }

                if (!Directory.Exists(csPath))
                {
                    Directory.CreateDirectory(csPath);
                }

                var tempPath = Environment.CurrentDirectory + "/Template/";
                if (Directory.Exists(tempPath))
                {
                    Directory.Delete(tempPath, true);
                }

                Directory.CreateDirectory(tempPath);

                filePath = filePath.Replace("\\", "/");
                csPath = csPath.Replace("\\", "/");
                if (!csPath.EndsWith("/")) csPath += "/";

                var isChanged = false;
                var excelFiles = Directory.GetFiles(filePath);

                for (var i = 0; i < excelFiles.Length; ++i)
                {
                    var excelFile = excelFiles[i].Replace("\\", "/");
                    if (i + 1 < excelFiles.Length)
                    {
                        UpdateProgress(i + 1, excelFiles.Length);
                    }
                    else
                    {
                        RemoveProgress();
                    }

                    if (!IsSupportedExcel(excelFile)) continue;
                    var fileName = Path.GetFileName(excelFile);
                    var scriptData = ConvertScriptData(excelFile);

                    foreach (var data in scriptData)
                    {
                        var csName = GetScriptName(data.Key);
                        var csTempPath = tempPath + csName;
                        var csFilePath = csPath + csName;

                        bool isRewrite = true;
                        if (File.Exists(csFilePath))
                        {
                            var oldFile = File.ReadAllText(csFilePath);
                            isRewrite = oldFile != data.Value;
                        }

                        if (!isRewrite) continue;
                        isChanged = true;
                        File.WriteAllText(csTempPath, data.Value, Encoding.UTF8);
                    }
                }

                FieldBuilder.Reset();

                if (isChanged)
                {
                    EditorPrefs.SetBool(EditorConst.ExcelDataKey, true);
                    var csFiles = Directory.GetFiles(tempPath);

                    foreach (var csFile in csFiles)
                    {
                        var path = csFile.Replace("\\", "/");
                        File.Copy(csFile, csPath + path.Substring(path.LastIndexOf("/", StringComparison.Ordinal)),
                            true);
                    }

                    AssetDatabase.Refresh();
                    Debug.Log("脚本已生成,正在创建资源.");
                }
                else
                {
                    Debug.Log("没有改变的脚本文件,开始创建资源.");
                    RemoveProgress();

                    var savePath = EditorPrefs.GetString(EditorConst.ExcelPathKey);
                    if (!string.IsNullOrEmpty(savePath))
                    {
                        ConvertToObject(savePath, Environment.CurrentDirectory + "/" + EditorConst.AssetsPath);
                    }
                }

                if (Directory.Exists(tempPath))
                {
                    Directory.Delete(tempPath, true);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
                EditorPrefs.SetBool(EditorConst.ExcelDataKey, false);
                RemoveProgress();
                AssetDatabase.Refresh();
            }
        }

        private static Dictionary<string, string> ConvertScriptData(string excelPath)
        {
            var tempFile = new Dictionary<string, string>();
            var dataFile = ExcelSource.Load(excelPath);
            if (dataFile == null) return tempFile;
            var fileName = Path.GetFileName(excelPath);
            foreach (var table in dataFile.tableList)
            {
                if (table == null) continue;
                if (!IsConvertTable(table))
                {
                    Debug.Log($"{fileName}跳过生成表:{table.fileName}.");
                    continue;
                }

                var tableData = GetTableData(table);
                var csFile = ConvertScript(tableData, table.fileName, fileName);
                tempFile.Add(table.fileName, csFile);
            }

            return tempFile;
        }

        private static string ConvertScript(ExcelData excelData, string sheetName, string fileName)
        {
            try
            {
                var containerName = GetTableName(sheetName);
                var csFile = new StringBuilder(2048);
                csFile.Append("using System;\n");
                csFile.Append("using System.Collections.Generic;\n");
                csFile.Append("using UnityEngine;\n");
                csFile.Append("using JFramework.Interface;\n");
                csFile.AppendFormat("namespace {0}\n", EditorConst.Namespace);
                csFile.Append("{\n");
                csFile.Append("\t[Serializable]\n");
                csFile.Append("\tpublic class " + sheetName + " : Data\n");
                csFile.Append("\t{\n");

                int columnCount = excelData.Column;
                List<int> enumList = new List<int>();
                DataField[] columnFields = new DataField[columnCount];
                for (int i = 0; i < columnCount; i++)
                {
                    string name = excelData.GetData(EditorConst.Name, i);
                    string type = excelData.GetData(EditorConst.Type, i);
                    if (type == "enum") enumList.Add(i);
                    DataField data = FieldParser.Parse(i, name, type);
                    columnFields[i] = data;
                }

                for (var i = 0; i < columnCount; i++)
                {
                    var columnField = columnFields[i];
                    if (columnField == null) continue;
                    csFile.Append(columnField.GetFieldLine());
                }

                csFile.Append("\n#if UNITY_EDITOR\n");
                csFile.AppendFormat("\t\tpublic {0}(List<List<string>> sheet, int row, int column)\n", sheetName);
                csFile.Append("\t\t{\n");
                for (var i = 0; i < columnCount; i++)
                {
                    var columnField = columnFields[i];
                    if (columnField == null) continue;
                    csFile.Append(columnField.GetParseLine());
                }

                csFile.Append("\t\t}\n#endif\n");

                csFile.Append("\t\tpublic override void InitData()\n");
                csFile.Append("\t\t{\n");
                for (var i = 0; i < columnCount; i++)
                {
                    var columnField = columnFields[i];
                    if (columnField == null) continue;
                    csFile.Append(columnField.GetInitLine());
                }

                csFile.Append("\t\t}\n");
                csFile.Append("\t}\n\n");

                csFile.AppendFormat("\tpublic class {0} : DataTable\n", containerName);
                csFile.Append("\t{\n");
                csFile.AppendFormat("\t\t[SerializeField]\n\t\tprivate List<{0}> dataList = new List<{0}>();\n\n", sheetName);

                csFile.Append("\t\tpublic override void InitData()\n\t\t{\n");
                csFile.Append("\t\t\tforeach (var data in dataList)\n");
                csFile.Append("\t\t\t{\n");
                csFile.Append("\t\t\t\tdata.InitData();\n");
                csFile.Append("\t\t\t}\n");
                csFile.Append("\t\t}\n");

                csFile.Append("\t\tpublic override void AddData(Data data)\n");
                csFile.Append("\t\t{\n");
                csFile.AppendFormat("\t\t\tdataList.Add(({0})data);\n", sheetName);
                csFile.Append("\t\t}\n\n");

                csFile.Append("\t\tpublic override Data GetData(int index)\n");
                csFile.Append("\t\t{\n");
                csFile.Append("\t\t\treturn dataList[index];\n");
                csFile.Append("\t\t}\n\n");

                csFile.Append("\t\tpublic override int GetCount()\n");
                csFile.Append("\t\t{\n");
                csFile.Append("\t\t\treturn dataList.Count;\n");
                csFile.Append("\t\t}\n");

                csFile.Append("\t}\n\n");

                foreach (var e in enumList)
                {
                    string name = excelData.GetData(EditorConst.Name, e);
                    csFile.Append("\tpublic enum " + name + "\n");
                    csFile.Append("\t{\n");

                    int rowCount = excelData.Row;
                    for (int i = 2; i < rowCount; i++)
                    {
                        var field = excelData.GetData(i, e);
                        csFile.AppendFormat("\t\t{0},\n", field);
                    }

                    csFile.Append("\t}\n\n");
                }

                csFile.Append("}\n");

                return csFile.ToString();
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.ToString());
            }

            return "";
        }
    }
}