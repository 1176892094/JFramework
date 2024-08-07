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

        private static string GetHashValue(string filePath)
        {
            using var provider = MD5.Create();
            using var stream = File.OpenRead(filePath);
            var hash = provider.ComputeHash(stream);
            var builder = new StringBuilder();
            foreach (var bytes in hash)
            {
                builder.Append(bytes.ToString("x2"));
            }

            return builder.ToString();
        }

        public static void EncryptAssetBundle(string filePath)
        {
            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(AssetManager.AES_KEY);
            var ivBytes = new byte[16];
            RandomNumberGenerator.Fill(ivBytes);
            aes.IV = ivBytes;
            var tempPath = filePath + ".temp";
            Debug.Log(filePath + "\n" + BitConverter.ToString(ivBytes, 0, 16));
            using (var inputFs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            using (var outputFs = new FileStream(tempPath, FileMode.Create, FileAccess.Write))
            using (var cs = new CryptoStream(outputFs, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                outputFs.Write(ivBytes, 0, ivBytes.Length);
                inputFs.CopyTo(cs);
            }

            File.Delete(filePath);
            File.Move(tempPath, filePath);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void RuntimeInitializeOnLoad() => UpdateAsset();
    }

    internal partial class EditorSetting
    {
        [MenuItem("Tools/JFramework/Editor Window _F1", priority = 1)]
        protected static void ShowEditorWindow()
        {
            var window = GetWindow<EditorSetting>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
        }

        [MenuItem("Tools/JFramework/Update Assets", priority = 2)]
        public static void UpdateAsset()
        {
            var names = AssetDatabase.GetAllAssetBundleNames();
            foreach (var name in names)
            {
                if (AssetDatabase.GetAssetPathsFromAssetBundle(name).Length == 0)
                {
                    AssetDatabase.RemoveAssetBundleName(name, true);
                }
            }

            SettingManager.Instance.sceneAssets.Clear();
            var folders = AssetDatabase.GetSubFolders(SettingManager.Instance.assetPath);
            foreach (var folder in folders)
            {
                if (string.IsNullOrEmpty(folder)) continue;
                var name = Path.GetFileNameWithoutExtension(folder);
                var guids = AssetDatabase.FindAssets("t:Object", new[] { folder });

                foreach (var guid in guids)
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    if (!AssetDatabase.IsValidFolder(path))
                    {
                        var importer = AssetImporter.GetAtPath(path);
                        if (importer == null) continue;

                        if (importer.assetBundleName != name.ToLower())
                        {
                            Debug.Log($"增加 AssetBundles 资源: {path.Green()}");
                            importer.assetBundleName = name;
                            importer.SaveAndReimport();
                        }

                        var asset = AssetDatabase.LoadAssetAtPath<Object>(path);
                        if (asset != null)
                        {
                            if (asset is SceneAsset)
                            {
                                SettingManager.Instance.sceneAssets.Add(path);
                            }

                            SettingManager.objects[$"{name}/{asset.name}"] = path;
                        }
                    }
                }
            }

            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/JFramework/Build Assets", priority = 3)]
        private static void BuildAsset()
        {
            UpdateAsset();
            var directory = Directory.CreateDirectory(SettingManager.platformPath);
            var platform = (BuildTarget)SettingManager.Instance.platform;
            BuildPipeline.BuildAssetBundles(SettingManager.platformPath, BuildAssetBundleOptions.None, platform);
            var dataFiles = directory.GetFiles().Where(info => info.Extension == "").ToList();
            var dataInfos = new List<BundleManager.BundleData>();
            if (File.Exists(SettingManager.assetBundleInfo))
            {
                var json = File.ReadAllText(SettingManager.assetBundleInfo);
                var readFiles = JsonManager.Reader<List<BundleManager.BundleData>>(json);
                var readInfos = readFiles.Select(data => data.code).ToList();

                foreach (var file in dataFiles)
                {
                    if (!readInfos.Contains(GetHashValue(file.FullName)))
                    {
                        EncryptAssetBundle(file.FullName);
                    }

                    dataInfos.Add(new BundleManager.BundleData(GetHashValue(file.FullName), file.Name, file.Length.ToString()));
                }
            }
            else
            {
                foreach (var file in dataFiles)
                {
                    EncryptAssetBundle(file.FullName);
                    dataInfos.Add(new BundleManager.BundleData(GetHashValue(file.FullName), file.Name, file.Length.ToString()));
                }
            }

            var contents = JsonManager.Writer(dataInfos, true);
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