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
using UnityEngine;

namespace JFramework
{
    [AddComponentMenu(""), DefaultExecutionOrder(-10)]
    public sealed partial class GlobalManager : MonoBehaviour, IEntity
    {
        public static GlobalManager Instance { get; private set; }
        
        public static AssetMode mode => GlobalSetting.Instance.assetMode;

        public static event Action OnUpdate;

        public static event Action OnFixedUpdate;

        public static event Action OnLateUpdate;

        public static event Action OnCheat;

        private void Awake()
        {
            Instance = this;
            UIManager.Register();
            PoolManager.Register();
            InputManager.Register();
            AudioManager.Register();
            TimerManager.Register();
            TweenManager.Register();
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

        private void LateUpdate()
        {
            try
            {
                OnLateUpdate?.Invoke();
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
                Instance = null;
                UIManager.UnRegister();
                DataManager.UnRegister();
                PoolManager.UnRegister();
                InputManager.UnRegister();
                AssetManager.UnRegister();
                AudioManager.UnRegister();
                TweenManager.UnRegister();
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
            OnCheat = null;
            OnUpdate = null;
            OnLateUpdate = null;
            OnFixedUpdate = null;
            GC.Collect();
        }

        internal static void OnAntiCheat()
        {
            OnCheat?.Invoke();
        }
    }

    public sealed partial class GlobalManager
    {
#if UNITY_EDITOR && ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowInInspector]
        private static Dictionary<IEntity, Dictionary<Type, IComponent>> entities = new();

        [Sirenix.OdinInspector.ShowInInspector]
        private static Dictionary<string, IPool<GameObject>> pools = new();

        [Sirenix.OdinInspector.ShowInInspector]
        private static Dictionary<Type, IPool> streams = new();

        [Sirenix.OdinInspector.ShowInInspector]
        private static Dictionary<Type, IEvent> events = new();

        [Sirenix.OdinInspector.ShowInInspector]
        private static Dictionary<string, UIPanel> panels = new();

        [Sirenix.OdinInspector.ShowInInspector]
        private static Dictionary<Type, List<UIPanel>> groups = new();

        [Sirenix.OdinInspector.ShowInInspector]
        private static Dictionary<Type, InputData> inputs = new();

        [Sirenix.OdinInspector.ShowInInspector]
        private static Dictionary<Type, Dictionary<int, IData>> intData = new();

        [Sirenix.OdinInspector.ShowInInspector]
        private static Dictionary<Type, Dictionary<Enum, IData>> enumData = new();

        [Sirenix.OdinInspector.ShowInInspector]
        private static Dictionary<Type, Dictionary<string, IData>> stringData = new();

        [Sirenix.OdinInspector.ShowInInspector]
        private static Dictionary<int, List<Timer>> timers = new();

        [Sirenix.OdinInspector.ShowInInspector]
        private static Dictionary<int, List<Tween>> motions = new();

        [Sirenix.OdinInspector.ShowInInspector]
        private static List<AudioSource> audios = new();

        [Sirenix.OdinInspector.ShowInInspector]
        private static Vector2 audioManager => new Vector2(AudioManager.mainVolume, AudioManager.audioVolume);

        [Sirenix.OdinInspector.ShowInInspector]
        private static Vector2 inputManager => new Vector2(InputManager.horizontal, InputManager.vertical);

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
            panels = (Dictionary<string, UIPanel>)field;
            field = typeof(UIManager).GetField("groups", Reflection.Static)?.GetValue(null);
            groups = (Dictionary<Type, List<UIPanel>>)field;
            field = typeof(InputManager).GetField("inputs", Reflection.Static)?.GetValue(null);
            inputs = (Dictionary<Type, InputData>)field;
            field = typeof(DataManager).GetField("intData", Reflection.Static)?.GetValue(null);
            intData = (Dictionary<Type, Dictionary<int, IData>>)field;
            field = typeof(DataManager).GetField("enumData", Reflection.Static)?.GetValue(null);
            enumData = (Dictionary<Type, Dictionary<Enum, IData>>)field;
            field = typeof(DataManager).GetField("stringData", Reflection.Static)?.GetValue(null);
            stringData = (Dictionary<Type, Dictionary<string, IData>>)field;
            field = typeof(AudioManager).GetField("audios", Reflection.Static)?.GetValue(null);
            audios = (List<AudioSource>)field;
            field = typeof(TimerManager).GetField("timers", Reflection.Static)?.GetValue(null);
            timers = (Dictionary<int, List<Timer>>)field;
            field = typeof(TweenManager).GetField("motions", Reflection.Static)?.GetValue(null);
            motions = (Dictionary<int, List<Tween>>)field;
        }

        public static void EditorWindow(string path, object editor) => EditorSetting.editors[path] = editor;
#endif
    }
}