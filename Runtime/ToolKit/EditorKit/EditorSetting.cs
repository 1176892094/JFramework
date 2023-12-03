// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-10-24  23:56
// # Copyright: 2023, Charlotte
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
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using Debug = UnityEngine.Debug;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JFramework.Editor
{
    internal class EditorSetting : OdinMenuEditorWindow
    {
        /// <summary>
        /// 编辑器窗口列表
        /// </summary>
        private static readonly SortedDictionary<int, (string, object)> windows = new SortedDictionary<int, (string, object)>();

        /// <summary>
        /// 更新构建模式
        /// </summary>
        [InitializeOnLoadMethod]
        private static void InitializeOnLoad() => UpdateBuildSettings();

        /// <summary>
        /// 为本地加载的字典赋值
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void RuntimeInitializeOnLoad() => UpdateAsset();

        /// <summary>
        /// 调用该方法增加面板
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <param name="item"></param>
        public static void Add(int id, string name, object item) => windows[id] = (name, item);

        /// <summary>
        /// 远端加载移除 BuildSetting 反之 加入到 BuildSetting
        /// </summary>
        public static void UpdateBuildSettings()
        {
            var sceneAssets = EditorBuildSettings.scenes.Select(scene => scene.path).ToList();
            foreach (var scenePath in BuildSetting.Instance.sceneAssets)
            {
                if (sceneAssets.Contains(scenePath))
                {
                    if (!BuildSetting.Instance.isRemote) continue;
                    var scenes = EditorBuildSettings.scenes.Where(scene => scene.path != scenePath);
                    EditorBuildSettings.scenes = scenes.ToArray();
                }
                else
                {
                    if (BuildSetting.Instance.isRemote) continue;
                    var scenes = EditorBuildSettings.scenes.ToList();
                    scenes.Add(new EditorBuildSettingsScene(scenePath, true));
                    EditorBuildSettings.scenes = scenes.ToArray();
                }
            }
        }

        /// <summary>
        /// 生成窗口
        /// </summary>
        protected override OdinMenuTree BuildMenuTree()
        {
            Add(2, "资源", BuildSetting.Instance);
            Add(1, "设置", GlobalSetting.Instance);
            var tree = new OdinMenuTree(false);
            foreach (var (id, (path, item)) in windows)
            {
                switch (id)
                {
                    case 0:
                        tree.Add(path, item, EditorIcons.House);
                        break;
                    case 1:
                        tree.Add(path, item, EditorIcons.SettingsCog);
                        break;
                    case 2:
                        tree.Add(path, item, EditorIcons.Bell);
                        break;
                    default:
                        tree.Add(path, item, EditorIcons.Folder);
                        break;
                }
            }

            return tree;
        }

        /// <summary>
        /// 获取MD5码
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private static string GetMD5(string filePath)
        {
            using var file = new FileStream(filePath, FileMode.Open);
            var md5 = new MD5CryptoServiceProvider();
            var md5Info = md5.ComputeHash(file);
            var builder = StreamPool.Pop<StringBuilder>();
            foreach (var info in md5Info)
            {
                builder.Append(info.ToString("X2"));
            }

            var result = builder.ToString();
            StreamPool.Push(builder);
            builder.Clear();
            return result;
        }

        /// <summary>
        /// 获取编辑器窗口面板
        /// </summary>
        [MenuItem("Tools/JFramework/Editor Setting _F1", priority = 1)]
        protected static void ShowEditorWindow()
        {
            var window = GetWindow<EditorSetting>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
        }

        /// <summary>
        /// 更新 AssetBundles 标签
        /// </summary>
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

            BuildSetting.Instance.sceneAssets.Clear();
            foreach (var folderPath in BuildSetting.Instance.folderPaths)
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
                        BuildSetting.Instance.sceneAssets.Add(path);
                    }

                    if (asset == null) continue;
                    BuildSetting.Instance.objects[$"{folder}/{asset.name}"] = asset;
                }
            }

            Debug.Log($"更新 AssetBundles 完成。".Green());
            BuildSetting.Instance.Save();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 构建 AssetBundles
        /// </summary>
        [MenuItem("Tools/JFramework/Build Assets", priority = 3)]
        private static void BuildAsset()
        {
            UpdateAsset();
            var bundleOptions = BuildAssetBundleOptions.ChunkBasedCompression;
            var directory = Directory.CreateDirectory(BuildSetting.platformPath);
            BuildPipeline.BuildAssetBundles(BuildSetting.platformPath, bundleOptions, (BuildTarget)GlobalSetting.Instance.platform);
            var infoList = directory.GetFiles().Where(info => info.Extension == "").ToList();
            var fileList = infoList.Select(info => new AssetData(GetMD5(info.FullName), info.Name, info.Length.ToString())).ToList();
            var contents = JsonUtility.ToJson(new Variables<AssetData>(fileList), true);
            Debug.Log(contents);
            File.WriteAllText(BuildSetting.assetBundleInfo, contents);
            Debug.Log("构建 AssetBundles 成功!".Green());
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 将Excel转化成ScriptableObject
        /// </summary>
        [MenuItem("Tools/JFramework/Update Data", priority = 4)]
        private static void ExcelToScripts()
        {
            var assembly = Reflection.GetAssembly("JFramework.Editor");
            if (assembly == null) return;
            var type = assembly.GetType("JFramework.Editor.ExcelHelper");
            var method = type?.GetMethod("ExcelToScripts", Reflection.Static);
            method?.Invoke(null, null);
        }

        /// <summary>
        /// 持久化路径
        /// </summary>
        [MenuItem("Tools/JFramework/PersistentData", priority = 5)]
        private static void PersistentDataPath() => Process.Start(Application.persistentDataPath);

        /// <summary>
        /// 当前项目路径
        /// </summary>
        [MenuItem("Tools/JFramework/ProjectDirectory", priority = 6)]
        private static void ProjectDirectory() => Process.Start(Environment.CurrentDirectory);

        /// <summary>
        /// 资源流路径
        /// </summary>
        [MenuItem("Tools/JFramework/StreamingAssets", priority = 7)]
        private static void StreamingAssetsPath()
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