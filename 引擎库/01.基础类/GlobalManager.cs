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
    public class GlobalManager : MonoBehaviour
    {
        public static GlobalManager Instance;

        public Canvas canvas;

        public AudioSource sounds;
        
        internal static AudioSetting settings;

        internal static AssetBundleManifest manifest;

        internal static readonly List<ITimer> timerData = new List<ITimer>();

        internal static readonly List<AudioSource> audioData = new List<AudioSource>();

        internal static Dictionary<Type, UIPanel> panelData = new Dictionary<Type, UIPanel>();

        internal static Dictionary<Type, ItemTable> itemTable = new Dictionary<Type, ItemTable>();

        internal static Dictionary<Type, NameTable> nameTable = new Dictionary<Type, NameTable>();

        internal static Dictionary<Type, EnumTable> enumTable = new Dictionary<Type, EnumTable>();

        internal static Dictionary<string, PackData> clientPacks = new Dictionary<string, PackData>();

        internal static Dictionary<string, PackData> serverPacks = new Dictionary<string, PackData>();

        internal static Dictionary<string, AssetData> assetData = new Dictionary<string, AssetData>();

        internal static Dictionary<string, AssetBundle> assetPack = new Dictionary<string, AssetBundle>();

        internal static Dictionary<string, Task<AssetBundle>> assetTask = new Dictionary<string, Task<AssetBundle>>();

        internal static Dictionary<string, IPool> poolData = new Dictionary<string, IPool>();

        internal static Dictionary<string, GameObject> poolGroup = new Dictionary<string, GameObject>();

        internal static Dictionary<Component, AgentData> agentData = new Dictionary<Component, AgentData>();

        internal static Dictionary<string, HashSet<UIPanel>> panelGroup = new Dictionary<string, HashSet<UIPanel>>();

        internal static Dictionary<int, RectTransform> panelLayer = new Dictionary<int, RectTransform>();

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
            AudioManager.Dispose();
            AssetManager.Dispose();
            AgentManager.Dispose();
            TimerManager.Dispose();
            PoolManager.Dispose();
            Service.Pool.Dispose();
            Service.Event.Dispose();
            GC.Collect();
        }
    }
}