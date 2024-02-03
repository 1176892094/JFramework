// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-10-24  23:55
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using JFramework.Editor;
using UnityEditor;
#endif

// ReSharper disable All

namespace JFramework
{
    [Serializable]
    internal partial class GlobalSetting
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
        [PropertyOrder(-1)] public AssetPlatform platform = AssetPlatform.StandaloneWindows;

        /// <summary>
        /// AssetBundle 校验文件名称
        /// </summary>
        public string assetInfo = "AssetBundleInfo";

        /// <summary>
        /// 从远端加载 AssetBundle 的路径
        /// </summary>
        public string remotePath = "http://192.168.0.3:8000/AssetBundles";

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
        public static string GetUIPath(string assetName) => "Prefabs/" + assetName;

        /// <summary>
        /// 获取音乐资源
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public static string GetAudioPath(string assetName) => "Audios/" + assetName;

        /// <summary>
        /// 获取场景资源
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public static string GetScenePath(string assetName) => "Scenes/" + assetName;

        /// <summary>
        /// 获取UI资源
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public static string GetTablePath(string assetName) => "DataTable/" + assetName;

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

        /// <summary>
        /// 构建 AssetBundle 存放的路径
        /// </summary>
        public string buildPath = "AssetBundles";

        /// <summary>
        /// 构建资源路径
        /// </summary>
        public string assetPath = "Assets/Template";

        /// <summary>
        /// 存放要构建成 AssetBundle 的文件路径
        /// </summary>
        public string editorPath = "Assets/Editor/Resources";

        /// <summary>
        /// 远端资源加载
        /// </summary>
        [HideInInspector] public bool remoteLoad;

        /// <summary>
        /// 远端资源构建
        /// </summary>
        [HideInInspector] public bool remoteBuild;

        /// <summary>
        /// 是否远端加载
        /// </summary>
        [ShowInInspector, LabelText("Remote Load")]
        public bool RemoteLoad
        {
            get => remoteLoad;
            set
            {
                remoteLoad = value;
                Instance.Save();
                EditorSetting.UpdateBuildSettings();
            }
        }

        /// <summary>
        /// 是否远端构建
        /// </summary>
        [ShowInInspector, LabelText("Remote Build")]
        public bool RemoteBuild
        {
            get => remoteBuild;
            set
            {
                remoteBuild = value;
                Instance.Save();
            }
        }

        /// <summary>
        /// 存储本地加载的资源字典
        /// </summary>
        public Dictionary<string, Object> objects = new Dictionary<string, Object>();

        /// <summary>
        /// 场景资源
        /// </summary>
        [HideInInspector] public List<string> sceneAssets = new List<string>();

        /// <summary>
        /// 远端构建路径
        /// </summary>
        [ShowInInspector]
        public static string remoteBuildPath => Instance.remoteBuild ? Instance.buildPath : Application.streamingAssetsPath;

        /// <summary>
        /// 本地构建存储路径
        /// </summary>
        [ShowInInspector]
        public static string platformPath => Path.Combine(remoteBuildPath, Instance.platform.ToString());

        /// <summary>
        /// 本地构建校验文件
        /// </summary>
        [ShowInInspector]
        public static string assetBundleInfo => Path.Combine(remoteBuildPath, GetPlatform(clientInfoName));

        [Button("保存设置")]
        public void Save()
        {
            var asset = Resources.Load<TextAsset>(nameof(GlobalSetting));
            var contents = JsonUtility.ToJson(instance);
            var path = AssetDatabase.GetAssetPath(asset);
            File.WriteAllText(path, contents);
        }

        /// <summary>
        /// 异步加载
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Load<T>(string path) where T : Object
        {
            if (objects.TryGetValue(path, out var obj))
            {
                if (typeof(T).IsSubclassOf(typeof(Component)))
                {
                    return ((GameObject)Object.Instantiate(obj)).GetComponent<T>();
                }
                else if (obj is GameObject)
                {
                    return (T)Object.Instantiate(obj);
                }
                else if (obj is Texture2D texture)
                {
                    obj = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                }

                return (T)obj;
            }

            return null;
        }
#endif
    }
}