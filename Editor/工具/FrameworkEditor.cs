using System;
using System.Diagnostics;
using System.IO;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace JFramework
{
    internal partial class FrameworkEditor : OdinMenuEditorWindow
    {
        [MenuItem("Tools/JFramework/JFrameworkEditor _F1")]
        private static void JFrameworkEditor()
        {
            var window = GetWindow<FrameworkEditor>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
        }
        
        [MenuItem("Tools/JFramework/Excel To Asset", false, 102)]
        public static void ExcelToAsset()
        {
            var loadPath = EditorPrefs.GetString(EditorConst.ExcelPathKey);
            if (string.IsNullOrEmpty(loadPath)) loadPath = Environment.CurrentDirectory;
            var filePath = EditorUtility.OpenFolderPanel(default, loadPath, "");
            if (string.IsNullOrEmpty(filePath)) return;
            EditorPrefs.SetString(EditorConst.ExcelPathKey, filePath);
            var scriptPath = Environment.CurrentDirectory + "/" + EditorConst.ScriptPath;
            ExcelConverter.ConvertToScript(filePath, scriptPath);
        }

        [MenuItem("Tools/JFramework/Excel Clear All", false, 103)]
        public static void ExcelClearAll()
        {
            EditorPrefs.SetBool(EditorConst.ExcelDataKey, false);
            DeleteScriptFolder();
            DeleteObjectFolder();
            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/JFramework/CurrentProjectPath")]
        private static void CurrentProjectPath()
        {
            Process.Start(System.Environment.CurrentDirectory);
        }

        [MenuItem("Tools/JFramework/PersistentDataPath")]
        private static void PersistentDataPath()
        {
            Process.Start(Application.persistentDataPath);
        }

        [MenuItem("Tools/JFramework/StreamingAssetsPath")]
        private static void StreamingAssetsPath()
        {
            if (Directory.Exists(Application.streamingAssetsPath))
            {
                Process.Start(Application.streamingAssetsPath);
            }
            else
            {
                Directory.CreateDirectory(Application.dataPath + "/StreamingAssets");
                Process.Start(Application.streamingAssetsPath);
            }
        }

        [DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            if (!EditorPrefs.GetBool(EditorConst.ExcelDataKey, false)) return;
            EditorPrefs.SetBool(EditorConst.ExcelDataKey, false);
            var loadPath = EditorPrefs.GetString(EditorConst.ExcelPathKey);
            if (string.IsNullOrEmpty(loadPath)) return;
            Debug.Log("脚本重新编译，开始生成资源.");
            var soPath = Environment.CurrentDirectory + "/" + EditorConst.AssetsPath;
            ExcelConverter.ConvertToObject(loadPath, soPath);
        }

        private static void DeleteScriptFolder()
        {
            if (Directory.Exists(EditorConst.ScriptPath))
            {
                Directory.Delete(EditorConst.ScriptPath, true);
            }

            if (EditorConst.ScriptPath.EndsWith("/") || EditorConst.ScriptPath.EndsWith("\\"))
            {
                var length = EditorConst.ScriptPath.Length - 1;
                var meta = EditorConst.ScriptPath.Substring(0, length) + ".meta";
                if (!string.IsNullOrEmpty(meta) && File.Exists(meta)) File.Delete(meta);
            }
        }

        private static void DeleteObjectFolder()
        {
            if (Directory.Exists(EditorConst.AssetsPath))
            {
                Directory.Delete(EditorConst.AssetsPath, true);
            }

            if (EditorConst.AssetsPath.EndsWith("/") || EditorConst.AssetsPath.EndsWith("\\"))
            {
                var length = EditorConst.AssetsPath.Length - 1;
                var meta = EditorConst.AssetsPath.Substring(0, length) + ".meta";
                if (!string.IsNullOrEmpty(meta) && File.Exists(meta)) File.Delete(meta);
            }
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            OdinMenuTree tree = new OdinMenuTree(supportsMultiSelect: false)
            {
                { "主页", FrameworkEditorHouse.Instance, EditorIcons.House },
                { "设置", FrameworkEditorSetting.Instance, EditorIcons.SettingsCog },
                { "资源", FrameworkEditorAsset.Instance, EditorIcons.Folder },
            };

            tree.SortMenuItemsByName();
            return tree;
        }
    }
}