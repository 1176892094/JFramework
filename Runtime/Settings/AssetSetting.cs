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
                var json = EditorPrefs.GetString(nameof(AssetSetting));
                if (string.IsNullOrEmpty(json))
                {
                    instance = new AssetSetting();
                    json = JsonUtility.ToJson(instance);
                    EditorPrefs.SetString(nameof(AssetSetting), json);
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
        /// 文件夹资源
        /// </summary>
        [PropertyOrder(1)] public List<DefaultAsset> assetBundles = new List<DefaultAsset>();

        /// <summary>
        /// 场景资源
        /// </summary>
        [HideInInspector] public List<string> sceneAssets = new List<string>();

        /// <summary>
        /// 存储本地加载的资源字典
        /// </summary>
        public Dictionary<string, Object> objects = new Dictionary<string, Object>();

        /// <summary>
        /// 是否远端构建
        /// </summary>
        [HideInInspector] public bool remoteBuild;

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


        [PropertyOrder(2), Button("远端资源构建"), ShowIf("remoteBuild"), GUIColor(1f, 0.5f, 0.5f)]
        public void LocalBuild()
        {
            remoteBuild = !remoteBuild;
            AssetSetting.Instance.Save();
        }

        [PropertyOrder(2), Button("本地资源构建"), HideIf("remoteBuild"), GUIColor(1f, 1f, 1f)]
        public void RemoteBuild()
        {
            remoteBuild = !remoteBuild;
            AssetSetting.Instance.Save();
        }

        /// <summary>
        /// 是否远端加载
        /// </summary>
        [HideInInspector] public bool isRemote;

        [PropertyOrder(2), Button("远端资源加载"), ShowIf("isRemote"), GUIColor(1f, 0.5f, 0.5f)]
        public void LocalLoad()
        {
            EditorSetting.AddSceneToBuildSettings(isRemote = !isRemote);
            AssetSetting.Instance.Save();
        }

        [PropertyOrder(2), Button("本地资源加载"), HideIf("isRemote"), GUIColor(1f, 1f, 1f)]
        public void RemoteLoad()
        {
            EditorSetting.AddSceneToBuildSettings(isRemote = !isRemote);
            AssetSetting.Instance.Save();
        }

        /// <summary>
        /// 设置保存
        /// </summary>
        [PropertyOrder(2), Button("保存设置")]
        public void Save() => EditorPrefs.SetString(nameof(AssetSetting), JsonUtility.ToJson(instance));

        /// <summary>
        /// 更新构建模式
        /// </summary>
        [InitializeOnLoadMethod]
        public static void GetLoadMode() => EditorSetting.AddSceneToBuildSettings(Instance.isRemote);

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