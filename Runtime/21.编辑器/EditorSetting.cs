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
using System.Threading.Tasks;
using Debug = UnityEngine.Debug;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JFramework
{
    internal partial class EditorSetting
    {
        public static readonly Dictionary<string, string> objects = new Dictionary<string, string>();

        private static string GetUniqueCode(string filePath)
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


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void RuntimeInitializeOnLoad() => UpdateAsset();

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

            GlobalSetting.Instance.sceneAssets.Clear();
            var folders = AssetDatabase.GetSubFolders(GlobalSetting.Instance.assetPath);
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
                        var asset = AssetDatabase.LoadAssetAtPath<Object>(path);
                        if (asset != null && asset is not DefaultAsset)
                        {
                            var importer = AssetImporter.GetAtPath(path);
                            
                            if (importer.assetBundleName != name.ToLower())
                            {
                                Debug.Log($"增加 AssetBundles 资源: {path.Green()}");
                                importer.assetBundleName = name;
                                importer.SaveAndReimport();
                            }

                            if (asset is SceneAsset)
                            {
                                GlobalSetting.Instance.sceneAssets.Add(path);
                            }

                            objects[$"{name}/{asset.name}"] = path;
                        }
                    }
                }
            }

            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/JFramework/Build Assets", priority = 3)]
        private static async void BuildAsset()
        {
            UpdateAsset();
            var directory = Directory.CreateDirectory(GlobalSetting.platformPath);
            var platform = (BuildTarget)GlobalSetting.Instance.platform;
            BuildPipeline.BuildAssetBundles(GlobalSetting.platformPath, BuildAssetBundleOptions.None, platform);
            var elapseTime = EditorApplication.timeSinceStartup;
            var fileInfos = directory.GetFiles().Where(info => info.Extension == "").ToList();
            var dataInfos = new List<BundleData>();
            var readInfos = new List<string>();
            var isExists = File.Exists(GlobalSetting.assetBundleInfo);
            if (isExists)
            {
                var json = await File.ReadAllTextAsync(GlobalSetting.assetBundleInfo);
                var readFiles = JsonManager.Read<List<BundleData>>(json);
                readInfos = readFiles.Select(data => data.code).ToList();
            }

            var writeTasks = new List<Task>();
            foreach (var fileInfo in fileInfos)
            {
                if (isExists && readInfos.Contains(GetUniqueCode(fileInfo.FullName)))
                {
                    dataInfos.Add(new BundleData(GetUniqueCode(fileInfo.FullName), fileInfo.Name, fileInfo.Length.ToString()));
                    continue;
                }

                var writeTask = Task.Run(() =>
                {
                    var readBytes = File.ReadAllBytes(fileInfo.FullName);
                    readBytes = Obfuscator.Encrypt(readBytes);
                    File.WriteAllBytes(fileInfo.FullName, readBytes);
                });
                writeTasks.Add(writeTask);
                WaitBundleTask();

                async void WaitBundleTask()
                {
                    await writeTask;
                    Debug.Log("加密AB包：" + fileInfo.FullName);
                    dataInfos.Add(new BundleData(GetUniqueCode(fileInfo.FullName), fileInfo.Name, fileInfo.Length.ToString()));
                }
            }

            await Task.WhenAll(writeTasks);
            await Task.Yield();
            var contents = JsonManager.Write(dataInfos);
            await File.WriteAllTextAsync(GlobalSetting.assetBundleInfo, contents);
            elapseTime = EditorApplication.timeSinceStartup - elapseTime;
            Debug.Log("加密 AssetBundle 完成。耗时:<color=#00FF00> " + elapseTime.ToString("F") + " </color>秒");
            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/JFramework/Update Data", priority = 4)]
        private static void ExcelToScripts()
        {
            var assembly = Reflection.GetAssembly("JFramework.Editor");
            if (assembly == null) return;
            var type = assembly.GetType("JFramework.Editor.ExcelManager");
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
                AssetDatabase.Refresh();
            }

            Process.Start(Application.streamingAssetsPath);
        }
    }
#if ODIN_INSPECTOR
    internal partial class EditorSetting : Sirenix.OdinInspector.Editor.OdinMenuEditorWindow
    {
        public static readonly SortedDictionary<string, object> editors = new SortedDictionary<string, object>();

        [MenuItem("Tools/JFramework/Editor Window _F1", priority = 1)]
        protected static void ShowEditorWindow()
        {
            var window = GetWindow<EditorSetting>();
            window.position = Sirenix.Utilities.Editor.GUIHelper.GetEditorWindowRect();
            window.position = AlignCenter(window.position, 800, 600);
        }

        public static Rect AlignCenter(Rect rect, float width, float height)
        {
            rect.x = (float)(rect.x + rect.width * 0.5 - width * 0.5);
            rect.y = (float)(rect.y + rect.height * 0.5 - height * 0.5);
            rect.width = width;
            rect.height = height;
            return rect;
        }

        protected override Sirenix.OdinInspector.Editor.OdinMenuTree BuildMenuTree()
        {
            var tree = new Sirenix.OdinInspector.Editor.OdinMenuTree
            {
                { "主页", GlobalSetting.Instance, Sirenix.Utilities.Editor.EditorIcons.House },
            };

            var icon = 0;
            foreach (var editor in editors)
            {
                switch (icon)
                {
                    case 0:
                        tree.Add(editor.Key, editor.Value, Sirenix.Utilities.Editor.EditorIcons.SettingsCog);
                        break;
                    case 1:
                        tree.Add(editor.Key, editor.Value, Sirenix.Utilities.Editor.EditorIcons.Bell);
                        break;
                    default:
                        tree.Add(editor.Key, editor.Value, Sirenix.Utilities.Editor.EditorIcons.Folder);
                        break;
                }

                icon++;
            }

            return tree;
        }
    }
#endif
}
#endif