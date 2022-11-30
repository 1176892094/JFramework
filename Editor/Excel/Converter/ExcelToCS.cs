using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using JFramework.Basic;
using UnityEditor;

namespace JFramework.Excel
{
	internal static partial class ExcelConverter
	{
		public static void ConvertCSFiles(string excelPath, string csPath)
		{
			try
			{
				excelPath = excelPath.Replace("\\", "/");
				csPath = csPath.Replace("\\", "/");

				if (!Directory.Exists(excelPath))
				{
					EditorUtility.DisplayDialog("JFramework ExcelTool", "Excel files path doesn't exist.", "OK");
					return;
				}

				if (!Directory.Exists(csPath)) Directory.CreateDirectory(csPath);
				string tempPath = Environment.CurrentDirectory + "/ExcelTemp/";
				if (Directory.Exists(tempPath)) Directory.Delete(tempPath, true);
				Directory.CreateDirectory(tempPath);

				excelPath = excelPath.Replace("\\", "/");
				csPath = csPath.Replace("\\", "/");
				if (!csPath.EndsWith("/")) csPath += "/";

				bool csChanged = false;
				string[] excelFiles = Directory.GetFiles(excelPath);
				for (var i = 0; i < excelFiles.Length; ++i)
				{
					string excelFile = excelFiles[i].Replace("\\", "/");
					if (i + 1 < excelFiles.Length)
					{
						UpdateProgress(i + 1, excelFiles.Length);
					}
					else
					{
						ClearProgress();
					}

					if (!ExcelEditor.IsExcelFileSupported(excelFile)) continue;
					string excelName = Path.GetFileName(excelFile);
					var csDict = ConvertCSArray(excelFile);
					foreach (var csNew in csDict)
					{
						string csName = ExcelSetting.Instance.GetCSharpFileName(excelName, csNew.Key);
						string csTempPath = tempPath + csName;
						string csFilePath = csPath + csName;
						bool isRewrite = true;
						if (File.Exists(csFilePath))
						{
							string csOld = File.ReadAllText(csFilePath);
							isRewrite = csOld != csNew.Value;
						}

						if (!isRewrite) continue;
						csChanged = true;
						File.WriteAllText(csTempPath, csNew.Value, Encoding.UTF8);
					}
				}

				ExcelStringBuilder.Reset();

				if (csChanged)
				{
					EditorPrefs.SetBool(csChangedKey, true);
					string[] csFiles = Directory.GetFiles(tempPath);
					foreach (var csFile in csFiles)
					{
						string path = csFile.Replace("\\", "/");
						File.Copy(csFile, csPath + path.Substring(path.LastIndexOf("/", StringComparison.Ordinal)), true);
					}

					AssetDatabase.Refresh();
					Logger.Log("脚本已生成,正在创建资源.");
				}
				else
				{
					Logger.Log("没有改变的CS文件,开始创建资源.");
					ClearProgress();
					string historyPath = EditorPrefs.GetString(excelPathKey);
					if (!string.IsNullOrEmpty(historyPath))
					{
						ConvertSOFiles(historyPath, Environment.CurrentDirectory + "/" + ExcelSetting.Instance.AssetPath);
					}
				}

				if (Directory.Exists(tempPath)) Directory.Delete(tempPath, true);
			}
			catch (Exception e)
			{
				Logger.LogError(e.ToString());
				EditorPrefs.SetBool(csChangedKey, false);
				ClearProgress();
				AssetDatabase.Refresh();
			}
		}

		private static Dictionary<string, string> ConvertCSArray(string excelPath)
		{
			var tempFile = new Dictionary<string, string>();
			var dataFile = ExcelFile.Load(excelPath);
			if (dataFile == null) return tempFile;
			string fileName = Path.GetFileName(excelPath);
			foreach (var sheet in dataFile.sheetList)
			{
				if (sheet == null) continue;
				if (!IsConvert(sheet))
				{
					Logger.Log($"跳过生成---{sheet.fileName}---{fileName}.");
					continue;
				}

				var sheetData = GetSheetData(sheet);
				var csFile = ConvertCS(sheetData, sheet.fileName, fileName);
				tempFile.Add(sheet.fileName, csFile);
			}

			return tempFile;
		}

		private static string ConvertCS(ExcelData excelData, string sheetName, string fileName)
		{
			try
			{
				var rowClassName = ExcelSetting.Instance.GetClassName(sheetName);
				var sheetClassName = ExcelSetting.Instance.GetSheetClassName(fileName, sheetName);
				var csFile = new StringBuilder(2048);
				csFile.Append("using System;\n");
				csFile.Append("using System.Collections.Generic;\n");
				csFile.Append("using UnityEngine;\n\n");

				csFile.AppendFormat("namespace {0}\n", ExcelSetting.Instance.Namespace);
				csFile.Append("{\n");
				csFile.Append("\t[Serializable]\n");
				csFile.Append("\tpublic class " + rowClassName + " : ExcelData\n");
				csFile.Append("\t{\n");
				
				int columnCount = excelData.ColumnCount;
				ExcelBaseType[] columnFields = new ExcelBaseType[columnCount];
				for (int i = 0; i < columnCount; i++)
				{
					string name = excelData.GetData(ExcelSetting.Instance.NameIndex, i);
					string type = excelData.GetData(ExcelSetting.Instance.TypeIndex, i);
					ExcelBaseType data = ExcelValueParser.Parse(i, name, type);
					columnFields[i] = data;
				}
				for (var i = 0; i < columnCount; i++)
				{
					var columnField = columnFields[i];
					if (columnField == null)continue;
					csFile.Append(columnField.GetFieldLine());
				}

				csFile.Append("\n#if UNITY_EDITOR\n");
				csFile.AppendFormat("\t\tpublic {0}(List<List<string>> sheet, int row, int column)\n", rowClassName);
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
				
				csFile.AppendFormat("\tpublic class {0} : ExcelContainer\n", sheetClassName);
				csFile.Append("\t{\n");
				csFile.AppendFormat("\t\t[SerializeField]\n\t\tprivate List<{0}> dataList = new List<{0}>();\n\n", rowClassName);
				
				csFile.Append("\t\tpublic override void InitData()\n\t\t{\n");
				csFile.Append("\t\t\tforeach (var data in dataList)\n");
				csFile.Append("\t\t\t{\n");
				csFile.Append("\t\t\t\tdata.InitData();\n");
				csFile.Append("\t\t\t}\n");
				csFile.Append("\t\t}\n");
				
				csFile.Append("\t\tpublic override void AddData(ExcelData data)\n");
				csFile.Append("\t\t{\n");
				csFile.AppendFormat("\t\t\tdataList.Add(({0})data);\n", rowClassName);
				csFile.Append("\t\t}\n\n");
				
				csFile.Append("\t\tpublic override ExcelData GetData(int index)\n");
				csFile.Append("\t\t{\n");
				csFile.Append("\t\t\treturn dataList[index];\n");
				csFile.Append("\t\t}\n\n");
				
				csFile.Append("\t\tpublic override int GetCount()\n");
				csFile.Append("\t\t{\n");
				csFile.Append("\t\t\treturn dataList.Count;\n");
				csFile.Append("\t\t}\n");

				csFile.Append("\t}\n");
				
				csFile.Append("}\n");

				return csFile.ToString();
			}
			catch (Exception ex)
			{
				Logger.LogError(ex.ToString());
			}

			return "";
		}
	}
}