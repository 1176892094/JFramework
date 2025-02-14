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
using System.Threading.Tasks;
using JFramework.Common;
using UnityEngine;
using AssetData = System.Collections.Generic.KeyValuePair<string, string>;
using EnumTable = System.Collections.Generic.Dictionary<System.Enum, JFramework.Common.IData>;
using ItemTable = System.Collections.Generic.Dictionary<int, JFramework.Common.IData>;
using NameTable = System.Collections.Generic.Dictionary<string, JFramework.Common.IData>;
using AgentData = System.Collections.Generic.Dictionary<System.Type, JFramework.Common.IAgent>;

namespace JFramework
{
    public abstract class GlobalManager : MonoBehaviour
    {
        public static GlobalManager Instance;

        public Canvas canvas;

        public AudioSource sounds;
        
        internal static AudioSetting settings;

        internal static AssetBundleManifest manifest;

        internal static readonly List<ITimer> timerData = new List<ITimer>();

        internal static readonly List<AudioSource> audioData = new List<AudioSource>();

        internal static Dictionary<Type, UIPanel> panelData => Instance.m_PanelData;

        internal static Dictionary<Type, ItemTable> itemTable => Instance.m_ItemTable;

        internal static Dictionary<Type, NameTable> nameTable => Instance.m_NameTable;

        internal static Dictionary<Type, EnumTable> enumTable => Instance.m_EnumTable;

        internal static Dictionary<string, PackData> clientPacks => Instance.m_ClientPacks;

        internal static Dictionary<string, PackData> serverPacks => Instance.m_ServerPacks;

        internal static Dictionary<string, AssetData> assetData => Instance.m_AssetData;

        internal static Dictionary<string, AssetBundle> assetPack => Instance.m_AssetPack;

        internal static Dictionary<string, Task<AssetBundle>> assetTask => Instance.m_AssetTask;

        internal static Dictionary<string, IPool> poolData => Instance.m_PoolData;

        internal static Dictionary<string, GameObject> poolGroup => Instance.m_PoolGroup;

        internal static Dictionary<Component, AgentData> agentData => Instance.m_AgentData;

        internal static Dictionary<string, HashSet<UIPanel>> panelGroup => Instance.m_PanelGroup;

        internal static Dictionary<int, RectTransform> panelLayer => Instance.m_PanelLayer;

        protected abstract Dictionary<Type, UIPanel> m_PanelData { get; }

        protected abstract Dictionary<Type, ItemTable> m_ItemTable { get; }

        protected abstract Dictionary<Type, NameTable> m_NameTable { get; }

        protected abstract Dictionary<Type, EnumTable> m_EnumTable { get; }

        protected abstract Dictionary<string, PackData> m_ClientPacks { get; }

        protected abstract Dictionary<string, PackData> m_ServerPacks { get; }

        protected abstract Dictionary<string, AssetData> m_AssetData { get; }

        protected abstract Dictionary<string, AssetBundle> m_AssetPack { get; }

        protected abstract Dictionary<string, Task<AssetBundle>> m_AssetTask { get; }

        protected abstract Dictionary<string, IPool> m_PoolData { get; }

        protected abstract Dictionary<string, GameObject> m_PoolGroup { get; }

        protected abstract Dictionary<Component, AgentData> m_AgentData { get; }

        protected abstract Dictionary<string, HashSet<UIPanel>> m_PanelGroup { get; }

        protected abstract Dictionary<int, RectTransform> m_PanelLayer { get; }

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            PackManager.LoadAssetData();
        }

        private void Update()
        {
            AgentManager.Update();
            TimerManager.Update();
        }

        private void OnDestroy()
        {
            UIManager.Dispose();
            PackManager.Dispose();
            DataManager.Dispose();
            AudioManager.Dispose();
            AssetManager.Dispose();
            AgentManager.Dispose();
            TimerManager.Dispose();
            PoolManager.Dispose();
            typeof(Service.Pool).GetMethod("Dispose", Service.Find.Static)?.Invoke(null, null);
            typeof(Service.Event).GetMethod("Dispose", Service.Find.Static)?.Invoke(null, null);
            Instance = null;
            GC.Collect();
        }
    }
}