// // *********************************************************************************
// // # Project: JFramework
// // # Unity: 6000.3.5f1
// // # Author: 云谷千羽
// // # Version: 1.0.0
// // # History: 2025-04-09 21:04:33
// // # Recently: 2025-04-09 21:04:33
// // # Copyright: 2024, 云谷千羽
// // # Description: This is an automatically generated comment.
// // *********************************************************************************

using System.Collections.Generic;
using System.IO;
using JFramework.Common;
using UnityEngine;
using UnityEngine.UI;

namespace JFramework
{
    internal class GlobalSetting : ScriptableObject
    {
        private static GlobalSetting instance;

        public static GlobalSetting Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load<GlobalSetting>(nameof(GlobalSetting));
                }

#if UNITY_EDITOR
                if (instance == null)
                {
                    var assetPath = Service.Text.Format("Assets/{0}", nameof(Resources));
                    instance = CreateInstance<GlobalSetting>();
                    if (!Directory.Exists(assetPath))
                    {
                        Directory.CreateDirectory(assetPath);
                    }

                    assetPath = Service.Text.Format("{0}/{1}.asset", assetPath, nameof(GlobalSetting));
                    UnityEditor.AssetDatabase.CreateAsset(instance, assetPath);
                    UnityEditor.AssetDatabase.SaveAssets();
                }
#endif
                return instance;
            }
        }

        public AssetPlatform assetPlatform = AssetPlatform.StandaloneWindows;

        public string smtpServer = "smtp.qq.com";

        public int smtpPort = 587;

        public string smtpUsername = "1176892094@qq.com";

        public string smtpPassword;

        public AssetMode assetPackMode = AssetMode.Simulate;

        public string assetPackName = "AssetPacket";

        public string assetBuildPath = "Assets/StreamingAssets";

        public string assetRemotePath = "http://192.168.0.3:8000/AssetPackets";

        public string assetAssembly = "HotUpdate.Data";

        public string assetCachePath = "Assets/Template";

#if UNITY_EDITOR
        [HideInInspector] public List<string> sceneAssets = new List<string>();

        [HideInInspector] public List<Object> ignoreAssets = new List<Object>();
#endif


        public static string assetPackData => Service.Text.Format("{0}.json", Instance.assetPackName);
        public static string assetPackPath => Service.Text.Format("{0}/{1}", Application.persistentDataPath, Instance.assetBuildPath);

        public static string GetScenePath(string assetName) => Service.Text.Format("Scenes/{0}", assetName);

        public static string GetAudioPath(string assetName) => Service.Text.Format("Audios/{0}", assetName);

        public static string GetPanelPath(string assetName) => Service.Text.Format("Prefabs/{0}", assetName);

        public static string GetTablePath(string assetName) => Service.Text.Format("DataTable/{0}", assetName);

        public static string GetPlatform(string fileName) => Path.Combine(Instance.assetPlatform.ToString(), fileName);

        public static string GetPacketPath(string fileName) => Path.Combine(Instance.assetBuildPath, fileName);

        public static string GetServerPath(string fileName) => Path.Combine(Instance.assetRemotePath, GetPlatform(fileName));

        public static string GetClientPath(string fileName) => Path.Combine(Application.streamingAssetsPath, GetPlatform(fileName));

        public MailData MailData(string mailBody)
        {
            return new MailData
            {
                smtpServer = smtpServer,
                smtpPort = smtpPort,
                senderName = "JFramework",
                senderAddress = smtpUsername,
                senderPassword = smtpPassword,
                targetAddress = smtpUsername,
                mailName = "来自《JFramework》的调试日志:",
                mailBody = mailBody
            };
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void RuntimeInitializeOnLoad()
        {
            var canvas = new GameObject(nameof(UIManager)).AddComponent<Canvas>();
            canvas.gameObject.layer = LayerMask.NameToLayer("UI");
            canvas.gameObject.AddComponent<GraphicRaycaster>();
            var scaler = canvas.gameObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.referencePixelsPerUnit = 64;
            DontDestroyOnLoad(canvas);
            var manager = new GameObject(nameof(EntityManager)).AddComponent<GlobalManager>();
            manager.canvas = canvas;
            manager.canvas.renderMode = RenderMode.ScreenSpaceCamera;
        }
    }
}