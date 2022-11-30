using System;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using Logger = JFramework.Basic.Logger;

namespace JFramework.Excel
{
    public class ExcelEditor : EditorWindow
    {
        private ExcelSetting settings;

        [MenuItem(@"Tools/JFramework/Excel Setting", false, 101)]
        public static void OpenSettingsWindow()
        {
            try
            {
                if (EditorApplication.isCompiling)
                {
                    Logger.Log("Waiting for Compiling completed.");
                    return;
                }

                Rect rect = new Rect(0, 0, 440, 320);
                var window = GetWindowWithRect<ExcelEditor>(rect, true, "EasyExcel Settings", true);
                window.Show();
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
            }
        }

        [MenuItem("Tools/JFramework/Excel Writer", false, 102)]
        public static void ImportFolder()
        {
            string loadPath = EditorPrefs.GetString(ExcelConverter.excelPathKey);
            if (string.IsNullOrEmpty(loadPath) || !Directory.Exists(loadPath))
            {
                string prevPath = Environment.CurrentDirectory + "/Assets/EasyExcel/Example/ExcelFiles";
                loadPath = Directory.Exists(prevPath) ? prevPath : Environment.CurrentDirectory;
            }

            string excelPath = EditorUtility.OpenFolderPanel(default, loadPath, "");
            if (string.IsNullOrEmpty(excelPath)) return;
            EditorPrefs.SetString(ExcelConverter.excelPathKey, excelPath);
            string csPath = Environment.CurrentDirectory + "/" + ExcelSetting.Instance.ScriptPath;
            ExcelConverter.ConvertCSFiles(excelPath, csPath);
        }

        [MenuItem("Tools/JFramework/Excel Delete", false, 103)]
        public static void DeleteFolder()
        {
            EditorPrefs.SetBool(ExcelConverter.csChangedKey, false);
            DeleteCSFolder();
            DeleteSOFolder();
            AssetDatabase.Refresh();
        }

        [DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            if (!EditorPrefs.GetBool(ExcelConverter.csChangedKey, false)) return;
            EditorPrefs.SetBool(ExcelConverter.csChangedKey, false);
            string loadPath = EditorPrefs.GetString(ExcelConverter.excelPathKey);
            if (string.IsNullOrEmpty(loadPath)) return;
            Logger.Log("脚本重新编译，开始生成资源.");
            string soPath = Environment.CurrentDirectory + "/" + ExcelSetting.Instance.AssetPath;
            ExcelConverter.ConvertSOFiles(loadPath, soPath);
        }

        private void Awake()
        {
            settings = ExcelSetting.Instance;
        }

