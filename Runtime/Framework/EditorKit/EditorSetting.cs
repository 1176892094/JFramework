// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-02-04  15:49
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

#if UNITY_EDITOR
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using JFramework.Core;
using Debug = UnityEngine.Debug;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JFramework
{
    internal partial class EditorSetting
    {
        public static void UpdateSceneSetting(bool remoteLoad)
        {
            if (!SettingManager.Instance) return;
            var sceneAssets = EditorBuildSettings.scenes.Select(scene => scene.path).ToList();
            foreach (var scenePath in SettingManager.Instance.sceneAssets)
            {
                if (sceneAssets.Contains(scenePath))
                {
                    if (!remoteLoad) continue;
                    var scenes = EditorBuildSettings.scenes.Where(scene => scene.path != scenePath);
                    EditorBuildSettings.scenes = scenes.ToArray();
                }
                else
                {
                    if (remoteLoad) continue;
                    var scenes = EditorBuildSettings.scenes.ToList();
                    scenes.Add(new EditorBuildSettingsScene(scenePath, true));
                    EditorBuildSettings.scenes = scenes.ToArray();
                }
            }
        }

        private static string GetProviderInfo(string filePath)
        {
            using var file = new FileStream(filePath, FileMode.Open);
            var provider = new MD5CryptoServiceProvider();
            var infos = provider.ComputeHash(file);
            var builder = StreamPool.Pop<StringBuilder>();
            foreach (var info in infos)
            {
                builder.Append(info.ToString("X2"));
            }

            var result = builder.ToString();
            builder.Clear();
            StreamPool.Push(builder);
            return result;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void RuntimeInitializeOnLoad() => UpdateAsset();
    }

    internal static partial class EditorSetting
    {
        [MenuItem("Tools/JFramework/Update Assets", priority = 2)]
        private static void UpdateAsset()
        {
            var bundleNames = AssetDatabase.GetAllAssetBundleNames();
            foreach (var bundleName in bundleNames)
            {
                if (AssetDatabase.GetAssetPathsFromAssetBundle(bundleName).Length == 0)
                {
                    AssetDatabase.RemoveAssetBundleName(bundleName, true);
                }
            }

            SettingManager.Instance.sceneAssets.Clear();
            var folderPaths = AssetDatabase.GetSubFolders(SettingManager.Instance.assetPath);
            foreach (var folderPath in folderPaths)
            {
                if (string.IsNullOrEmpty(folderPath)) continue;
                var folder = Path.GetFileNameWithoutExtension(folderPath);
                var guids = AssetDatabase.FindAssets("t:Object", new[] { folderPath });
                foreach (var guid in guids)
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    if (AssetDatabase.IsValidFolder(path)) continue;
                    var importer = AssetImporter.GetAtPath(path);
                    if (importer == null) continue;

                    if (importer.assetBundleName != folder.ToLower())
                    {
                        Debug.Log($"增加 AssetBundles 资源: {path.Green()}");
                        importer.assetBundleName = folder;
                        importer.SaveAndReimport();
                    }

                    var asset = AssetDatabase.LoadAssetAtPath<Object>(path);
                    if (asset is SceneAsset)
                    {
                        SettingManager.Instance.sceneAssets.Add(path);
                    }

                    if (asset == null) continue;
                    SettingManager.Instance.objects[$"{folder}/{asset.name}"] = asset;
                }
            }
            
            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/JFramework/Build Assets", priority = 3)]
        private static void BuildAsset()
        {
            UpdateAsset();
            var directory = Directory.CreateDirectory(SettingManager.platformPath);
            BuildPipeline.BuildAssetBundles(SettingManager.platformPath, BuildAssetBundleOptions.ChunkBasedCompression, (BuildTarget)SettingManager.Instance.platform);
            var infoList = directory.GetFiles().Where(info => info.Extension == "").ToList();
            var fileList = infoList.Select(info => new Bundle(GetProviderInfo(info.FullName), info.Name, info.Length.ToString())).ToList();
            var contents = JsonManager.Writer(fileList, true);
            File.WriteAllText(SettingManager.assetBundleInfo, contents);
            Debug.Log("构建 AssetBundles 成功!".Green());
            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/JFramework/Update Data", priority = 4)]
        private static void ExcelToScripts()
        {
            var assembly = Reflection.GetAssembly("JFramework.Editor");
            if (assembly == null) return;
            var type = assembly.GetType("JFramework.Editor.ExcelHelper");
            var method = type?.GetMethod("ExcelToScripts", Reflection.Static);
            method?.Invoke(null, null);
        }

        [MenuItem("Tools/JFramework/ProjectDirectory", priority = 5)]
        private static void ProjectDirectories() => Process.Start(Environment.CurrentDirectory);

        [MenuItem("Tools/JFramework/PersistentData", priority = 6)]
        private static void PersistentDataPath() => Process.Start(Application.persistentDataPath);

        [MenuItem("Tools/JFramework/StreamingAssets", priority = 7)]
        private static void StreamingAssetPath()
        {
            if (!Directory.Exists(Application.streamingAssetsPath))
            {
                Directory.CreateDirectory(Application.dataPath + "/StreamingAssets");
            }

            Process.Start(Application.streamingAssetsPath);
            AssetDatabase.Refresh();
        }
    }
}
#endif