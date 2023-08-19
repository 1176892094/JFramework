#if UNITY_EDITOR
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using JFramework.Core;
using Newtonsoft.Json;
using Debug = UnityEngine.Debug;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace JFramework.Editor
{
    public class AssetEditor : ScriptableObject
    {
        /// <summary>
        /// 资源编辑器单例
        /// </summary>
        private static AssetEditor instance;
        
        /// <summary>
        /// 资源标签导入器
        /// </summary>
        private static AssetImporter importer;
        
        /// <summary>
        /// 存储本地加载的资源字典
        /// </summary>
        [ShowInInspector] public static readonly Dictionary<string, Object> objects = new Dictionary<string, Object>();
        
        /// <summary>
        /// 本地加载资源的键
        /// </summary>
        [HideInInspector] public List<string> objectKey = new List<string>();
        
        /// <summary>
        /// 本地加载资源的值
        /// </summary>
        [HideInInspector] public List<Object> objectValue = new List<Object>();
        
        /// <summary>
        /// 场景资源
        /// </summary>
        public List<string> sceneAssets = new List<string>();

        /// <summary>
        /// 安全的单例调用
        /// </summary>
        private static AssetEditor Instance
        {
            get
            {
                if (instance != null) return instance;
                var path = $"Assets/Editor/Resources";
                var asset = $"{path}/{nameof(AssetEditor)}.asset";
                instance = AssetDatabase.LoadAssetAtPath<AssetEditor>(asset);
                if (instance != null) return instance;
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                instance = CreateInstance<AssetEditor>();
                AssetDatabase.CreateAsset(instance, asset);
                AssetDatabase.Refresh();
                Debug.Log($"创建 {nameof(AssetEditor).Red()} 单例资源。路径: {path.Yellow()}");
                return instance;
            }
        }

        /// <summary>
        /// 更新 AssetBundles 标签
        /// </summary>
        [MenuItem("Tools/JFramework/Update AssetBundles", priority = 1)]
        private static void Update()
        {
            string[] assetBundleNames = AssetDatabase.GetAllAssetBundleNames();

            foreach (string assetBundleName in assetBundleNames)
            {
                if (AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleName).Length == 0)
                {
                    AssetDatabase.RemoveAssetBundleName(assetBundleName, true);
                }
            }

            if (!Directory.Exists(AssetSetting.FILE_PATH))
            {
                Directory.CreateDirectory(AssetSetting.FILE_PATH);
            }

            string[] guids = AssetDatabase.FindAssets("t:Object", new[] { AssetSetting.FILE_PATH });
            var enumerable = guids.Select(AssetDatabase.GUIDToAssetPath).Where(p => !AssetDatabase.IsValidFolder(p));
            Instance.objectKey.Clear();
            Instance.objectValue.Clear();
            Instance.sceneAssets.Clear();
            foreach (var path in enumerable)
            {
                var array = path.Replace('\\', '/').Split('/');
                importer = AssetImporter.GetAtPath(path);
                if (importer != null)
                {
                    var assetBundleName = importer.assetBundleName;
                    if (assetBundleName != array[2].ToLower())
                    {
                        Debug.Log($"增加 AssetBundles 资源: {path.Green()}");
                        importer.assetBundleName = array[2];
                        importer.SaveAndReimport();
                    }

                    var obj = AssetDatabase.LoadAssetAtPath<Object>(path);
                    if (obj is SceneAsset)
                    {
                        Instance.sceneAssets.Add(path);
                    }

                    if (obj == null) continue;
                    var name = $"{array[2]}/{obj.name}";
                    Instance.objectValue.Add(obj);
                    Instance.objectKey.Add(name);
                    objects[name] = obj;
                }
            }

            Debug.Log($"更新 AssetBundles 完成。".Green());
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 为本地加载的字典赋值
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void RuntimeInitializeOnLoad()
        {
#if UNITY_EDITOR
            Update();
#endif
        }

        /// <summary>
        /// 构建 AssetBundles
        /// </summary>
        [MenuItem("Tools/JFramework/Build AssetBundles", priority = 2)]
        private static void Build()
        {
            Update();
            if (!Directory.Exists(AssetSetting.SAVE_PATH))
            {
                Directory.CreateDirectory(AssetSetting.SAVE_PATH);
            }

            if (!Directory.Exists(AssetSetting.localSavePath))
            {
                Directory.CreateDirectory(AssetSetting.localSavePath);
            }

            BuildPipeline.BuildAssetBundles(AssetSetting.localSavePath, BuildAssetBundleOptions.ChunkBasedCompression, AssetSetting.Target);
            var directory = Directory.CreateDirectory(AssetSetting.localSavePath);
            var fileInfos = directory.GetFiles();
            var fileList = new List<AssetData>();
            foreach (var info in fileInfos)
            {
                if (info.Extension == "")
                {
                    fileList.Add(new AssetData(info.Name, info.Length.ToString(), GetMD5(info.FullName)));
                }
            }

            var saveJson = JsonConvert.SerializeObject(fileList);
            File.WriteAllText(AssetSetting.localSaveInfo, saveJson);
            AssetDatabase.Refresh();
            Debug.Log($"构建 AssetBundles 成功!".Green());
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
            var builder = PoolManager.Pop<StringBuilder>();
            foreach (var info in md5Info)
            {
                builder.Append(info.ToString("X2"));
            }

            var result = builder.ToString();
            PoolManager.Push(builder);
            builder.Clear();
            return result;
        }

        /// <summary>
        /// 当前项目路径
        /// </summary>
        [MenuItem("Tools/JFramework/CurrentProjectPath", priority = 11)]
        private static void CurrentProjectPath() => Process.Start(Application.dataPath);

        /// <summary>
        /// 持久化路径
        /// </summary>
        [MenuItem("Tools/JFramework/PersistentDataPath", priority = 12)]
        private static void PersistentDataPath() => Process.Start(Application.persistentDataPath);

        /// <summary>
        /// 资源流路径
        /// </summary>
        [MenuItem("Tools/JFramework/StreamingAssetsPath", priority = 13)]
        private static void StreamingAssetsPath()
        {
            if (Directory.Exists(Application.streamingAssetsPath))
            {
                Process.Start(Application.streamingAssetsPath);
            }
            else
            {
                Directory.CreateDirectory(Application.dataPath + "/StreamingAssets");
                Process.Start(Application.streamingAssetsPath);
            }
        }

        /// <summary>
        /// 远端加载移除 BuildSetting 反之 加入到 BuildSetting
        /// </summary>
        /// <param name="isRemote"></param>
        internal static void AddSceneToBuildSettings(bool isRemote)
        {
            var sceneAssets = new HashSet<string>();
            foreach (var scene in EditorBuildSettings.scenes)
            {
                sceneAssets.Add(scene.path);
            }

            foreach (var scenePath in Instance.sceneAssets)
            {
                bool sceneFound = sceneAssets.Contains(scenePath);
                if (sceneFound)
                {
                    if (isRemote)
                    {
                        var oldScenes = EditorBuildSettings.scenes;
                        var newScenes = new EditorBuildSettingsScene[oldScenes.Length - 1];
                        int newIndex = 0;

                        foreach (var scene in oldScenes)
                        {
                            if (scene.path != scenePath)
                            {
                                newScenes[newIndex] = scene;
                                newIndex++;
                            }
                        }

                        EditorBuildSettings.scenes = newScenes;
                    }
                }
                else
                {
                    var scenes = new EditorBuildSettingsScene[EditorBuildSettings.scenes.Length + 1];
                    for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
                    {
                        scenes[i] = EditorBuildSettings.scenes[i];
                    }

                    scenes[EditorBuildSettings.scenes.Length] = new EditorBuildSettingsScene(scenePath, true);
                    EditorBuildSettings.scenes = scenes;
                }
            }
        }
    }
}
#endif