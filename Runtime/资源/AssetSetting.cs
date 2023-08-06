using UnityEngine;

namespace JFramework
{
    internal enum BuildPlatform
    {
        StandaloneOSX = 2,
        StandaloneWindows = 5,
        iOS = 9,
        Android = 13,
        WebGL = 20,
    }

    public static class AssetSetting
    {
        /// <summary>
        /// 构建平台
        /// </summary>
        internal const BuildPlatform PLATFORM =
#if UNITY_STANDALONE_WINDOWS
            BuildPlatform.StandaloneWindows;
#elif UNITY_STANDALONE_OSX
            BuildPlatform.StandaloneOSX;
#elif UNITY_ANDROID
            BuildPlatform.Android;
#elif UNITY_IPHONE
            BuildPlatform.iOS;
#endif

        /// <summary>
        /// 存放要构建成 AssetBundle 的文件路径
        /// </summary>
        public const string FILE_PATH = "Assets/Template";

        /// <summary>
        /// AssetBundle 校验文件名称
        /// </summary>
        private const string ASSET_INFO = "AssetBundleInfo";

        /// <summary>
        /// 构建 AssetBundle 存放的路径
        /// </summary>
        public const string SAVE_PATH = "AssetBundles";

        /// <summary>
        /// 从远端加载 AssetBundle 的路径
        /// </summary>
        private const string LOAD_PATH = "http://192.168.0.3:8000/Files/Unity/Forest/AssetBundles";

#if UNITY_EDITOR
        /// <summary>
        /// 构建目标
        /// </summary>
        public static UnityEditor.BuildTarget Target => (UnityEditor.BuildTarget)PLATFORM;
#endif
        /// <summary>
        /// 客户端校验文件名称
        /// </summary>
        public static readonly string clientInfoName = $"{ASSET_INFO}.json";

        /// <summary>
        /// 客户端校验文件名称
        /// </summary>
        private static readonly string serverInfoName = $"{ASSET_INFO}_TMP.json";

        /// <summary>
        /// 客户端校对文件路径
        /// </summary>
        public static readonly string clientInfoPath = $"{Application.persistentDataPath}/{clientInfoName}";

        /// <summary>
        /// 服务器校对文件路径
        /// </summary>
        public static readonly string serverInfoPath = $"{Application.persistentDataPath}/{serverInfoName}";

        /// <summary>
        /// 本地构建存储路径
        /// </summary>
        public static readonly string localSavePath = $"{SAVE_PATH}/{PLATFORM}";

        /// <summary>
        /// 本地构建校验文件
        /// </summary>
        public static readonly string localSaveInfo = $"{SAVE_PATH}/{PLATFORM}/{clientInfoName}";

        /// <summary>
        /// 本地校验文件路径
        /// </summary>
        public static readonly string localInfoPath =
#if UNITY_ANDROID
            $"file://{Application.streamingAssetsPath}/{PLATFORM}/{clientInfoName}";
#else
            $"{Application.streamingAssetsPath}/{PLATFORM}/{clientInfoName}";
#endif

        /// <summary>
        /// 根据 persistentDataPath 获取文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetPerFile(string fileName) => $"{Application.persistentDataPath}/{fileName}";

        /// <summary>
        /// 根据 streamingAssetsPath 获取文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetStrFile(string fileName) => $"{Application.streamingAssetsPath}/{PLATFORM}/{fileName}";

        /// <summary>
        /// 获取远端文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetRemoteFile(string fileName) => $"{LOAD_PATH}/{PLATFORM}/{fileName}";
    }
}