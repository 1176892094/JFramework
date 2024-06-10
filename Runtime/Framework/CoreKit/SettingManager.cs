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
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace JFramework
{
    internal class SettingManager : ScriptableObject
    {
        private static SettingManager instance;
        public static SettingManager Instance => instance ??= Resources.Load<SettingManager>(nameof(SettingManager));

        public AssetPlatform platform = AssetPlatform.StandaloneWindows;

        public string assetInfo = "AssetBundleInfo";

        public string buildPath = "AssetBundles";

        public string dataAssembly = "HotUpdate.Data";

        public string assetPath = "Assets/Template";

        public string editorPath = "Assets/Editor/Resources";

        public string dataPath = "Assets/Template/DataTable";

        public string scriptPath = "Assets/Scripts/DataTable";

        public string remotePath = "http://192.168.0.3:8000/AssetBundles";

        [OnValueChanged("UpdateSceneSetting")] public AssetMode assetMode;

        public AssetBuild assetBuild = AssetBuild.StreamingAssets;

        [HideInInspector] public bool assetLoadKey;

        [HideInInspector] public string excelPathKey;

        public static string clientInfoName => Instance.assetInfo + ".json";

        private static string remoteInfoName => Instance.assetInfo + "_TMP.json";

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

        private static string GetPlatform(string fileName) => Path.Combine(Instance.platform.ToString(), fileName);

#if UNITY_EDITOR
        [HideInInspector] public string[] sceneEditor = new string[3];

        [HideInInspector] public List<string> sceneAssets = new List<string>();

        [ShowInInspector] public readonly Dictionary<string, string> objects = new Dictionary<string, string>();

        private static string remoteBuildPath => Instance.assetBuild == AssetBuild.BuildPath ? Instance.buildPath : Application.streamingAssetsPath;

        public static string platformPath => Path.Combine(remoteBuildPath, Instance.platform.ToString());

        public static string assetBundleInfo => Path.Combine(remoteBuildPath, GetPlatform(clientInfoName));

        public void UpdateSceneSetting()
        {
            var assets = EditorBuildSettings.scenes.Select(scene => scene.path).ToList();
            foreach (var scenePath in sceneAssets)
            {
                if (assets.Contains(scenePath))
                {
                    if (assetMode == AssetMode.Simulate) continue;
                    var scenes = EditorBuildSettings.scenes.Where(scene => scene.path != scenePath);
                    EditorBuildSettings.scenes = scenes.ToArray();
                }
                else
                {
                    if (assetMode == AssetMode.AssetBundle) continue;
                    var scenes = EditorBuildSettings.scenes.ToList();
                    scenes.Add(new EditorBuildSettingsScene(scenePath, true));
                    EditorBuildSettings.scenes = scenes.ToArray();
                }
            }
        }
#endif
    }
}