// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-18 21:12:29
// # Recently: 2024-12-22 20:12:33
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

#if UNITY_EDITOR
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace JFramework
{
    internal static partial class EditorSetting
    {
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

        public static void UpdateSceneSetting(GlobalSetting.AssetMode assetPackMode)
        {
            var assets = EditorBuildSettings.scenes.Select(scene => scene.path).ToList();
            foreach (var scenePath in GlobalSetting.Instance.sceneAssets)
            {
                if (assets.Contains(scenePath))
                {
                    if (assetPackMode == GlobalSetting.AssetMode.Simulate) continue;
                    var scenes = EditorBuildSettings.scenes.Where(scene => scene.path != scenePath);
                    EditorBuildSettings.scenes = scenes.ToArray();
                }
                else
                {
                    if (assetPackMode == GlobalSetting.AssetMode.Authentic) continue;
                    var scenes = EditorBuildSettings.scenes.ToList();
                    scenes.Add(new EditorBuildSettingsScene(scenePath, true));
                    EditorBuildSettings.scenes = scenes.ToArray();
                }
            }
        }

        [MenuItem("Tools/JFramework/框架配置窗口 _F1", priority = 2)]
        public static void ShowWindow()
        {
            var window = EditorWindow.GetWindow<GlobalSetting.EditorWindow>(nameof(GlobalSetting));
            window.position = new Rect(400, 300, 800, 600);
            window.Show();
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
                    AssetLoadKey = await Service.Form.WriteScripts(folderPath);
                    if (!AssetLoadKey)
                    {
                        UpdateAsset();
                    }

                    var elapsedTime = EditorApplication.timeSinceStartup - sinceTime;
                    Debug.Log(Utility.Text.Format("自动生成脚本完成。耗时: {0}秒", elapsedTime.ToString("F").Color("00FF00")));
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
                    AssetLoadKey = false;
                    await Service.Form.WriteAssets(ExcelPathKey);
                    UpdateAsset();
                    var elapsedTime = EditorApplication.timeSinceStartup - sinceTime;
                    Debug.Log(Utility.Text.Format("自动生成资源完成。耗时: {0}秒", elapsedTime.ToString("F").Color("00FF00")));
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
#endif