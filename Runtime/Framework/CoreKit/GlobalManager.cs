// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-02-04  13:16
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using JFramework.Interface;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JFramework.Core
{
    public sealed partial class GlobalManager
    {
        public static bool isRemote => SettingManager.Instance.remoteLoad;
        public static AssetPlatform platform => SettingManager.Instance.platform;
        [ShowInInspector] private static Dictionary<Type, IPool> activators => PoolManager.activators;
        [ShowInInspector] private static Dictionary<IEntity, Dictionary<Type, IComponent>> entities => EntityManager.entities;
        [ShowInInspector] private static Dictionary<Type, UIPanel> panels => UIManager.panels;
        [ShowInInspector] private static Dictionary<Type, IEvent> events => EventManager.observers;
        [ShowInInspector] private static Dictionary<Type, IEntity> scenes => SceneManager.objects;
        [ShowInInspector] private static Dictionary<string, IPool<GameObject>> objects => PoolManager.pools;
        [ShowInInspector] private static Dictionary<Type, Dictionary<int, IData>> intData => DataManager.intData;
        [ShowInInspector] private static Dictionary<Type, Dictionary<Enum, IData>> enumData => DataManager.enumData;
        [ShowInInspector] private static Dictionary<Type, Dictionary<string, IData>> stringData => DataManager.stringData;
        [ShowInInspector] private static List<Timer> timerPlay => TimerManager.timers;
        [ShowInInspector] private static List<AudioClip> audioStop => AudioManager.stops.Select(source => source.clip).ToList();
        [ShowInInspector] private static List<AudioClip> audioPlay => AudioManager.plays.Select(source => source.clip).ToList();
        [ShowInInspector] private static float audioVolume => AudioManager.audioValue;
        [ShowInInspector] private static float soundVolume => AudioManager.soundValue;
#if UNITY_EDITOR
        public static void EditorWindow(string path, object editor) => EditorSetting.editors[path] = editor;
#endif
    }

    [AddComponentMenu(""), DefaultExecutionOrder(-10)]
    public sealed partial class GlobalManager : MonoBehaviour, IEntity
    {
        public static GlobalManager Instance { get; private set; }

        public static event Action OnStart;

        public static event Action OnUpdate;

        public static event Action OnQuit;

        public static event Action OnCheat;

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            UIManager.Register();
            JsonManager.Register();
            PoolManager.Register();
            AudioManager.Register();
            TimerManager.Register();
        }

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

        private void OnDisable()
        {
            UIManager.UnRegister();
            DataManager.UnRegister();
            PoolManager.UnRegister();
            AssetManager.UnRegister();
            AudioManager.UnRegister();
            SceneManager.UnRegister();
            TimerManager.UnRegister();
            EventManager.UnRegister();
            EntityManager.UnRegister();
            RequestManager.UnRegister();
        }

        private void OnApplicationQuit()
        {
            try
            {
                Instance = null;
                OnQuit?.Invoke();
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }

        private void OnDestroy()
        {
            OnQuit = null;
            OnStart = null;
            OnUpdate = null;
            OnCheat = null;
            GC.Collect();
        }

        public static void Cheat()
        {
            Debug.LogWarning("检查到作弊！");
            OnCheat?.Invoke();
        }
    }
}