using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace JFramework.Excel
{
	internal static partial class ExcelConverter
	{
		public static void ConvertSOFiles(string excelPath, string soPath)
		{
			try
			{
				excelPath = excelPath.Replace("\\", "/");
				soPath = soPath.Replace("\\", "/");

				if (!Directory.Exists(excelPath))
				{
					EditorUtility.DisplayDialog("EasyExcel", "xls/xlsx path doesn't exist.", "OK");
					return;
				}

				excelPath = excelPath.Replace("\\", "/");
				soPath = soPath.Replace("\\", "/");
				if (!soPath.EndsWith("/")) soPath += "/";
				if (Directory.Exists(soPath)) Directory.Delete(soPath, true);
				Directory.CreateDirectory(soPath);
				AssetDatabase.Refresh();

				string[] excelFiles = Directory.GetFiles(excelPath);
				int count = 0;
				for (int i = 0; i < excelFiles.Length; ++i)
				{
					string filePath = excelFiles[i].Replace("\\", "/");
					if (!ExcelEditor.IsExcelFileSupported(filePath)) continue;
					UpdateProgress(i, excelFiles.Length);
					ConvertSOArray(filePath, soPath);
					count++;
				}

				Logger.Log($"资源生成成功,总计{count}个.");
				ClearProgress();
				AssetDatabase.Refresh();
			}
			catch (Exception e)
			{
				Logger.LogError(e.ToString());
				ClearProgress();
				AssetDatabase.Refresh();
			}
		}
		
		private static void ConvertSOArray(string excelPath, string outputPath)
		{
			try
			{
				ExcelFile excelFile = ExcelFile.Load(excelPath);
				if (excelFile == null) return;
				foreach (var sheet in excelFile.sheetList)
				{
					if (sheet == null) continue;
					if (!IsConvert(sheet)) continue;
					var sheetData = RemoveEmptyColumn(sheet);
					ConvertSO(excelPath, sheet.fileName, outputPath, sheetData);
				}
			}
			catch (Exception e)
			{
				Logger.LogError(e.ToString());
				AssetDatabase.Refresh();
			}
		}

		private static void ConvertSO(string excelPath, string sheetName, string outputPath, ExcelData excelData)
		{
			try
			{
				string fileName = Path.GetFileName(excelPath);
				string sheetClassName = ExcelSetting.Instance.GetSheetClassName(fileName, sheetName);
				var asset = ScriptableObject.CreateInstance(sheetClassName);
				var container = asset as ExcelContainer;
				if (container == null) return;
				
				container.ExcelFileName = fileName;
				string className = ExcelSetting.Instance.GetClassName(sheetName, true);
				Type dataType = Type.GetType(className);
				if (dataType == null)
				{
					var assemblies = AppDomain.CurrentDomain.GetAssemblies();
					foreach (var assembly in assemblies)
					{
						dataType = assembly.GetType(className);
						if (dataType != null) break;
					}
				}
				if (dataType == null)
				{
					Logger.LogError(className + " not exist !");
					return;
				}

				ConstructorInfo constructor = dataType.GetConstructor(new []
				{
					typeof(List<List<string>>), 
					typeof(int), 
					typeof(int)
				});
				if (constructor == null) return;
				var keySet = new HashSet<object>();
				for (var row = ExcelSetting.Instance.DataIndex; row < excelData.RowCount; ++row)
				{
					for (var col = 0; col < excelData.ColumnCount; ++col)
					{
						excelData.SetData(row, col, excelData.GetData(row, col).Replace("\n", "\\n"));
					}

					var data = constructor.Invoke(new object[]{excelData.ExcelSheet, row, 0}) as Excel.ExcelData;
					if (data == null) continue;
					
					var key = data.GetKeyFieldValue();
					if (key == null)
					{
						Logger.LogError("Excel表中缺少主键:" + sheetName);
						continue;
					}

					if (key is 0 || key is string s && string.IsNullOrEmpty(s)) continue;
					
					if (!keySet.Contains(key))
					{
						container.AddData(data);
						keySet.Add(key);
					}
				}
				
				var itemPath = outputPath + ExcelSetting.Instance.GetAssetFileName(fileName, sheetName);
				itemPath = itemPath.Substring(itemPath.IndexOf("Assets", StringComparison.Ordinal));
				AssetDatabase.CreateAsset(asset, itemPath);

				AssetDatabase.Refresh();
			}
			catch (Exception ex)
			{
				Logger.LogError(ex.ToString());
			}
		}
	}
}