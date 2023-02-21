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
    public abstract class FrameworkEditor : OdinMenuEditorWindow
    {
        private static string LoadPath => string.IsNullOrEmpty(PathKey) ? Environment.CurrentDirectory : PathKey;

        public static string ScriptPath => EditorConst.ScriptPath + Path.GetFileName(LoadPath) + "/";

        public static string AssetsPath => EditorConst.AssetsPath + Path.GetFileName(LoadPath) + "/";
        
        public static string PathKey
        {
            get => EditorPrefs.GetString("ExcelPath");
            private set => EditorPrefs.SetString("ExcelPath", value);
        }

        public static bool DataKey
        {
            get => EditorPrefs.GetBool("ExcelData", false);
            set => EditorPrefs.SetBool("ExcelData", value);
        }


        [MenuItem("Tools/JFramework/Excel To Asset", false, 102)]
        public static void ExcelToAsset()
        {
            var filePath = EditorUtility.OpenFolderPanel(default, LoadPath, "");
            if (string.IsNullOrEmpty(filePath)) return;
            ExcelConverter.ConvertToScript(PathKey = filePath, ScriptPath);
        }

        [MenuItem("Tools/JFramework/Excel Clear All", false, 103)]
        public static void ExcelClearAll()
        {
            DataKey = false;
            DeleteScriptFolder();
            DeleteObjectFolder();
            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/JFramework/CurrentProjectPath")]
        private static void CurrentProjectPath()
        {
            Process.Start(Environment.CurrentDirectory);
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

        /// <summary>
        /// 脚本重新编译
        /// </summary>
        [DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            if (!DataKey) return;
            DataKey = false;
            if (string.IsNullOrEmpty(LoadPath)) return;
            Debug.Log("脚本重新编译，开始生成资源.");
            ExcelConverter.ConvertToObject(LoadPath, AssetsPath);
        }

        /// <summary>
        /// 删除CScript脚本的文件夹
        /// </summary>
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

        /// <summary>
        /// 删除ScriptableObject的文件夹
        /// </summary>
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
         
        /// <summary>
        /// 获取编辑器窗口面板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        protected static void ShowEditorWindow<T>() where T : EditorWindow
        {
            var window = GetWindow<T>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
        }

        /// <summary>
        /// Odin窗口面板
        /// </summary>
        /// <returns></returns>
        protected override OdinMenuTree BuildMenuTree()
        {
            OdinMenuTree tree = new OdinMenuTree(supportsMultiSelect: false)
            {
                { "主页", FrameworkEditorHouse.Instance, EditorIcons.House },
                { "设置", FrameworkEditorSetting.Instance, EditorIcons.SettingsCog },
                { "资源", FrameworkEditorAsset.Instance, EditorIcons.Folder },
            };

            JFrameworkWindow(tree);
            tree.SortMenuItemsByName();
            return tree;
        }

        /// <summary>
        /// 重新绘制框架窗口面板
        /// </summary>
        /// <param name="tree"></param>
        protected abstract void JFrameworkWindow(OdinMenuTree tree);
    }
}