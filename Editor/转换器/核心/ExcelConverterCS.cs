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

                if (!Directory.Exists(filePath)) return;
                if (!Directory.Exists(csPath)) Directory.CreateDirectory(csPath);
                
                var tempPath = Environment.CurrentDirectory + "/Template/";
                if (Directory.Exists(tempPath)) Directory.Delete(tempPath, true);
                Directory.CreateDirectory(tempPath);

                filePath = filePath.Replace("\\", "/");
                csPath = csPath.Replace("\\", "/");
                if (!csPath.EndsWith("/")) csPath += "/";

                var isChanged = false;
                //获取目录下所有的Excel文件
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

                ExcelBuilder.Reset();

                if (isChanged)
                {
                    FrameworkEditor.DataKey = true;
                    var csFiles = Directory.GetFiles(tempPath);

                    foreach (var csFile in csFiles)
                    {
                        var path = csFile.Replace("\\", "/");
                        var desFileName = csPath + path.Substring(path.LastIndexOf("/", StringComparison.Ordinal));
                        File.Copy(csFile, desFileName, true);
                    }

                    AssetDatabase.Refresh();
                    Debug.Log("脚本已生成,正在创建资源.");
                }
                else
                {
                    Debug.Log("没有改变的脚本文件,开始创建资源.");
                    RemoveProgress();

                    var savePath = FrameworkEditor.PathKey;
                    if (!string.IsNullOrEmpty(savePath))
                    {
                        ConvertToObject(savePath, FrameworkEditor.AssetsPath);
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
                FrameworkEditor.DataKey = false;
                RemoveProgress();
                AssetDatabase.Refresh();
            }
        }
        /// <summary>
        /// 将给定Excel文件转换为脚本文件
        /// 也就是通过读取Excel的前几列,生成对应的C#类
        /// </summary>
        /// <param name="excelPath"></param>
        /// <returns></returns>
        private static Dictionary<string, string> ConvertScriptData(string excelPath)
        {
            var tempFile = new Dictionary<string, string>();
            var dataFile = ExcelSource.Load(excelPath);
            if (dataFile == null) return tempFile;
            var fileName = Path.GetFileName(excelPath);
            //对Excel文件中的每个表进行遍历
            foreach (var table in dataFile.tableList)
            {
                if (table == null) continue;
                //判断当前表能否被转换
                ExcelBuilder.isClass = false;
                if (!IsConvertTable(table))
                {
                    Debug.Log($"{fileName}跳过生成表:{table.fileName}.");
                    continue;
                }

                var tableData = GetTableData(table);
                var csFile = ConvertScript(tableData, table.fileName);
                tempFile.Add(table.fileName, csFile);
            }

            return tempFile;
        }

        private static string ConvertScript(ExcelData excelData, string sheetName)
        {
            try
            {
                var dataName = GetDataName(sheetName);
                var tableName = GetTableName(sheetName);
                var csFile = new StringBuilder(2048);
                csFile.Append("//------------------------------------------------------------------------------\n");
                csFile.Append("// <auto-generated>\n");
                csFile.Append("//     This code was auto-generated by JFramework.ExcelConverter\n");
                csFile.Append("//     version 1.0.0\n");
                csFile.AppendFormat("//     from Assets/AddressableResources/{0}.asset\n", tableName);
                csFile.Append("//\n");
                csFile.Append("//     Changes to this file may cause incorrect behavior and will be lost if\n");
                csFile.Append("//     the code is regenerated.\n");
                csFile.Append("// </auto-generated>\n");
                csFile.Append("//------------------------------------------------------------------------------\n\n");
                csFile.Append("using System;\n");
                csFile.Append("using System.Collections.Generic;\n");
                csFile.Append("using Sirenix.OdinInspector;\n");
                csFile.Append("using UnityEngine;\n");
                csFile.AppendFormat("namespace {0}\n", EditorConst.Namespace);
                csFile.Append("{\n");
                csFile.Append("\t[Serializable]\n");
                csFile.AppendFormat("\tpublic {0} {1} : IData\n", ExcelBuilder.isClass ? "class" : "struct", dataName);
                csFile.Append("\t{\n");
                int columnCount = excelData.column;
                List<int> enumList = new List<int>();
                Parse[] columnFields = new Parse[columnCount];
                for (int i = 0; i < columnCount; i++)
                {
                    string name = excelData.GetData(EditorConst.Name, i);
                    string type = excelData.GetData(EditorConst.Type, i);
                    if (type == "enum") enumList.Add(i);
                    Parse data = ExcelBuilder.Parse(name, type);
                    columnFields[i] = data;
                }

                for (var i = 0; i < columnCount; i++)
                {
                    var columnField = columnFields[i];
                    if (columnField == null) continue;
                    csFile.Append(columnField.GetFieldLine());
                }

                csFile.Append("\n#if UNITY_EDITOR\n");
                csFile.AppendFormat("\t\tpublic {0}(List<List<string>> sheet, int row, int column)\n", dataName);
                csFile.Append("\t\t{\n");
                for (var i = 0; i < columnCount; i++)
                {
                    var columnField = columnFields[i];
                    if (columnField == null) continue;
                    csFile.Append(columnField.GetParseLine());
                }

                csFile.Append("\t\t}\n#endif\n");

                csFile.Append("\t\tpublic void InitData()\n");
                csFile.Append("\t\t{\n");
                for (var i = 0; i < columnCount; i++)
                {
                    var columnField = columnFields[i];
                    if (columnField == null) continue;
                    csFile.Append(columnField.GetInitLine());
                }

                csFile.Append("\t\t}\n");
                csFile.Append("\t}\n\n");

                csFile.AppendFormat("\tpublic class {0} : DataTable\n", tableName);
                csFile.Append("\t{\n");
                csFile.AppendFormat("\t\t[SerializeField]\n\t\tprivate List<{0}> dataList = new List<{0}>();\n", dataName);
                csFile.Append("\t\tpublic override int Count => dataList.Count;\n");
                csFile.Append("\t\tpublic override void InitData() => dataList.ForEach(data => data.InitData());\n");
                csFile.AppendFormat("\t\tpublic override void AddData(IData data) => dataList.Add(({0})data);\n", dataName);
                csFile.Append("\t\tpublic override IData GetData(int index) => dataList[index];\n");
                csFile.Append("\t}\n\n");

                foreach (var e in enumList)
                {
                    string name = excelData.GetData(EditorConst.Name, e);
                    csFile.Append("\tpublic enum " + name + "\n");
                    csFile.Append("\t{\n");

                    int rowCount = excelData.row;
                    for (int i = EditorConst.Data ; i < rowCount; i++)
                    {
                        var field = excelData.GetData(i, e);
                        csFile.AppendFormat("\t\t{0},\n", field);
                    }

                    csFile.Append("\t}\n");
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