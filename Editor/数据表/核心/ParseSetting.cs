using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace JFramework
{
    public static class ParseSetting
    {
        private static string LoadPath => PathDataKey.IsEmpty() ? Environment.CurrentDirectory : PathDataKey;

        public static string PathDataKey
        {
            get => EditorPrefs.GetString("PathData");
            private set => EditorPrefs.SetString("PathData", value);
        }
        
        public static bool SaveDataKey
        {
            get => EditorPrefs.GetBool("SavePath");
            set => EditorPrefs.SetBool("SavePath", value);
        }

        [MenuItem("Tools/JFramework/ExcelToScripts", false, 102)]
        public static void ExcelToScripts()
        {
            var filePath = EditorUtility.OpenFolderPanel(default, LoadPath, "");
            if (filePath.IsEmpty()) return;
            PathDataKey = filePath;
            ParseGenerator.GenerateCode();
        }
        
        [DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            if (SaveDataKey == false) return;
            SaveDataKey = false;
            if (LoadPath.IsEmpty()) return;
            Debug.Log("脚本重新编译，开始生成资源.");
        }


        public static string GetDataName(string sheetName) => sheetName + "Data";
        public static string GetTableName(string sheetName) => sheetName + "DataTable";
        public static string GetDataFullName(string sheetName) => Const.Namespace + "." + GetDataName(sheetName);
        public static string GetTableFullName(string sheetName) => Const.Namespace + "." + GetTableName(sheetName);
        public static string GetScriptPath(string sheetName) => Const.ScriptPath + GetTableName(sheetName) + ".cs";
        public static string GetAssetsPath(string sheetName) => Const.AssetsPath + GetTableName(sheetName) + ".asset";

        public static bool IsKeyField(string name, string type)
        {
            var key = name.ToLower().Trim();
            if (!key.EndsWith(":key")) return false;
            if (type.Equals("int") || type.Equals("string")) return true;
            Debug.LogWarning($"主键只支持int和string两种类型!");
            return false;
        }

        public static bool IsEnumField(string type)
        {
            type = type.ToLower().Trim();
            if (!type.EndsWith(":field")) return false;
            if (type.Split(":")[0].Equals("enum")) return true;
            Debug.LogWarning($"不是有效的枚举字段!");
            return false;
        }

        public static IParser Parse(string name, string type)
        {
            var fixType = type.Trim().ToLower();
            if (IsNormal(fixType)) return new ParseNormal(name, type);
            if (IsNormalArray(fixType)) return new ParseNormalArray(name, type);
            if (IsStruct(fixType)) return new ParseStruct(name, type);
            if (IsStructArray(type)) return new ParseStructArray(name, type);
            if (IsEnum(fixType)) return new ParseEnum(name, type);
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