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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using JFramework.Core;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using Debug = UnityEngine.Debug;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JFramework
{
    internal partial class EditorSetting : OdinMenuEditorWindow
    {
        public static readonly SortedDictionary<string, object> editors = new SortedDictionary<string, object>();

        [MenuItem("Tools/JFramework/Editor Window _F1", priority = 1)]
        protected static void ShowEditorWindow()
        {
            var window = GetWindow<EditorSetting>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree
            {
                { "主页", SettingManager.Instance, EditorIcons.House },
            };

            var icon = 0;
            foreach (var editor in editors)
            {
                switch (icon)
                {
                    case 0:
                        tree.Add(editor.Key, editor.Value, EditorIcons.SettingsCog);
                        break;
                    case 1:
                        tree.Add(editor.Key, editor.Value, EditorIcons.Bell);
                        break;
                    default:
                        tree.Add(editor.Key, editor.Value, EditorIcons.Folder);
                        break;
                }

                icon++;
            }

            return tree;
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

    internal partial class EditorSetting
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
            BuildPipeline.BuildAssetBundles(SettingManager.platformPath, BuildAssetBundleOptions.ChunkBasedCompression,
                (BuildTarget)SettingManager.Instance.platform);
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