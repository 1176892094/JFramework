using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Debug = UnityEngine.Debug;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#endif


namespace JFramework
{
    public static class AssetSetting
    {
        /// <summary>
        /// 校验文件名称
        /// </summary>
        public const string Info = "AssetInfos";

        /// <summary>
        /// 资源文件路径
        /// </summary>
        public const string FilePath = "Assets/Template";

        /// <summary>
        /// AB包构建路径
        /// </summary>
        public const string BuildPath = "Assets/StreamingAssets";

        /// <summary>
        /// 资源服务器路径
        /// </summary>
        public const string LoadPath = "http://192.168.0.3:8000/Files/Unity/Forest/Assets/StreamingAssets";

        /// <summary>
        /// 压缩选项
        /// </summary>
        public const BuildOptions Options = BuildOptions.ChunkBasedCompression;

        /// <summary>
        /// 构建平台
        /// </summary>
        public const BuildPlatform Platform =
#if UNITY_STANDALONE_WINDOWS
            BuildPlatform.StandaloneWindows;
#elif UNITY_STANDALONE_OSX
            BuildPlatform.StandaloneOSX;
#elif UNITY_ANDROID
            BuildPlatform.Android;
#elif UNITY_IPHONE
            BuildPlatform.iOS;
#endif

#if UNITY_EDITOR
        private static AssetImporter importer;

        [MenuItem("Tools/JFramework/Update AssetBundles", priority = 1)]
        public static void Update()
        {
            string[] assetBundleNames = AssetDatabase.GetAllAssetBundleNames();

            foreach (string assetBundleName in assetBundleNames)
            {
                if (AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleName).Length == 0)
                {
                    AssetDatabase.RemoveAssetBundleName(assetBundleName, true);
                }
            }

            if (!Directory.Exists(FilePath))
            {
                Directory.CreateDirectory(FilePath);
            }

            string[] guids = AssetDatabase.FindAssets("t:Object", new[] { FilePath });
            var enumerable = guids.Select(AssetDatabase.GUIDToAssetPath)
                .Where(asset => !AssetDatabase.IsValidFolder(asset));
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
                }
            }

            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/JFramework/Build AssetBundles", priority = 2)]
        public static void Build()
        {
            if (!Directory.Exists(BuildPath))
            {
                Directory.CreateDirectory(BuildPath);
            }

            var platform = $"{BuildPath}/{Platform}";
            if (!Directory.Exists(platform))
            {
                Directory.CreateDirectory(platform);
            }

            BuildPipeline.BuildAssetBundles($"{BuildPath}/{Platform}", (BuildAssetBundleOptions)Options,
                (BuildTarget)Platform);
            AssetHelper.CreateAssetBundleInfo();
            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/JFramework/Upload AssetBundles", priority = 3)]
        public static void Upload()
        {
            AssetHelper.Uploads();
        }
        
        [MenuItem("Tools/JFramework/CurrentProjectPath", priority = 11)]
        private static void CurrentProjectPath() => Process.Start(Environment.CurrentDirectory);

        [MenuItem("Tools/JFramework/PersistentDataPath", priority = 12)]
        private static void PersistentDataPath() => Process.Start(Application.persistentDataPath);

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
#endif
    }

    public enum BuildPlatform
    {
        StandaloneOSX = 2,
        StandaloneWindows = 5,
        iOS = 9,
        Android = 13,
        WebGL = 20,
    }

    public enum BuildOptions
    {
        /// <summary>
        /// 默认
        /// </summary>
        None = 0,

        /// <summary>
        /// 不压缩
        /// </summary>
        UncompressedAssetBundle = 1,

        /// <summary>
        /// LZ4
        /// </summary>
        ChunkBasedCompression = 256,
    }
}