        private void OnGUI()
        {
            ExcelStyle.Enable();

            if (settings == null)
            {
                EditorGUILayout.HelpBox("Cannot find JFramework ExcelTool settings file", MessageType.Error);
                return;
            }

            GUILayout.BeginHorizontal();
            GUILayout.Label(settings.Language == LanguageType.Chinese ? "字段名称行" : "Row of Name", ExcelStyle.Label, ExcelStyle.nameOptions);
            settings.NameIndex = EditorGUILayout.IntField(settings.NameIndex, ExcelStyle.TextField, ExcelStyle.valueOptions); 
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label(settings.Language == LanguageType.Chinese ? "字段类型行" : "Row of Type", ExcelStyle.Label, ExcelStyle.nameOptions);
            settings.TypeIndex = EditorGUILayout.IntField(settings.TypeIndex, ExcelStyle.TextField, ExcelStyle.valueOptions);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label(settings.Language == LanguageType.Chinese ? "数据开始行" : "Row of Data", ExcelStyle.Label, ExcelStyle.nameOptions);
            settings.DataIndex = EditorGUILayout.IntField(settings.DataIndex, ExcelStyle.TextField, ExcelStyle.valueOptions);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label(settings.Language == LanguageType.Chinese ? "命名空间前缀" : "Name Space Prefix", ExcelStyle.Label, ExcelStyle.nameOptions);
            settings.Namespace = EditorGUILayout.TextField(settings.Namespace, ExcelStyle.TextField, ExcelStyle.valueOptions);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label(settings.Language == LanguageType.Chinese ? "页数据类名后缀" : "SheetData Postfix", ExcelStyle.Label, ExcelStyle.nameOptions);
            settings.TablePrefix = EditorGUILayout.TextField(settings.TablePrefix, ExcelStyle.TextField, ExcelStyle.valueOptions);
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.Label(settings.Language == LanguageType.Chinese ? "生成C#文件路径" : "CSharpPath", ExcelStyle.Label, ExcelStyle.nameOptions);
            settings.ScriptPath = EditorGUILayout.TextField(settings.ScriptPath, ExcelStyle.TextField, ExcelStyle.valueOptions);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label(settings.Language == LanguageType.Chinese ? "生成资源文件路径" : "AssetPath", ExcelStyle.Label, ExcelStyle.nameOptions);
            settings.AssetPath = EditorGUILayout.TextField(settings.AssetPath, ExcelStyle.TextField, ExcelStyle.valueOptions);
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(settings.Language == LanguageType.Chinese ? "重置" : "Reset", ExcelStyle.Button, GUILayout.Height(20)))
            {
                if (EditorUtility.DisplayDialog("EasyExcel", "Are you sure to reset JFramework ExcelTool settings?", "Yes", "Cancel"))
                {
                    ExcelSetting.Instance.ResetData();
                    EditorUtility.SetDirty(settings);
                }
            }
            
            if (GUILayout.Button(settings.Language == LanguageType.Chinese ? "保存" : "Save", ExcelStyle.Button, GUILayout.Height(20)))
            {
                EditorUtility.SetDirty(settings);
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            // var type = (LanguageType)ExcelStyle.EnumPopup(settings.Language, GUILayout.Height(20), GUILayout.Width(200));
            // if (settings != null && type != settings.Language)
            // {
            //     settings.Language = type;
            // }

            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(settings.Language == LanguageType.Chinese ? "导入" : "Import", ExcelStyle.Button, GUILayout.Height(30)))
            {
                ImportFolder();
            }
            
            if (GUILayout.Button(settings.Language == LanguageType.Chinese ? "删除" : "Delete", ExcelStyle.Button, GUILayout.Height(30)))
            {
                DeleteFolder();
            }

            GUILayout.EndHorizontal();
        }

        private static void DeleteCSFolder()
        {
            if (Directory.Exists(ExcelSetting.Instance.ScriptPath))
            {
                Directory.Delete(ExcelSetting.Instance.ScriptPath, true);
            }

            if (ExcelSetting.Instance.ScriptPath.EndsWith("/") || ExcelSetting.Instance.ScriptPath.EndsWith("\\"))
            {
                int length = ExcelSetting.Instance.ScriptPath.Length - 1;
                string meta = ExcelSetting.Instance.ScriptPath.Substring(0, length) + ".meta";
                if (!string.IsNullOrEmpty(meta) && File.Exists(meta)) File.Delete(meta);
            }
        }

        private static void DeleteSOFolder()
        {
            if (Directory.Exists(ExcelSetting.Instance.AssetPath))
            {
                Directory.Delete(ExcelSetting.Instance.AssetPath, true);
            }

            if (ExcelSetting.Instance.AssetPath.EndsWith("/") || ExcelSetting.Instance.AssetPath.EndsWith("\\"))
            {
                int length = ExcelSetting.Instance.AssetPath.Length - 1;
                string meta = ExcelSetting.Instance.AssetPath.Substring(0, length) + ".meta";
                if (!string.IsNullOrEmpty(meta) && File.Exists(meta)) File.Delete(meta);
            }
        }

        public static bool IsExcelFileSupported(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return false;
            var fileName = Path.GetFileName(filePath);
            if (fileName.Contains("~$")) return false;
            var lower = Path.GetExtension(filePath).ToLower();
            return lower == ".xlsx" || lower == ".xls" || lower == ".xlsm";
        }
    }
}