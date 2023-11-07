// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-10-26  01:14
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

// ReSharper disable All

namespace JFramework.Editor
{
    public class AssetSetting
    {
        /// <summary>
        /// 单例自身
        /// </summary>
        private static AssetSetting instance;

        /// <summary>
        /// 公开的单例访问
        /// </summary>
        public static AssetSetting Instance
        {
            get
            {
                if (instance != null) return instance;
                var asset = Resources.Load<TextAsset>(nameof(AssetSetting));
                var json = asset != null ? asset.text : string.Empty;
                if (string.IsNullOrEmpty(json))
                {
                    instance = new AssetSetting();
                    json = JsonUtility.ToJson(instance);
                    File.WriteAllText(AssetDatabase.GetAssetPath(asset), json);
                    EditorSetting.Add(3, "资源", instance);
                    return instance;
                }

                instance = JsonUtility.FromJson<AssetSetting>(json);
                EditorSetting.Add(3, "资源", instance);
                return instance;
            }
        }

        /// <summary>
        /// 构建 AssetBundle 存放的路径
        /// </summary>
        [FolderPath] public string buildPath = "AssetBundles";

        /// <summary>
        /// 存放要构建成 AssetBundle 的文件路径
        /// </summary>
        [FolderPath] public string editorPath = "Assets/Editor/Resources";

        /// <summary>
        /// 远端资源构建
        /// </summary>
        [LabelText("Remote Build")] public bool remoteBuild;
        
        /// <summary>
        /// 远端资源加载
        /// </summary>
        [HideInInspector] public bool remoteLoad;

        /// <summary>
        /// 是否远端加载
        /// </summary>
        [ShowInInspector, LabelText("Remote Load")]
        public bool isRemote
        {
            get => remoteLoad;
            set
            {
                remoteLoad = !remoteLoad;
                EditorSetting.AddSceneToBuildSettings(remoteLoad);
            }
        }

        /// <summary>
        /// 构建 AssetBundle 文件夹路径
        /// </summary>
        [HideInInspector] public List<string> bundlePaths = new List<string>();

        /// <summary>
        /// AssetBundle 的文件夹资源
        /// </summary>
        [PropertyOrder(1), ShowInInspector] public static List<DefaultAsset> assetBundles = new List<DefaultAsset>();

        /// <summary>
        /// 获取 AssetBundle 的文件夹
        /// </summary>
        /// <returns></returns>
        public static List<DefaultAsset> folderAssets
        {
            get
            {
                if (Instance.bundlePaths.Count > 0)
                {
                    assetBundles.Clear();
                    foreach (var path in Instance.bundlePaths)
                    {
                        assetBundles.Add(AssetDatabase.LoadAssetAtPath<DefaultAsset>(path));
                    }
                }

                return assetBundles;
            }
        }

        /// <summary>
        /// 场景资源
        /// </summary>
        [HideInInspector] public List<string> sceneAssets = new List<string>();

        /// <summary>
        /// 存储本地加载的资源字典
        /// </summary>
        public Dictionary<string, Object> objects = new Dictionary<string, Object>();

        /// <summary>
        /// 本地构建存储路径
        /// </summary>
        [ShowInInspector]
        public static string platformPath
        {
            get
            {
                if (!Instance.remoteBuild)
                {
                    return $"{Application.streamingAssetsPath}/{GlobalSetting.Instance.platform}";
                }

                return $"{Instance.buildPath}/{GlobalSetting.Instance.platform}";
            }
        }

        /// <summary>
        /// 本地构建校验文件
        /// </summary>
        [ShowInInspector]
        public static string assetBundleInfo
        {
            get
            {
                if (!Instance.remoteBuild)
                {
                    return $"{Application.streamingAssetsPath}/{GlobalSetting.Instance.platform}/{GlobalSetting.clientInfoName}";
                }

                return $"{Instance.buildPath}/{GlobalSetting.Instance.platform}/{GlobalSetting.clientInfoName}";
            }
        }


        /// <summary>
        /// 是否远端资源构建
        /// </summary>
        public void RemoteBuild()
        {
            remoteBuild = !remoteBuild;
            AssetSetting.Instance.Save();
        }

        /// <summary>
        /// 是否远端资源加载
        /// </summary>
        public void RemoteLoad()
        {
            EditorSetting.AddSceneToBuildSettings(isRemote = !isRemote);
            AssetSetting.Instance.Save();
        }

        /// <summary>
        /// 设置保存
        /// </summary>
        [PropertyOrder(2), Button("保存设置")]
        public void Save()
        {
            Instance.bundlePaths.Clear();
            foreach (var bundle in assetBundles)
            {
                Instance.bundlePaths.Add(AssetDatabase.GetAssetPath(bundle));
            }

            var asset = Resources.Load<TextAsset>(nameof(AssetSetting));
            var json = JsonUtility.ToJson(instance);
            File.WriteAllText(AssetDatabase.GetAssetPath(asset), json);
        }

        /// <summary>
        /// 更新构建模式
        /// </summary>
        [InitializeOnLoadMethod]
        public static void InitializeOnLoad()
        {
            if (Instance.bundlePaths.Count > 0)
            {
                assetBundles.Clear();
                foreach (var path in Instance.bundlePaths)
                {
                    assetBundles.Add(AssetDatabase.LoadAssetAtPath<DefaultAsset>(path));
                }
            }

            EditorSetting.AddSceneToBuildSettings(Instance.isRemote);
        }

        /// <summary>
        /// 同步加载
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        internal T Load<T>(string path) where T : Object
        {
            if (objects.TryGetValue(path, out var obj))
            {
                if (obj is Texture2D texture)
                {
                    obj = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                    return (T)obj;
                }

                return obj is GameObject ? Object.Instantiate((T)obj) : (T)obj;
            }

            return null;
        }

        /// <summary>
        /// 异步加载
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        internal async Task<T> LoadAsync<T>(string path) where T : Object
        {
            await Task.Yield();
            if (objects.TryGetValue(path, out var obj))
            {
                if (obj is Texture2D texture)
                {
                    obj = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                    return (T)obj;
                }

                return obj is GameObject ? Object.Instantiate((T)obj) : (T)obj;
            }

            return null;
        }

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        internal async Task<AsyncOperation> LoadSceneAsync(string sceneName)
        {
            await Task.Yield();
            return UnitySceneManager.LoadSceneAsync(sceneName.Split('/')[1], LoadSceneMode.Single);
        }
    }
}
#endif