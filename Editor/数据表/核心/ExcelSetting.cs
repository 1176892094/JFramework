using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace JFramework
{
    public static class ExcelSetting
    {
        private static string LoadPath => PathDataKey.IsEmpty() ? Environment.CurrentDirectory : PathDataKey;

        public static string PathDataKey
        {
            get => EditorPrefs.GetString("PathData");
            private set => EditorPrefs.SetString("PathData", value);
        }
        
        private static bool IsCompleted;

        [MenuItem("Tools/JFramework/ExcelToScripts", false, 102)]
        public static void ExcelToScripts()
        {
            var filePath = EditorUtility.OpenFolderPanel(default, LoadPath, "");
            if (filePath.IsEmpty()) return;
            PathDataKey = filePath;
            ExcelGenerator.GenerateScripts();
        }
        
        [MenuItem("Tools/JFramework/ExcelToAssets", false, 102)]
        public static void ExcelToAssets()
        {
            var filePath = EditorUtility.OpenFolderPanel(default, LoadPath, "");
            if (filePath.IsEmpty()) return;
            PathDataKey = filePath;
            ExcelGenerator.GenerateAssets();
            FrameworkEditorAsset.Instance.LoadAddressableGroup();
        }
        
        public static string GetDataName(string sheetName) => sheetName + "Data";
        public static string GetTableName(string sheetName) => sheetName + "DataTable";
        public static string GetDataFullName(string sheetName) => Const.Namespace + "." + GetDataName(sheetName);
        public static string GetTableFullName(string sheetName) => Const.Namespace + "." + GetTableName(sheetName);
        public static string GetScriptPath(string sheetName) => Const.ScriptPath + GetTableName(sheetName) + ".cs";
        public static string GetAssetsPath(string sheetName) => Const.AssetsPath + GetTableName(sheetName) + ".asset";
        public static bool IsKeyField(string name) => name.ToLower().Trim().EndsWith(":key");
        public static bool IsEnumField(string type) => type.ToLower().Trim() == Const.EnumField;

        public static IParser Parse(string name, string type)
        {
            var fixType = type.Trim().ToLower();
            if (IsNormal(fixType)) return new ParseNormal(name, type);
            if (IsEnum(fixType)) return new ParseNormal(name, type);
            if (IsNormalArray(fixType)) return new ParseNormalArray(name, type);
            if (IsStruct(fixType)) return new ParseStruct(name, type);
            if (IsStructArray(type)) return new ParseStructArray(name, type);
            if (name.IsEmpty() || type.IsEmpty()) Debug.LogWarning($"不能转换:{name}=>{type}");
            return null;
        }

        public static bool IsSupported(string filePath)
        {
            if (filePath.IsEmpty()) return false;
            var fileName = Path.GetFileName(filePath);
            if (fileName.Contains("~$")) return false;
            var lower = Path.GetExtension(filePath).ToLower();
            return lower is ".xlsx" or ".xls" or ".xlsm";
        }

        public static Type GetTypeByString(string className)
        {
            var dataType = Type.GetType(className);
            if (dataType == null)
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var assembly in assemblies)
                {
                    dataType = assembly.GetType(className);
                    if (dataType != null) break;
                }
            }

            return dataType;
        }

        public static void UpdateProgress(int curProgress, int maxProgress)
        {
            var title = "数据导入进度 [" + curProgress + " / " + maxProgress + "]";
            var value = curProgress / (float)maxProgress;
            EditorUtility.DisplayProgressBar(title, "", value);
            IsCompleted = true;
        }

        public static void RemoveProgress()
        {
            if (!IsCompleted) return;
            try
            {
                EditorUtility.ClearProgressBar();
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }

            IsCompleted = false;
        }

        private static bool IsNormal(string type) => Const.Array.Any(type.Equals);

        private static bool IsNormalArray(string type)
        {
            if (!type.EndsWith("[]")) return false;
            var name = type.Substring(0, type.IndexOf('['));
            return !name.IsEmpty() && Const.Array.Any(t => name.Equals(t));
        }

        private static bool IsStruct(string type) => type.StartsWith("{") && type.EndsWith("}");

        private static bool IsStructArray(string type) => type.StartsWith("{") && type.EndsWith("}[]");

        private static bool IsEnum(string type) => type == Const.EnumField;
    }
}