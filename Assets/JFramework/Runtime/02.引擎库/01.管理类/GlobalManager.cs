// // *********************************************************************************
// // # Project: JFramework
// // # Unity: 6000.3.5f1
// // # Author: 云谷千羽
// // # Version: 1.0.0
// // # History: 2025-04-09 21:04:41
// // # Recently: 2025-04-09 21:04:41
// // # Copyright: 2024, 云谷千羽
// // # Description: This is an automatically generated comment.
// // *********************************************************************************

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
    public class GlobalManager : MonoBehaviour
    {
        public static GlobalManager Instance;
        
        public Canvas canvas;

        public AudioSource sounds;
        
        internal static AudioSetting settings;
        
        internal static AssetBundleManifest manifest;
        
        internal static readonly List<ITimer> timerData = new List<ITimer>();
        
        internal static readonly List<AudioSource> audioData = new List<AudioSource>();
        
        internal static readonly Dictionary<string, PackData> clientPacks = new Dictionary<string, PackData>();

        internal static readonly Dictionary<string, PackData> serverPacks = new Dictionary<string, PackData>();
        
        internal static readonly Dictionary<string, AssetData> assetData = new Dictionary<string, AssetData>();

        internal static readonly Dictionary<string, AssetBundle> assetPack = new Dictionary<string, AssetBundle>();

        internal static readonly Dictionary<string, Task<AssetBundle>> assetTask = new Dictionary<string, Task<AssetBundle>>();
        
        internal static readonly Dictionary<string, string> assetPath = new Dictionary<string, string>();
        
        internal static readonly Dictionary<Type, ItemTable> itemTable = new Dictionary<Type, ItemTable>();

        internal static readonly Dictionary<Type, NameTable> nameTable = new Dictionary<Type, NameTable>();

        internal static readonly Dictionary<Type, EnumTable> enumTable = new Dictionary<Type, EnumTable>();
        
        internal static readonly Dictionary<string, IPool> poolData = new Dictionary<string, IPool>();

        internal static readonly Dictionary<string, GameObject> poolGroup = new Dictionary<string, GameObject>();
        
        internal static readonly Dictionary<Component, AgentData> agentData = new Dictionary<Component, AgentData>();
        
        internal static readonly Dictionary<Type, UIPanel> panelData = new Dictionary<Type, UIPanel>();
        
        internal static readonly Dictionary<int, RectTransform> panelLayer = new Dictionary<int, RectTransform>();
        
        internal static readonly Dictionary<string, HashSet<UIPanel>> panelGroup = new Dictionary<string, HashSet<UIPanel>>();
        
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
            Instance = null;
            UIManager.Dispose();
            PackManager.Dispose();
            DataManager.Dispose();
            EventManager.Dispose();
            AudioManager.Dispose();
            AssetManager.Dispose();
            AgentManager.Dispose();
            TimerManager.Dispose();
            EntityManager.Dispose();
            PoolManager.Dispose();
            GC.Collect();
        }
    }
}