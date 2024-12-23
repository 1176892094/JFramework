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
using UnityEngine;

namespace JFramework
{
    internal static partial class EditorSetting
    {
        [MenuItem("Tools/JFramework/框架配置窗口 _F1", priority = 2)]
        public static void ShowWindow()
        {
            var window = EditorWindow.GetWindow<GlobalSetting.EditorWindow>(nameof(GlobalSetting));
            window.position = new Rect(400, 300, 800, 600);
            window.Show();
        }

        public static void UpdateSceneSetting(GlobalSetting.AssetPackMode assetPackMode)
        {
            var assets = EditorBuildSettings.scenes.Select(scene => scene.path).ToList();
            foreach (var scenePath in GlobalSetting.Instance.sceneAssets)
            {
                if (assets.Contains(scenePath))
                {
                    if (assetPackMode == GlobalSetting.AssetPackMode.Simulate) continue;
                    var scenes = EditorBuildSettings.scenes.Where(scene => scene.path != scenePath);
                    EditorBuildSettings.scenes = scenes.ToArray();
                }
                else
                {
                    if (assetPackMode == GlobalSetting.AssetPackMode.Authentic) continue;
                    var scenes = EditorBuildSettings.scenes.ToList();
                    scenes.Add(new EditorBuildSettingsScene(scenePath, true));
                    EditorBuildSettings.scenes = scenes.ToArray();
                }
            }
        }

        [MenuItem("Tools/JFramework/转化表格数据", priority = 5)]
        private static void ExcelToScripts()
        {
            var assembly = Service.Depend.GetAssembly("JFramework.Editor");
            if (assembly == null)
            {
                return;
            }

            var assetType = assembly.GetType("JFramework.Editor.ExcelManager");
            if (assetType == null)
            {
                return;
            }

            var assetData = assetType.GetMethod("ExcelToScripts", Service.Depend.Static);
            if (assetData == null)
            {
                return;
            }

            assetData.Invoke(null, null);
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