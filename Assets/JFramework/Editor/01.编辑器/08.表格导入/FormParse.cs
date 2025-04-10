// // *********************************************************************************
// // # Project: JFramework
// // # Unity: 6000.3.5f1
// // # Author: 云谷千羽
// // # Version: 1.0.0
// // # History: 2025-04-09 20:04:35
// // # Recently: 2025-04-09 20:04:35
// // # Copyright: 2024, 云谷千羽
// // # Description: This is an automatically generated comment.
// // *********************************************************************************

using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace JFramework
{
    [InitializeOnLoad]
    internal static partial class FormManager
    {
        [Serializable]
        private struct Name
        {
            public string name;
        }
        
        private const int NAME_LINE = 1;
        private const int TYPE_LINE = 2;
        private const int DATA_LINE = 3;
        private static readonly string enumData;
        private static readonly string itemData;
        private static readonly string mainData;
        private static readonly string assemblyName;
        private static readonly string assemblyPath;
        private static readonly string assemblyData;
        
        private static readonly string[] Array =
        {
            "int", "long", "bool", "float", "double", "string",
            "Vector2", "Vector3", "Vector4", "Vector2Int", "Vector3Int"
        };
        
        public static string ScriptPath
        {
            get => EditorPrefs.GetString(nameof(ScriptPath), "Assets/Scripts/DataTable");
            set => EditorPrefs.SetString(nameof(ScriptPath), value);
        }

        public static string DataTablePath
        {
            get => EditorPrefs.GetString(nameof(DataTablePath), "Assets/Template/DataTable");
            set => EditorPrefs.SetString(nameof(DataTablePath), value);
        }

        static FormManager()
        {
            assemblyData = Resources.LoadAll<TextAsset>(nameof(GlobalSetting))[0].text;
            assemblyName = JsonUtility.FromJson<Name>(assemblyData).name;
            assemblyPath = ScriptPath + "/" + assemblyName + ".asmdef";
            enumData = Resources.LoadAll<TextAsset>(nameof(GlobalSetting))[1].text;
            itemData = Resources.LoadAll<TextAsset>(nameof(GlobalSetting))[2].text;
            mainData = Resources.LoadAll<TextAsset>(nameof(GlobalSetting))[3].text;
        }
        
        private static string EnumPath(string name) => ScriptPath + "/01.枚举类/" + name + ".cs";

        private static string ItemPath(string name) => ScriptPath + "/02.结构体/" + name + ".cs";

        private static string MainPath(string name) => ScriptPath + "/03.数据表/" + name + "DataTable.cs";

        private static string AssetPath(string name) => DataTablePath + "/" + name + "DataTable.asset";
        
        private static string DataPath(string name) => "JFramework.Table." + name + "Data";

        private static string TablePath(string name) => "JFramework.Table." + name + "DataTable";
        
        private static bool IsBasic(string assetType)
        {
            if (string.IsNullOrEmpty(assetType))
            {
                return false;
            }

            assetType = assetType.Trim();
            if (assetType.EndsWith(":enum"))
            {
                return true;
            }

            foreach (var basic in Array)
            {
                if (assetType.Equals(basic))
                {
                    return true;
                }
            }

            if (!assetType.EndsWith("[]"))
            {
                return false;
            }

            assetType = assetType.Substring(0, assetType.IndexOf('['));
            foreach (var basic in Array)
            {
                if (assetType.Equals(basic))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsStruct(string assetType)
        {
            if (string.IsNullOrEmpty(assetType))
            {
                return false;
            }

            assetType = assetType.Trim();
            if (assetType.StartsWith("{") && assetType.EndsWith("}"))
            {
                return true;
            }

            if (assetType.StartsWith("{") && assetType.EndsWith("}[]"))
            {
                return true;
            }

            return false;
        }

        private static bool IsSupport(string assetPath)
        {
            if (string.IsNullOrEmpty(assetPath))
            {
                return false;
            }

            if (Path.GetFileName(assetPath).Contains("~$"))
            {
                return false;
            }

            return Path.GetExtension(assetPath).ToLower() is ".xlsx";
        }
    }
}