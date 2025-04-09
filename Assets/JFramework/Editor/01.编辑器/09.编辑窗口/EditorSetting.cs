// // *********************************************************************************
// // # Project: JFramework
// // # Unity: 6000.3.5f1
// // # Author: 云谷千羽
// // # Version: 1.0.0
// // # History: 2025-04-09 23:04:12
// // # Recently: 2025-04-09 23:04:12
// // # Copyright: 2024, 云谷千羽
// // # Description: This is an automatically generated comment.
// // *********************************************************************************

using System;
using System.Diagnostics;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Windows;
using Debug = UnityEngine.Debug;

namespace JFramework
{
    internal static partial class EditorSetting
    {
        private static BuildMode BuildPath
        {
            get => (BuildMode)EditorPrefs.GetInt(nameof(BuildPath), (int)BuildMode.StreamingAssets);
            set => EditorPrefs.SetInt(nameof(BuildPath), (int)value);
        }

        public static string EditorPath
        {
            get => EditorPrefs.GetString(nameof(EditorPath), "Assets/Editor/Resources");
            set => EditorPrefs.SetString(nameof(EditorPath), value);
        }

        public static bool AssetLoadKey
        {
            get => EditorPrefs.GetBool(nameof(AssetLoadKey), false);
            set => EditorPrefs.SetBool(nameof(AssetLoadKey), value);
        }

        public static string ExcelPathKey
        {
            get => EditorPrefs.GetString(nameof(ExcelPathKey), Environment.CurrentDirectory);
            set => EditorPrefs.SetString(nameof(ExcelPathKey), value);
        }

        [MenuItem("Tools/JFramework/框架配置窗口 _F1", priority = 2)]
        public static void ShowWindow()
        {
            // var window = EditorWindow.GetWindow<GlobalSetting.EditorWindow>(nameof(GlobalSetting));
            // window.position = new Rect(400, 300, 800, 600);
            // window.Show();
        }

        [MenuItem("Tools/JFramework/转化表格数据", priority = 5)]
        private static async void ExcelToScripts()
        {
            var folderPath = ExcelPathKey;
            if (string.IsNullOrEmpty(folderPath))
            {
                folderPath = Environment.CurrentDirectory;
            }

            folderPath = EditorUtility.OpenFolderPanel("选择文件夹", folderPath, "");
            if (!string.IsNullOrEmpty(folderPath))
            {
                try
                {
                    AssetLoadKey = false;
                    ExcelPathKey = folderPath;
                    var sinceTime = EditorApplication.timeSinceStartup;
                    EditorUtility.DisplayProgressBar("", "", 0);
                    AssetLoadKey = await FormManager.WriteScripts(folderPath);
                    if (!AssetLoadKey)
                    {
                        UpdateAsset();
                    }

                    var elapsedTime = EditorApplication.timeSinceStartup - sinceTime;
                    Debug.Log(Service.Text.Format("自动生成脚本完成。耗时: {0}秒", elapsedTime.ToString("F").Color("00FF00")));
                }
                finally
                {
                    AssetDatabase.Refresh();
                    EditorUtility.ClearProgressBar();
                }
            }
        }

        [DidReloadScripts]
        private static async void CompileScripts()
        {
            if (AssetLoadKey)
            {
                try
                {
                    var sinceTime = EditorApplication.timeSinceStartup;
                    EditorUtility.DisplayProgressBar("", "", 0);
                    AssetLoadKey = false;
                    await FormManager.WriteAssets(ExcelPathKey);
                    UpdateAsset();
                    var elapsedTime = EditorApplication.timeSinceStartup - sinceTime;
                    Debug.Log(Service.Text.Format("自动生成资源完成。耗时: {0}秒", elapsedTime.ToString("F").Color("00FF00")));
                }
                finally
                {
                    AssetDatabase.Refresh();
                    EditorUtility.ClearProgressBar();
                }
            }
        }

        [MenuItem("Tools/JFramework/项目工程路径", priority = 6)]
        private static void ProjectDirectories() => Process.Start(Environment.CurrentDirectory);


        [MenuItem("Tools/JFramework/程序集编译路径", priority = 7)]
        private static void AssemblyDefinitionPath()
        {
            if (!Directory.Exists(Environment.CurrentDirectory + "/Library/ScriptAssemblies"))
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + "/Library/ScriptAssemblies");
                AssetDatabase.Refresh();
            }

            Process.Start(Environment.CurrentDirectory + "/Library/ScriptAssemblies");
        }

        [MenuItem("Tools/JFramework/持久化存储路径", priority = 8)]
        private static void PersistentDataPath() => Process.Start(Application.persistentDataPath);

        [MenuItem("Tools/JFramework/流动资源路径", priority = 9)]
        private static void StreamingAssetPath()
        {
            if (!Directory.Exists(Application.streamingAssetsPath))
            {
                Directory.CreateDirectory(Application.dataPath + "/StreamingAssets");
                AssetDatabase.Refresh();
            }

            Process.Start(Application.streamingAssetsPath);
        }
    }
}