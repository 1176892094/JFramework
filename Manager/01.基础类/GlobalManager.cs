// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2025-01-08 17:01:37
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using JFramework.Common;
using UnityEngine;
using AssetData = System.Collections.Generic.KeyValuePair<string, string>;
using EnumTable = System.Collections.Generic.Dictionary<System.Enum, JFramework.Common.IData>;
using ItemTable = System.Collections.Generic.Dictionary<int, JFramework.Common.IData>;
using NameTable = System.Collections.Generic.Dictionary<string, JFramework.Common.IData>;
using AgentData = System.Collections.Generic.Dictionary<System.Type, UnityEngine.ScriptableObject>;

namespace JFramework
{
    public class GlobalManager : MonoBehaviour
    {
        public static GlobalManager Instance;

        public static Canvas canvas;

        public static AudioSource audioSource;

        internal static Component entity;

        internal static AssetBundleManifest manifest;

        internal static readonly AudioSetting setting = new AudioSetting();

        internal static readonly List<ITimer> timerData = new List<ITimer>();

        internal static readonly HashSet<AudioSource> audioData = new HashSet<AudioSource>();

        internal static readonly Dictionary<Type, UIPanel> panelData = new Dictionary<Type, UIPanel>();

        internal static readonly Dictionary<Type, ItemTable> itemTable = new Dictionary<Type, ItemTable>();

        internal static readonly Dictionary<Type, NameTable> nameTable = new Dictionary<Type, NameTable>();

        internal static readonly Dictionary<Type, EnumTable> enumTable = new Dictionary<Type, EnumTable>();

        internal static readonly Dictionary<string, PackData> clientPacks = new Dictionary<string, PackData>();

        internal static readonly Dictionary<string, PackData> serverPacks = new Dictionary<string, PackData>();

        internal static readonly Dictionary<string, AssetData> assetData = new Dictionary<string, AssetData>();

        internal static readonly Dictionary<string, AssetBundle> assetPack = new Dictionary<string, AssetBundle>();

        internal static readonly Dictionary<string, Task<AssetBundle>> assetTask = new Dictionary<string, Task<AssetBundle>>();

        internal static readonly Dictionary<string, IPool> poolData = new Dictionary<string, IPool>();

        internal static readonly Dictionary<string, GameObject> poolGroup = new Dictionary<string, GameObject>();

        internal static readonly Dictionary<Component, AgentData> agentData = new Dictionary<Component, AgentData>();

        internal static readonly Dictionary<string, HashSet<UIPanel>> panelGroup = new Dictionary<string, HashSet<UIPanel>>();

        internal static readonly Dictionary<UIPanel, HashSet<string>> groupPanel = new Dictionary<UIPanel, HashSet<string>>();

        internal static readonly Dictionary<UILayer, RectTransform> panelLayer = new Dictionary<UILayer, RectTransform>();

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            JsonManager.Load(setting, nameof(AudioSetting));
        }

        private void Start()
        {
            PackManager.LoadAssetData();
        }

        private void Update()
        {
            TimerManager.Update(Time.time, Time.unscaledTime);
        }

        private void OnDestroy()
        {
            Instance = null;
            UIManager.Dispose();
            PoolManager.Dispose();
            PackManager.Dispose();
            DataManager.Dispose();
            AudioManager.Dispose();
            AssetManager.Dispose();
            AgentManager.Dispose();
            TimerManager.Dispose();
            Service.Pool.Dispose();
            Service.Event.Dispose();
            GC.Collect();
        }

        public static string Reference()
        {
            var builder = Service.Pool.Dequeue<StringBuilder>();
            builder.AppendLine("audioData.Count:" + audioData.Count);
            builder.AppendLine("panelData.Count:" + panelData.Count);
            builder.AppendLine("itemTable.Count:" + itemTable.Count);
            builder.AppendLine("nameTable.Count:" + nameTable.Count);
            builder.AppendLine("enumTable.Count:" + enumTable.Count);
            builder.AppendLine("assetData.Count:" + assetData.Count);
            builder.AppendLine("assetPack.Count:" + assetPack.Count);
            builder.AppendLine("assetTask.Count:" + assetTask.Count);
            builder.AppendLine("poolData.Count:" + poolData.Count);
            builder.AppendLine("poolGroup.Count:" + poolGroup.Count);
            builder.AppendLine("agentData.Count:" + agentData.Count);
            builder.AppendLine("panelGroup.Count:" + panelGroup.Count);
            builder.AppendLine("groupPanel.Count:" + groupPanel.Count);
            builder.AppendLine("panelLayer.Count:" + panelLayer.Count);
            var result = builder.ToString();
            Service.Pool.Enqueue(builder);
            builder.Length = 0;
            return result;
        }
    }
}