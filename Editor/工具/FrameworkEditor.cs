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
        private static string LoadPath => PathKey.IsEmpty() ? Environment.CurrentDirectory : PathKey;

        private static string ScriptPath => Const.ScriptPath + Path.GetFileName(LoadPath) + "/";

        public static string AssetsPath => Const.AssetsPath + Path.GetFileName(LoadPath) + "/";
        
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
            if (filePath.IsEmpty()) return;
            //ExcelConverter.ConvertToScript(PathKey = filePath, ScriptPath);
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
            // if (!DataKey) return;
            // DataKey = false;
            // if (LoadPath.IsEmpty()) return;
            Debug.Log("脚本重新编译，开始生成资源.");
            // ExcelConverter.ConvertToObject(LoadPath, AssetsPath);
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
            OdinMenuTree tree = new OdinMenuTree(supportsMultiSelect: false);
            tree.Add("主页", FrameworkEditorHouse.Instance, EditorIcons.House);
            tree.Add("设置", FrameworkEditorSetting.Instance, EditorIcons.SettingsCog);
            tree.Add( "资源", FrameworkEditorAsset.Instance, EditorIcons.Folder );
            JFrameworkWindow(tree);
            return tree;
        }

        /// <summary>
        /// 重新绘制框架窗口面板
        /// </summary>
        /// <param name="tree"></param>
        protected abstract void JFrameworkWindow(OdinMenuTree tree);
    }
}