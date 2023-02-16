using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Type = System.Type;

namespace JFramework
{
	internal static partial class ExcelConverter
	{
		public static void ConvertToObject(string excelPath, string soPath)
		{
			try
			{
				excelPath = excelPath.Replace("\\", "/");
				soPath = soPath.Replace("\\", "/");

				if (!Directory.Exists(excelPath)) return;

				excelPath = excelPath.Replace("\\", "/");
				soPath = soPath.Replace("\\", "/");
				if (!soPath.EndsWith("/")) soPath += "/";
				if (Directory.Exists(soPath))
				{
					Directory.Delete(soPath, true);
				}

				Directory.CreateDirectory(soPath);
				AssetDatabase.Refresh();

				string[] excelFiles = Directory.GetFiles(excelPath);
				int count = 0;
				for (int i = 0; i < excelFiles.Length; ++i)
				{
					string filePath = excelFiles[i].Replace("\\", "/");
					if (!IsSupportedExcel(filePath)) continue;
					UpdateProgress(i, excelFiles.Length);
					ConvertObjectData(filePath, soPath);
					count++;
				}

				Debug.Log($"资源生成成功,总计{count}个.");
				RemoveProgress();
				AssetDatabase.Refresh();
			}
			catch (Exception e)
			{
				Debug.LogError(e.ToString());
				RemoveProgress();
				AssetDatabase.Refresh();
			}
		}

		private static void ConvertObjectData(string excelPath, string outputPath)
		{
			try
			{
				ExcelSource excelSource = ExcelSource.Load(excelPath);
				if (excelSource == null) return;
				foreach (var sheet in excelSource.tableList)
				{
					if (sheet == null) continue;
					if (!IsConvertTable(sheet)) continue;
					var sheetData = RemoveEmptyColumn(sheet);
					ConvertObject(sheet.fileName, outputPath, sheetData);
				}
			}
			catch (Exception e)
			{
				Debug.LogError(e.ToString());
				AssetDatabase.Refresh();
			}
		}

		private static void ConvertObject(string sheetName, string outputPath, ExcelData excelData)
		{
			try
			{
				string sheetClassName = GetTableName(sheetName);
				var asset = ScriptableObject.CreateInstance(sheetClassName);
				var container = asset as DataTable;
				if (container == null) return;
				string className = EditorConst.Namespace + "." + sheetName;
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
					Debug.LogError($"不能转化该数据:{className}");
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
				for (var row = EditorConst.Data; row < excelData.Row; ++row)
				{
					for (var col = 0; col < excelData.Column; ++col)
					{
						excelData.SetData(row, col, excelData.GetData(row, col).Replace("\n", "\\n"));
					}

					var data = (Data)constructor.Invoke(new object[] { excelData.DataList, row, 0 });
					if (data == null) continue;
					
					var key = data.KeyValue();
					if (key == null)
					{
						Debug.LogWarning($"Excel表中缺少主键:{sheetName}");
						continue;
					}

					if (key is 0 || key is string s && string.IsNullOrEmpty(s)) continue;
					
					if (!keySet.Contains(key))
					{
						container.AddData(data);
						keySet.Add(key);
					}
				}
				
				var itemPath = outputPath + GetObjectName(sheetName);
				itemPath = itemPath.Substring(itemPath.IndexOf("Assets", StringComparison.Ordinal));
				AssetDatabase.CreateAsset(asset, itemPath);
				
				AssetDatabase.Refresh();
			}
			catch (Exception ex)
			{
				Debug.LogError(ex.ToString());
			}
		}
	}
}