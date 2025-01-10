// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-19 03:12:36
// # Recently: 2024-12-22 20:12:33
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.IO;
using UnityEngine;

namespace JFramework
{
    [Serializable]
    internal partial class GlobalSetting : ScriptableObject
    {
        public enum AssetPackMode : byte
        {
            [InspectorName("模拟加载")] Simulate,
            [InspectorName("真实加载")] Authentic
        }

        public enum AssetPlatform : byte
        {
            StandaloneOSX = 2,
            StandaloneWindows = 5,
            IOS = 9,
            Android = 13,
            WebGL = 20
        }

        public enum DebugWindow
        {
            [InspectorName("开启")] Enable,
            [InspectorName("关闭")] Disable,
        }

        public enum PackBuildMode : byte
        {
            [InspectorName("默认路径")] StreamingAssets,
            [InspectorName("指定路径")] BuildPath,
        }

        private static GlobalSetting instance;

        public AssetPlatform assetPlatform = AssetPlatform.StandaloneWindows;

        public string smtpServer = "smtp.qq.com";

        public int smtpPort = 587;

        public string smtpUsername = "1176892094@qq.com";

        public string smtpPassword;

        public AssetPackMode assetPackMode = AssetPackMode.Simulate;

        public string assetPackName = "AssetPacket";

        public string assetPackPath = "Assets/StreamingAssets";

        public string assetCachePath = "Assets/Template";

        public string assetRemotePath = "http://192.168.0.3:8000/AssetPackets";

        public DebugWindow debugWindow = DebugWindow.Disable;

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

        private Service.Mail.MailData SendMail(string mailBody)
        {
            return new Service.Mail.MailData
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

#if UNITY_ANDROID
        private bool multicast;
        private AndroidJavaObject multicastLock;

        private void BeginMulticastLock()
        {
            if (multicast) return;
            using var activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            using var wifiManager = activity.Call<AndroidJavaObject>("getSystemService", "wifi");
            multicastLock = wifiManager.Call<AndroidJavaObject>("createMulticastLock", "lock");
            multicastLock.Call("acquire");
            multicast = true;
        }

        private void EndMulticastLock()
        {
            if (!multicast) return;
            multicastLock?.Call("release");
            multicast = false;
        }
#endif
    }
}