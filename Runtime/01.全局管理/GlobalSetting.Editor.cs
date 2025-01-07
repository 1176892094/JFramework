// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-19 03:12:36
// # Recently: 2024-12-22 20:12:40
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JFramework
{
    internal sealed partial class GlobalSetting
    {
        [HideInInspector] public List<string> sceneAssets = new List<string>();
        [HideInInspector] public List<Object> ignoreAssets = new List<Object>();

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
        
        public static string EditorPath
        {
            get => EditorPrefs.GetString(nameof(EditorPath), "Assets/Editor/Resources");
            set => EditorPrefs.SetString(nameof(EditorPath), value);
        }

        public static string ScriptPath
        {
            get => EditorPrefs.GetString(nameof(ScriptPath), "Assets/Template/DataTable");
            set => EditorPrefs.SetString(nameof(ScriptPath), value);
        }

        public static string DataTablePath
        {
            get => EditorPrefs.GetString(nameof(DataTablePath), "Assets/Scripts/DataTable");
            set => EditorPrefs.SetString(nameof(DataTablePath), value);
        }

        public static PackBuildMode PackBuildPath
        {
            get => (PackBuildMode)EditorPrefs.GetInt(nameof(PackBuildPath), (int)PackBuildMode.StreamingAssets);
            set => EditorPrefs.SetInt(nameof(PackBuildPath), (int)value);
        }

        private static string remotePackPath
        {
            get
            {
                if (PackBuildPath == PackBuildMode.BuildPath)
                {
                    return Instance.assetPackPath;
                }

                return Application.streamingAssetsPath;
            }
        }

        public static string remoteAssetPath => Path.Combine(remotePackPath, Instance.assetPlatform.ToString());

        private static AssetPackMode AssetLoadMode
        {
            get => Instance.assetPackMode;
            set
            {
                Instance.assetPackMode = value;
                EditorSetting.UpdateSceneSetting(value);
            }
        }

        public static string remoteAssetPack => Path.Combine(remoteAssetPath, Instance.assetPackData);

        internal sealed class EditorWindow : UnityEditor.EditorWindow
        {
            private void OnGUI()
            {
                var setting = Instance;
                setting.assetPlatform = (AssetPlatform)EditorGUILayout.EnumPopup("资源加载模式", setting.assetPlatform);
                setting.smtpServer = EditorGUILayout.TextField("Smtp 服务器", setting.smtpServer);
                setting.smtpPort = EditorGUILayout.IntField("Smtp 端口号", setting.smtpPort);
                setting.smtpUsername = EditorGUILayout.TextField("Smtp 邮箱", setting.smtpUsername);
                setting.smtpPassword = EditorGUILayout.TextField("Smtp 密钥", setting.smtpPassword);
                AssetLoadMode = (AssetPackMode)EditorGUILayout.EnumPopup("资源加载模式", AssetLoadMode);
                setting.assetPackName = EditorGUILayout.TextField("资源信息名称", setting.assetPackName);
                setting.assetCachePath = EditorGUILayout.TextField("资源存放路径", setting.assetCachePath);
                setting.assetRemotePath = EditorGUILayout.TextField("资源服务器", setting.assetRemotePath);
                setting.debugWindow = (DebugWindow)EditorGUILayout.EnumPopup("调试器窗口", setting.debugWindow);
                EditorPath = EditorGUILayout.TextField("编辑器文件", EditorPath);
                ScriptPath = EditorGUILayout.TextField("数据表脚本", ScriptPath);
                DataTablePath = EditorGUILayout.TextField("数据表文件", DataTablePath);
                PackBuildPath = (PackBuildMode)EditorGUILayout.EnumPopup("AB包构建选项", PackBuildPath);
                setting.assetPackPath = EditorGUILayout.TextField("AB包构建路径", setting.assetPackPath);
                GUI.enabled = false;
                EditorGUILayout.TextField("AB包生成路径", remotePackPath);
                EditorGUILayout.TextField("AB包平台路径", remoteAssetPath);
                EditorGUILayout.TextField("AB包文件路径", remoteAssetPack);
                GUI.enabled = true;
                var settings = new SerializedObject(setting);
                EditorGUILayout.PropertyField(settings.FindProperty("ignoreAssets"));
                settings.ApplyModifiedPropertiesWithoutUndo();
            }
        }

        [CustomEditor(typeof(GlobalSetting))]
        public sealed class Editor : UnityEditor.Editor
        {
            public override void OnInspectorGUI()
            {
                if (GUILayout.Button("框架配置窗口"))
                {
                    EditorSetting.ShowWindow();
                }
            }
        }
    }
}
#endif