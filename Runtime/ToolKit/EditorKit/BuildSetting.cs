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
using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;


namespace JFramework.Editor
{
    [Serializable]
    internal class BuildSetting
    {
        /// <summary>
        /// 单例自身
        /// </summary>
        private static BuildSetting instance;

        /// <summary>
        /// 安全的单例调用
        /// </summary>
        public static BuildSetting Instance
        {
            get
            {
                if (instance != null) return instance;
                var json = EditorPrefs.GetString(nameof(BuildSetting));
                if (string.IsNullOrEmpty(json))
                {
                    instance = new BuildSetting();
                    json = JsonUtility.ToJson(instance);
                    EditorPrefs.SetString(nameof(BuildSetting), json);
                    return instance;
                }

                instance = JsonUtility.FromJson<BuildSetting>(json);
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
        [HideInInspector] public bool remoteBuild;

        /// <summary>
        /// 是否远端构建
        /// </summary>
        [ShowInInspector, LabelText("Remote Build")]
        public bool isRemoteBuild
        {
            get => remoteBuild;
            set
            {
                remoteBuild = value;
                Instance.Save();
            }
        }

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
                remoteLoad = value;
                Instance.Save();
                EditorSetting.UpdateBuildSettings();
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
        /// 构建 AssetBundle 文件夹路径
        /// </summary>
        [PropertyOrder(1), Folder] public List<string> folderPaths = new List<string>();

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
                    return Path.Combine(Application.streamingAssetsPath, GlobalSetting.Instance.platform.ToString());
                }

                return Path.Combine(Instance.buildPath, GlobalSetting.Instance.platform.ToString());
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
                    return Path.Combine(Application.streamingAssetsPath, GlobalSetting.GetPlatform(GlobalSetting.clientInfoName));
                }

                return Path.Combine(Instance.buildPath, GlobalSetting.GetPlatform(GlobalSetting.clientInfoName));
            }
        }

        /// <summary>
        /// 设置保存
        /// </summary>
        [PropertyOrder(2), Button("保存设置")]
        public void Save()
        {
            EditorPrefs.SetString(nameof(BuildSetting), JsonUtility.ToJson(instance));
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
                if (obj is Texture2D texture)
                {
                    obj = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                    return (T)obj;
                }

                if (typeof(T).IsSubclassOf(typeof(Component)))
                {
                    var asset = ((GameObject)Object.Instantiate(obj)).GetComponent<T>();
                    return asset;
                }
                else
                {
                    var asset = obj is GameObject ? Object.Instantiate((T)obj) : (T)obj;
                    return asset;
                }
            }

            return null;
        }

        /// <summary>
        /// 异步加载
        /// </summary>
        /// <param name="path"></param>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public void LoadAsync<T>(string path, Action<T> action) where T : Object
        {
            if (objects.TryGetValue(path, out var obj))
            {
                if (obj is Texture2D texture)
                {
                    obj = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                    action?.Invoke((T)obj);
                }

                if (typeof(T).IsSubclassOf(typeof(Component)))
                {
                    var asset = ((GameObject)Object.Instantiate(obj)).GetComponent<T>();
                    action?.Invoke(asset);
                }
                else
                {
                    var asset = obj is GameObject ? Object.Instantiate((T)obj) : (T)obj;
                    action?.Invoke(asset);
                }
            }
        }
    }
}
#endif