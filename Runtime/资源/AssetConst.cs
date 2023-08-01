namespace JFramework
{
    public struct AssetConst
    {
        /// <summary>
        /// 校验文件名称
        /// </summary>
        public const string INFO = "AssetInfos";
        
        /// <summary>
        /// 资源文件路径
        /// </summary>
        public const string FILE_PATH = "Assets/Template";
        
        /// <summary>
        /// AB包构建路径
        /// </summary>
        public const string BUILD_PATH = "Assets/StreamingAssets";
        
        /// <summary>
        /// 资源服务器路径
        /// </summary>
        public const string LOAD_PATH = "http://192.168.0.3:8000/Files/Unity/JFramework/Assets/StreamingAssets";
        
        /// <summary>
        /// 压缩选项
        /// </summary>
        public const BuildOptions OPTIONS = BuildOptions.ChunkBasedCompression;
        
        /// <summary>
        /// 构建平台
        /// </summary>
        public const BuildPlatform PLATFORM =
#if UNITY_STANDALONE_WINDOWS
            BuildPlatform.StandaloneWindows;
#elif UNITY_STANDALONE_OSX
            BuildPlatform.StandaloneOSX;
#elif UNITY_ANDROID
            BuildPlatform.Android;
#elif UNITY_IPHONE
            BuildPlatform.iOS;
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