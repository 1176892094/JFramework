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
using JFramework.Interface;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JFramework.Core
{
    [AddComponentMenu(""), DefaultExecutionOrder(-10)]
    public sealed partial class GlobalManager : MonoBehaviour, IEntity
    {
        public static GlobalManager Instance { get; private set; }

        public static event Action OnStart;

        public static event Action OnUpdate;

        public static event Action OnFixedUpdate;

        public static event Action OnQuit;

        public static event Action<bool> OnPause;

        public static event Action OnCheat;

        private void Awake()
        {
            Instance = this;
            UIManager.Register();
            JsonManager.Register();
            PoolManager.Register();
            InputManager.Register();
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

        private void FixedUpdate()
        {
            try
            {
                OnFixedUpdate?.Invoke();
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            try
            {
                OnPause?.Invoke(pauseStatus);
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }

        private void OnApplicationQuit()
        {
            try
            {
                OnQuit?.Invoke();
                Instance = null;
                UIManager.UnRegister();
                DataManager.UnRegister();
                PoolManager.UnRegister();
                InputManager.UnRegister();
                AssetManager.UnRegister();
                AudioManager.UnRegister();
                SceneManager.UnRegister();
                TimerManager.UnRegister();
                EventManager.UnRegister();
                EntityManager.UnRegister();
                BundleManager.UnRegister();
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
            OnFixedUpdate = null;
            OnCheat = null;
            GC.Collect();
        }

        public static void Cheat()
        {
            Debug.LogWarning("检查到作弊！");
            OnCheat?.Invoke();
        }
    }

    public sealed partial class GlobalManager
    {
        public static AssetMode mode => SettingManager.Instance.assetMode;
        public static AssetPlatform platform => SettingManager.Instance.platform;
#if UNITY_EDITOR
        [ShowInInspector] private static Dictionary<IEntity, Dictionary<Type, IComponent>> entities = new();
        [ShowInInspector] private static Dictionary<string, IPool<GameObject>> pools = new();
        [ShowInInspector] private static Dictionary<Type, IPool> streams = new();
        [ShowInInspector] private static Dictionary<Type, IEvent> events = new();
        [ShowInInspector] private static Dictionary<Type, UIPanel> panels = new();
        [ShowInInspector] private static Dictionary<Type, IEntity> objects = new();
        [ShowInInspector] private static Dictionary<Type, InputManager.InputData> inputs = new();
        [ShowInInspector] private static Dictionary<Type, Dictionary<int, IData>> intData = new();
        [ShowInInspector] private static Dictionary<Type, Dictionary<Enum, IData>> enumData = new();
        [ShowInInspector] private static Dictionary<Type, Dictionary<string, IData>> stringData = new();
        [ShowInInspector] private static Dictionary<GameObject, AudioSource> audios = new();
        [ShowInInspector] private static List<Timer> timers = new();
        [ShowInInspector] private static float audioVolume => AudioManager.mainVolume;
        [ShowInInspector] private static float soundVolume => AudioManager.audioVolume;
        [ShowInInspector] private static float horizontal => InputManager.horizontal;
        [ShowInInspector] private static float vertical => InputManager.vertical;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void RuntimeInitializeOnLoad()
        {
            var field = typeof(PoolManager).GetField("streams", Reflection.Static)?.GetValue(null);
            streams = (Dictionary<Type, IPool>)field;
            field = typeof(PoolManager).GetField("pools", Reflection.Static)?.GetValue(null);
            pools = (Dictionary<string, IPool<GameObject>>)field;
            field = typeof(EntityManager).GetField("entities", Reflection.Static)?.GetValue(null);
            entities = (Dictionary<IEntity, Dictionary<Type, IComponent>>)field;
            field = typeof(EventManager).GetField("events", Reflection.Static)?.GetValue(null);
            events = (Dictionary<Type, IEvent>)field;
            field = typeof(UIManager).GetField("panels", Reflection.Static)?.GetValue(null);
            panels = (Dictionary<Type, UIPanel>)field;
            field = typeof(InputManager).GetField("inputs", Reflection.Static)?.GetValue(null);
            inputs = (Dictionary<Type, InputManager.InputData>)field;
            field = typeof(DataManager).GetField("intData", Reflection.Static)?.GetValue(null);
            intData = (Dictionary<Type, Dictionary<int, IData>>)field;
            field = typeof(DataManager).GetField("enumData", Reflection.Static)?.GetValue(null);
            enumData = (Dictionary<Type, Dictionary<Enum, IData>>)field;
            field = typeof(DataManager).GetField("stringData", Reflection.Static)?.GetValue(null);
            stringData = (Dictionary<Type, Dictionary<string, IData>>)field;
            field = typeof(AudioManager).GetField("audios", Reflection.Static)?.GetValue(null);
            audios = (Dictionary<GameObject, AudioSource>)field;
            field = typeof(TimerManager).GetField("timers", Reflection.Static)?.GetValue(null);
            timers = (List<Timer>)field;
        }

        public static void EditorWindow(string path, object editor) => EditorSetting.editors[path] = editor;
#endif
    }
}