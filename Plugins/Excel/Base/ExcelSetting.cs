using System;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace JFramework.Excel
{
    public class ExcelSetting : ScriptableObject
    {
        private const string ExcelData = "ExcelData";
        private const string Extension = ".asset";
        private const string ExcelPath = "Assets/Resources/Settings/" + ExcelData + Extension;
        public int NameIndex;
        public int TypeIndex;
        public int DataIndex;
        public bool IsPrefix;
        public string Namespace;
        public string AssetPath;
        public string ScriptPath;
        public string TablePrefix;
        public LanguageType Language = LanguageType.English;

        private static ExcelSetting instance;

        public static ExcelSetting Instance
        {
            get
            {
                if (instance != null) return instance;
                instance = ResourceManager.Load<ExcelSetting>("Settings/" + ExcelData);
                if (instance != null) return instance;
                instance = CreateInstance<ExcelSetting>();
                instance.ResetData();
#if UNITY_EDITOR
                string path = Application.dataPath + "/Resources/Settings";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    AssetDatabase.Refresh();
                }

                AssetDatabase.CreateAsset(instance, ExcelPath);
#endif
                return instance;
            }
        }

        public void ResetData()
        {
            NameIndex = 0;
            TypeIndex = 1;
            DataIndex = 2;
            Namespace = "JFramework.Excel";
            AssetPath = "Assets/Resources/Settings/Excel/";
            ScriptPath = "Assets/Scripts/Excel/";
            TablePrefix = "Table";
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }

        public string GetClassName(string sheetName, bool isNamespace = false)
        {
            return (isNamespace ? Namespace + "." : null) + sheetName;
        }

        public string GetAssetFileName(string fileName, string sheetName)
        {
            if (IsPrefix)
            {
                return Path.GetFileNameWithoutExtension(fileName) + sheetName + TablePrefix + Extension;
            }

            return sheetName + TablePrefix + Extension;
        }
        
        public string GetSheetClassName(string fileName, string sheetName)
        {
            if (IsPrefix)
            {
                return Path.GetFileNameWithoutExtension(fileName) + sheetName + TablePrefix;
            }
            return sheetName + TablePrefix;
        }

        public string GetCSharpFileName(string fileName, string sheetName)
        {
            return GetSheetClassName(fileName, sheetName) + ".cs";
        }

        public string GetSheetName(Type sheetClassType)
        {
            string fullName = sheetClassType.Name;
            string[] parts = fullName.Split('.');
            string lastPart = parts[^1];
            lastPart = lastPart.Substring(lastPart.IndexOf('_') + 1);
            return string.IsNullOrEmpty(TablePrefix) ? lastPart : lastPart.Substring(0, lastPart.IndexOf(TablePrefix, StringComparison.Ordinal));
        }
    }
    
    public enum LanguageType
    {
        English,
        Chinese
    }
}