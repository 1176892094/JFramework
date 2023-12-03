// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-10-24  23:45
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using JFramework.Interface;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using JFramework.Editor;
#endif

// ReSharper disable All

namespace JFramework.Core
{
    using Components = Dictionary<Type, ScriptableObject>;

    [AddComponentMenu(""), DefaultExecutionOrder(-10)]
    public sealed partial class GlobalManager : MonoBehaviour
    {
        /// <summary>
        /// 安全的单例调用
        /// </summary>
        internal static GlobalManager Instance { get; private set; }

        /// <summary>
        /// 是否在运行模式
        /// </summary>
        public static bool Runtime { get; private set; }

        /// <summary>
        /// 全局管理器开始事件
        /// </summary>
        public static event Action OnStart;

        /// <summary>
        /// 全局管理器更新事件
        /// </summary>
        public static event Action OnUpdate;

        /// <summary>
        /// 应用程序退出事件
        /// </summary>
        public static event Action OnQuit;

        /// <summary>
        /// 全局管理器初始化
        /// </summary>
        private void Awake()
        {
            Runtime = true;
            Instance = this;
            UIManager.Register();
            PoolManager.Register();
            AudioManager.Register();
            TimerManager.Register();
        }

        /// <summary>
        /// 全剧管理器开始事件
        /// </summary>
        private void Start()
        {
            try
            {
                OnStart?.Invoke();
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }

        /// <summary>
        /// 广播管理器更新事件
        /// </summary>
        private void Update()
        {
            try
            {
                OnUpdate?.Invoke();
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }

        /// <summary>
        /// 当应用退出
        /// </summary>
        private void OnApplicationQuit()
        {
            try
            {
                Runtime = false;
                Instance = null;
                OnQuit?.Invoke();
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }

        /// <summary>
        /// 全剧管理器销毁
        /// </summary>
        private void OnDestroy()
        {
            Dispose();
            OnQuit = null;
            OnStart = null;
            OnUpdate = null;
            UIManager.UnRegister();
            PoolManager.UnRegister();
            AudioManager.UnRegister();
            TimerManager.UnRegister();
            GC.Collect();
        }

        /// <summary>
        /// 在游戏运行前初始化静态类
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Dispose()
        {
            StreamPool.Dispose();
            JsonManager.Dispose();
            DataManager.Dispose();
            SceneManager.Dispose();
            EventManager.Dispose();
            AssetManager.Dispose();
            AssetRequest.Dispose();
            EntityManager.Dispose();
        }
    }

#if UNITY_EDITOR
    public sealed partial class GlobalManager
    {
        public static bool isRemote => BuildSetting.Instance.isRemote;
        public static void Add(int id, string name, object item) => EditorSetting.Add(id, name, item);

        [ShowInInspector, LabelText("数据池")]
        private Dictionary<Type, IPool> streams => StreamPool.streams;

        [ShowInInspector, LabelText("对象池")]
        private Dictionary<string, IPool> pools => PoolManager.pools;
        
        [ShowInInspector, LabelText("实体单位")]
        private Dictionary<IEntity, Components> entities => EntityManager.entities;

        [ShowInInspector, LabelText("动态资源")]
        private Dictionary<string, Asset> assets => AssetManager.assets;
        
        [ShowInInspector, LabelText("事件中心")]
        private Dictionary<Type, HashSet<IEvent>> observers => EventManager.observers;

        [ShowInInspector, LabelText("用户界面")]
        private Dictionary<Type, IPanel> panels => UIManager.panels;

        [ShowInInspector, LabelText("数据表 (int)")]
        private Dictionary<Type, Dictionary<int, IData>> intDataTable => Data<int>.dataTable;

        [ShowInInspector, LabelText("数据表 (enum)")]
        private Dictionary<Type, Dictionary<Enum, IData>> enmDataTable => Data<Enum>.dataTable;

        [ShowInInspector, LabelText("数据表 (string)")]
        private Dictionary<Type, Dictionary<string, IData>> strDataTable => Data<string>.dataTable;

        [ShowInInspector, LabelText("计时器 (已完成)")]
        private Queue<ITimer> queues => TimerManager.queues;

        [ShowInInspector, LabelText("计时器 (运行中)")]
        private LinkedList<ITimer> timers => TimerManager.timers;

        [ShowInInspector, LabelText("游戏音效 (已完成)")]
        private List<AudioClip> stacks => AudioManager.stacks.Select(audio => audio.clip).ToList();

        [ShowInInspector, LabelText("游戏音效 (播放中)")]
        private List<AudioClip> audios => AudioManager.audios.Select(audio => audio.clip).ToList();
        
        [ShowInInspector, LabelText("背景音乐大小")]
        private float soundVolume => AudioManager.audioData.musicVolume;

        [ShowInInspector, LabelText("游戏音效大小")]
        private float audioVolume => AudioManager.audioData.audioVolume;
    }
#endif
}