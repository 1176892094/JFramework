// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-10-24  23:55
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

// ReSharper disable All

namespace JFramework
{
    internal class GlobalSetting
    {
        /// <summary>
        /// 单例自身
        /// </summary>
        private static GlobalSetting instance;

        /// <summary>
        /// 安全的单例调用
        /// </summary>
        public static GlobalSetting Instance
        {
            get
            {
                if (instance != null) return instance;
                var asset = Resources.Load<TextAsset>(nameof(GlobalSetting));
                var contents = asset != null ? asset.text : string.Empty;
#if UNITY_EDITOR
                if (string.IsNullOrEmpty(contents))
                {
                    instance = new GlobalSetting();
                    contents = JsonUtility.ToJson(instance);
                    var path = AssetDatabase.GetAssetPath(asset);
                    File.WriteAllText(path, contents);
                    return instance;
                }
#endif
                instance = JsonUtility.FromJson<GlobalSetting>(contents);
                return instance;
            }
        }

        /// <summary>
        /// 构建平台
        /// </summary>
        public AssetPlatform platform = AssetPlatform.StandaloneWindows;

        /// <summary>
        /// AssetBundle 校验文件名称
        /// </summary>
        public string assetInfo = "AssetBundleInfo";

        /// <summary>
        /// 从远端加载 AssetBundle 的路径
        /// </summary>
        public string remotePath = "http://192.168.0.3:8000/AssetBundles";

        /// <summary>
        /// 存放UI的AB包
        /// </summary>
        public string UIBundle = "Prefabs";

        /// <summary>
        /// 存放音效的AB包
        /// </summary>
        public string audioBundle = "Audios";

        /// <summary>
        /// 存放场景的AB包
        /// </summary>
        public string sceneBundle = "Scenes";

        /// <summary>
        /// 存放音效的AB包
        /// </summary>
        public string tableBundle = "DataTable";

        /// <summary>
        /// 客户端校验文件名称
        /// </summary>
        [ShowInInspector]
        public static string clientInfoName => Instance.assetInfo + ".json";

        /// <summary>
        /// 客户端校验文件名称
        /// </summary>
        [ShowInInspector]
        private static string remoteInfoName => Instance.assetInfo + "_TMP.json";

        /// <summary>
        /// 客户端校对文件路径
        /// </summary>
        [ShowInInspector]
        public static string clientInfoPath => GetPersistentPath(clientInfoName);

        /// <summary>
        /// 服务器校对文件路径
        /// </summary>
        [ShowInInspector]
        public static string remoteInfoPath => GetPersistentPath(remoteInfoName);

        /// <summary>
        /// 本地校验文件路径
        /// </summary>
        [ShowInInspector]
        public static string streamingInfoPath => GetStreamingPath(clientInfoName);

        /// <summary>
        /// 获取UI资源
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public static string GetUIPath(string assetName) => Instance.UIBundle + "/" + assetName;

        /// <summary>
        /// 获取音乐资源
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public static string GetAudioPath(string assetName) => Instance.audioBundle + "/" + assetName;

        /// <summary>
        /// 获取场景资源
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public static string GetScenePath(string assetName) => Instance.sceneBundle + "/" + assetName;

        /// <summary>
        /// 获取UI资源
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public static string GetTablePath(string assetName) => Instance.tableBundle + "/" + assetName;


        /// <summary>
        /// 根据 persistentDataPath 获取文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetPersistentPath(string fileName) => Path.Combine(Application.persistentDataPath, fileName);

        /// <summary>
        /// 根据 streamingAssetsPath 获取文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetStreamingPath(string fileName) => Path.Combine(Application.streamingAssetsPath, GetPlatform(fileName));

        /// <summary>
        /// 获取远端文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetRemoteFilePath(string fileName) => Path.Combine(Instance.remotePath, GetPlatform(fileName));

        /// <summary>
        /// 获取资源平台
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetPlatform(string fileName) => Path.Combine(Instance.platform.ToString(), fileName);

#if UNITY_EDITOR
        [Button("保存设置")]
        public void Save()
        {
            var asset = Resources.Load<TextAsset>(nameof(GlobalSetting));
            var contents = JsonUtility.ToJson(instance);
            var path = AssetDatabase.GetAssetPath(asset);
            File.WriteAllText(path, contents);
        }
#endif
    }
}