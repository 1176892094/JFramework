// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-02-04  15:31
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace JFramework
{
    internal partial class GlobalSetting : ScriptableObject
    {
        private static GlobalSetting instance;
        public static GlobalSetting Instance => instance ??= Resources.Load<GlobalSetting>(nameof(GlobalSetting));

        public AssetPlatform platform = AssetPlatform.StandaloneWindows;
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.OnValueChanged("UpdateSceneSetting")]
#endif
        public string assetInfo = "AssetBundleInfo";

        public string buildPath = "AssetBundles";

        public string dataAssembly = "HotUpdate.Data";

        public string assetPath = "Assets/Template";

        public string remotePath = "http://192.168.0.3:8000/AssetBundles";
        
        public AssetMode assetMode;
        
        public string smtpServer = "smtp.qq.com";
        
        public int smtpPort = 587;
        
        public string senderAddress;
        
        public string senderPassword;
        
        public DebugMode debugMode = DebugMode.Disable;

        public static string clientInfoName => Instance.assetInfo + ".json";

        public static string assetBundlePath => Path.Combine(Application.persistentDataPath, Instance.buildPath);

        public static string GetUIPath(string assetName) => "Prefabs/" + assetName;

        public static string GetAudioPath(string assetName) => "Audios/" + assetName;

        public static string GetScenePath(string assetName) => "Scenes/" + assetName;

        public static string GetTablePath(string assetName) => "DataTable/" + assetName;

        public static string GetAssetBundles(string fileName) => Path.Combine(assetBundlePath, fileName);

        private static string GetPlatform(string fileName) => Path.Combine(Instance.platform.ToString(), fileName);

        public static string GetRemoteFilePath(string fileName) => Path.Combine(Instance.remotePath, GetPlatform(fileName));

        public static string GetStreamingPath(string fileName) => Path.Combine(Application.streamingAssetsPath, GetPlatform(fileName));
    }

#if UNITY_EDITOR
    internal partial class GlobalSetting
    {
        [HideInInspector] public List<string> sceneAssets = new List<string>();

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        public static string EditorPath
        {
            get => UnityEditor.EditorPrefs.GetString(nameof(EditorPath), "Assets/Editor/Resources");
            set => UnityEditor.EditorPrefs.SetString(nameof(EditorPath), value);
        }

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        public static string ScriptPath
        {
            get => UnityEditor.EditorPrefs.GetString(nameof(ScriptPath), "Assets/Template/DataTable");
            set => UnityEditor.EditorPrefs.SetString(nameof(ScriptPath), value);
        }

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        public static string DataTablePath
        {
            get => UnityEditor.EditorPrefs.GetString(nameof(DataTablePath), "Assets/Scripts/DataTable");
            set => UnityEditor.EditorPrefs.SetString(nameof(DataTablePath), value);
        }

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
#endif
        public static BundleMode AssetBuild
        {
            get => (BundleMode)UnityEditor.EditorPrefs.GetInt(nameof(AssetBuild), (int)BundleMode.StreamingAssets);
            set => UnityEditor.EditorPrefs.SetInt(nameof(AssetBuild), (int)value);
        }

        private static string remoteBuildPath => AssetBuild == BundleMode.BuildPath ? Instance.buildPath : Application.streamingAssetsPath;

        public static string platformPath => Path.Combine(remoteBuildPath, Instance.platform.ToString());

        public static string assetBundleInfo => Path.Combine(remoteBuildPath, GetPlatform(clientInfoName));

        public void UpdateSceneSetting()
        {
            var assets = UnityEditor.EditorBuildSettings.scenes.Select(scene => scene.path).ToList();
            foreach (var scenePath in sceneAssets)
            {
                if (assets.Contains(scenePath))
                {
                    if (assetMode == AssetMode.Simulate) continue;
                    var scenes = UnityEditor.EditorBuildSettings.scenes.Where(scene => scene.path != scenePath);
                    UnityEditor.EditorBuildSettings.scenes = scenes.ToArray();
                }
                else
                {
                    if (assetMode == AssetMode.AssetBundle) continue;
                    var scenes = UnityEditor.EditorBuildSettings.scenes.ToList();
                    scenes.Add(new UnityEditor.EditorBuildSettingsScene(scenePath, true));
                    UnityEditor.EditorBuildSettings.scenes = scenes.ToArray();
                }
            }
        }
    }
#endif
}