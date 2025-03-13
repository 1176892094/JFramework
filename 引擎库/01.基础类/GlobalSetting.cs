// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-11 00:01:01
// # Recently: 2025-01-11 00:01:01
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JFramework
{
    public abstract class GlobalSetting : ScriptableObject
    {
        protected static GlobalSetting instance;
        public static GlobalSetting Instance => instance ??= Resources.Load<GlobalSetting>(nameof(GlobalSetting));

        [SerializeField] protected AssetPlatform assetPlatform = AssetPlatform.StandaloneWindows;

        [SerializeField] protected string smtpServer = "smtp.qq.com";

        [SerializeField] protected int smtpPort = 587;

        [SerializeField] protected string smtpUsername = "1176892094@qq.com";

        [SerializeField] protected string smtpPassword;

        [SerializeField] protected AssetMode assetPackMode = AssetMode.Simulate;

        [SerializeField] protected string assetPackName = "AssetPacket";

        [SerializeField] protected string assetBuildPath = "Assets/StreamingAssets";

        [SerializeField] protected string assetRemotePath = "http://192.168.0.3:8000/AssetPackets";

        [SerializeField] protected string assetAssembly = "HotUpdate.Data";

        [SerializeField] protected string assetCachePath = "Assets/Template";

        protected abstract bool odinSerialize { get; }
        
        protected abstract string scriptDataPath { get; }

        protected abstract string assetDataPath { get; }

        internal static bool odinInspector => Instance.odinSerialize;

        internal static string platformPath => Instance.assetPlatform.ToString();

        internal static bool assetLoadMode => Instance.assetPackMode == AssetMode.Authentic;

        internal static string assetPath => Instance.assetDataPath + "/{0}DataTable.asset";

        internal static string assemblyName => Path.GetFileNameWithoutExtension(assemblyPath);

        internal static string assemblyPath => Service.Text.Format("{0}/{1}.asmdef", Instance.scriptDataPath, Instance.assetAssembly);

        internal static string enumPath => Instance.scriptDataPath + "/01.枚举类/{0}.cs";

        internal static string structPath => Instance.scriptDataPath + "/02.结构体/{0}.cs";

        internal static string tablePath => Instance.scriptDataPath + "/03.数据表/{0}DataTable.cs";

        internal static string assemblyData => Resources.LoadAll<TextAsset>(nameof(GlobalSetting))[0].text;

        internal static string enumData => Resources.LoadAll<TextAsset>(nameof(GlobalSetting))[1].text;

        internal static string structData => Resources.LoadAll<TextAsset>(nameof(GlobalSetting))[2].text;

        internal static string tableData => Resources.LoadAll<TextAsset>(nameof(GlobalSetting))[3].text;

        internal static string assetPackData => Service.Text.Format("{0}.json", Instance.assetPackName);

        internal static string assetPackPath => Service.Text.Format("{0}/{1}", Application.persistentDataPath, Instance.assetBuildPath);

        internal static string GetScenePath(string assetName) => Service.Text.Format("Scenes/{0}", assetName);
        
        internal static string GetAudioPath(string assetName) => Service.Text.Format("Audios/{0}", assetName);

        internal static string GetPanelPath(string assetName) => Service.Text.Format("Prefabs/{0}", assetName);

        internal static string GetTablePath(string assetName) => Service.Text.Format("DataTable/{0}", assetName);

        internal static string GetPlatform(string fileName) => Path.Combine(platformPath, fileName);

        internal static string GetPacketPath(string fileName) => Path.Combine(Instance.assetBuildPath, fileName);

        internal static string GetServerPath(string fileName) => Path.Combine(Instance.assetRemotePath, GetPlatform(fileName));

        internal static string GetClientPath(string fileName) => Path.Combine(Application.streamingAssetsPath, GetPlatform(fileName));

        public abstract void MulticastLock(bool multicast);

        public abstract void CreateAsset(Object assetData, string assetPath);

        public abstract void CreateProgress(string assetPath, float progress);

        public abstract Object LoadByAssetPack(string assetPath, Type assetType, AssetBundle assetPack);

        public abstract Object LoadByResources(string assetPath, Type assetType);

        public abstract Object LoadBySimulates(string assetPath, Type assetType);

        public abstract Task<KeyValuePair<int, string>> LoadRequest(string persistentData, string streamingAssets);

        public static MailData SendMail(string mailBody)
        {
            return new MailData
            {
                smtpServer = Instance.smtpServer,
                smtpPort = Instance.smtpPort,
                senderName = "JFramework",
                senderAddress = Instance.smtpUsername,
                senderPassword = Instance.smtpPassword,
                targetAddress = Instance.smtpUsername,
                mailName = "来自《JFramework》的调试日志:",
                mailBody = mailBody
            };
        }

        protected enum AssetPlatform : byte
        {
            StandaloneOSX = 2,
            StandaloneWindows = 5,
            IOS = 9,
            Android = 13,
            WebGL = 20
        }

        protected enum AssetMode : byte
        {
            Simulate,
            Authentic
        }
    }
}