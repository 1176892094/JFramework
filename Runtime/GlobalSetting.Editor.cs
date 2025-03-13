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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JFramework.Common
{
    internal sealed partial class GlobalSetting
    {
        [HideInInspector] public List<string> sceneAssets = new List<string>();
        [HideInInspector] public List<Object> ignoreAssets = new List<Object>();

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

        public static BuildMode BuildPath
        {
            get => (BuildMode)EditorPrefs.GetInt(nameof(BuildPath), (int)BuildMode.StreamingAssets);
            set => EditorPrefs.SetInt(nameof(BuildPath), (int)value);
        }

        private static AssetMode AssetData
        {
            get => singleton.assetPackMode;
            set
            {
                singleton.assetPackMode = value;
                UpdateSceneSetting(value);
            }
        }

        public static BuildTarget BuildTarget => (BuildTarget)singleton.assetPlatform;
    
        public static string[] FolderPaths => AssetDatabase.GetSubFolders(singleton.assetCachePath);

        private static void UpdateSceneSetting(AssetMode assetPackMode)
        {
            var assets = EditorBuildSettings.scenes.Select(scene => scene.path).ToList();
            foreach (var scenePath in singleton.sceneAssets)
            {
                if (assets.Contains(scenePath))
                {
                    if (assetPackMode == AssetMode.Simulate) continue;
                    var scenes = EditorBuildSettings.scenes.Where(scene => scene.path != scenePath);
                    EditorBuildSettings.scenes = scenes.ToArray();
                }
                else
                {
                    if (assetPackMode == AssetMode.Authentic) continue;
                    var scenes = EditorBuildSettings.scenes.ToList();
                    scenes.Add(new EditorBuildSettingsScene(scenePath, true));
                    EditorBuildSettings.scenes = scenes.ToArray();
                }
            }
        }

        public static string remotePackPath => BuildPath == BuildMode.BuildPath ? singleton.assetBuildPath : Application.streamingAssetsPath;

        public static string remoteAssetPath => Path.Combine(remotePackPath, singleton.assetPlatform.ToString());

        public static string remoteAssetPack => Service.Text.Format("{0}/{1}.json", remoteAssetPath, singleton.assetPackName);

        public sealed class EditorWindow : UnityEditor.EditorWindow
        {
            private void OnGUI()
            {
                var setting = singleton;
                setting.assetPlatform = (AssetPlatform)EditorGUILayout.EnumPopup("资源加载模式", setting.assetPlatform);
                setting.smtpServer = EditorGUILayout.TextField("Smtp 服务器", setting.smtpServer);
                setting.smtpPort = EditorGUILayout.IntField("Smtp 端口号", setting.smtpPort);
                setting.smtpUsername = EditorGUILayout.TextField("Smtp 邮箱", setting.smtpUsername);
                setting.smtpPassword = EditorGUILayout.TextField("Smtp 密钥", setting.smtpPassword);
                AssetData = (AssetMode)EditorGUILayout.EnumPopup("资源加载模式", AssetData);
                setting.assetPackName = EditorGUILayout.TextField("资源信息名称", setting.assetPackName);
                setting.assetCachePath = EditorGUILayout.TextField("资源存放路径", setting.assetCachePath);
                setting.assetRemotePath = EditorGUILayout.TextField("资源服务器", setting.assetRemotePath);
                EditorPath = EditorGUILayout.TextField("编辑器文件", EditorPath);
                ScriptPath = EditorGUILayout.TextField("数据表脚本", ScriptPath);
                DataTablePath = EditorGUILayout.TextField("数据表文件", DataTablePath);
                BuildPath = (BuildMode)EditorGUILayout.EnumPopup("AB包构建选项", BuildPath);
                setting.assetBuildPath = EditorGUILayout.TextField("AB包构建路径", setting.assetBuildPath);
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
            }
        }

        public enum BuildMode : byte
        {
            StreamingAssets,
            BuildPath,
        }
    }
}

#endif