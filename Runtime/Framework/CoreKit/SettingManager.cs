// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-02-04  15:31
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif

// ReSharper disable All

namespace JFramework
{
    internal partial class SettingManager : ScriptableObject
    {
        private static SettingManager instance;
        public static SettingManager Instance => instance ??= Resources.Load<SettingManager>(nameof(SettingManager));

        public AssetPlatform platform = AssetPlatform.StandaloneWindows;

        public string assetInfo = "AssetBundleInfo";

        public string buildPath = "AssetBundles";

        public string assetPath = "Assets/Template";

        public string editorPath = "Assets/Editor/Resources";

        public string remotePath = "http://192.168.0.3:8000/AssetBundles";

        public bool remoteLoad;

        public bool remoteBuild;

        public static string clientInfoName => Instance.assetInfo + ".json";

        private static string remoteInfoName => Instance.assetInfo + "_TMP.json";

        private static string assetSavePath = $"Packages/com.jinyijie.core/Resources/{nameof(SettingManager)}.asset";

        public static string clientInfoPath => GetPersistentPath(clientInfoName);

        public static string remoteInfoPath => GetPersistentPath(remoteInfoName);

        public static string streamingInfoPath => GetStreamingPath(clientInfoName);

        public static string GetUIPath(string assetName) => "Prefabs/" + assetName;

        public static string GetAudioPath(string assetName) => "Audios/" + assetName;

        public static string GetScenePath(string assetName) => "Scenes/" + assetName;

        public static string GetTablePath(string assetName) => "DataTable/" + assetName;

        public static string GetPersistentPath(string fileName) => Path.Combine(Application.persistentDataPath, fileName);

        public static string GetStreamingPath(string fileName) => Path.Combine(Application.streamingAssetsPath, GetPlatform(fileName));

        public static string GetRemoteFilePath(string fileName) => Path.Combine(Instance.remotePath, GetPlatform(fileName));

        public static string GetPlatform(string fileName) => Path.Combine(Instance.platform.ToString(), fileName);

#if UNITY_EDITOR
        [Folder] public List<string> sceneAssets = new List<string>();

        public Dictionary<string, Object> objects = new Dictionary<string, Object>();

        public static string remoteBuildPath => Instance.remoteBuild ? Instance.buildPath : Application.streamingAssetsPath;

        public static string platformPath => Path.Combine(remoteBuildPath, Instance.platform.ToString());

        public static string assetBundleInfo => Path.Combine(remoteBuildPath, GetPlatform(clientInfoName));

        public T Load<T>(string path) where T : Object
        {
            if (objects.TryGetValue(path, out var obj))
            {
                if (typeof(T).IsSubclassOf(typeof(Component)))
                {
                    return ((GameObject)Object.Instantiate(obj)).GetComponent<T>();
                }
                else if (obj is GameObject)
                {
                    return (T)Object.Instantiate(obj);
                }
                else if (obj is Texture2D texture)
                {
                    obj = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                }

                return (T)obj;
            }

            return null;
        }

        public static SettingManager GetOrAddSettings()
        {
            var settings = Resources.Load<SettingManager>(nameof(SettingManager));
            if (settings == null)
            {
                settings = ScriptableObject.CreateInstance<SettingManager>();
                if (!Directory.Exists(Path.GetDirectoryName(assetSavePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(assetSavePath));
                }

                AssetDatabase.CreateAsset(settings, assetSavePath);
                AssetDatabase.SaveAssets();
            }

            return settings;
        }

        public static SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(GetOrAddSettings());
        }

        [SettingsProvider]
        public static SettingsProvider CreateEditorLearnSettingsProvider()
        {
            var provider = new SettingsProvider("Project/JFramework", SettingsScope.Project)
            {
                guiHandler = (searchContext) =>
                {
                    var settings = GetSerializedSettings();
                    EditorGUILayout.PropertyField(settings.FindProperty("platform"));
                    EditorGUILayout.PropertyField(settings.FindProperty("assetInfo"));
                    EditorGUILayout.PropertyField(settings.FindProperty("buildPath"));
                    EditorGUILayout.PropertyField(settings.FindProperty("assetPath"));
                    EditorGUILayout.PropertyField(settings.FindProperty("editorPath"));
                    EditorGUILayout.PropertyField(settings.FindProperty("remotePath"));
                    EditorGUILayout.PropertyField(settings.FindProperty("remoteLoad"));
                    EditorGUILayout.PropertyField(settings.FindProperty("remoteBuild"));
                    EditorGUILayout.PropertyField(settings.FindProperty("sceneAssets"));
                    settings.ApplyModifiedPropertiesWithoutUndo();
                },

                keywords = new HashSet<string>(new[] { "JFramework" })
            };

            return provider;
        }
#endif
    }
}