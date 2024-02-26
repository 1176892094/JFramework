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
using JFramework.Core;
using JFramework.Interface;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JFramework
{
    [AddComponentMenu(""), DefaultExecutionOrder(-10)]
    public sealed partial class GlobalManager : MonoBehaviour, IEntity
    {
        public static GlobalManager Instance { get; private set; }

        public static event Action OnStart;

        public static event Action OnUpdate;

        public static event Action OnQuit;

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            json.OnEnable();
            pool.OnEnable();
            sound.OnEnable();
            timer.OnEnable();
            panel.OnEnable();
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
            data.OnDisable();
            json.OnDisable();
            pool.OnDisable();
            asset.OnDisable();
            sound.OnDisable();
            scene.OnDisable();
            timer.OnDisable();
            panel.OnDisable();
            entity.OnDisable();
            @event.OnDisable();
            request.OnDisable();
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
            GC.Collect();
        }
    }

    public sealed partial class GlobalManager
    {
        public static bool isRemote => SettingManager.Instance.remoteLoad;

        [ShowInInspector] private Dictionary<Type, IPool> stream => StreamPool.streams;

        [SerializeField] private EntityManager entity;
        internal static EntityManager Entity => Instance.entity;

        [SerializeField] private UIManager panel;
        public static UIManager UI => Instance.panel;

        [SerializeField] private JsonManager json;
        public static JsonManager Json => Instance.json;

        [SerializeField] private PoolManager pool;
        public static PoolManager Pool => Instance.pool;

        [SerializeField] private DataManager data;
        public static DataManager Data => Instance.data;

        [SerializeField] private AssetManager asset;
        public static AssetManager Asset => Instance.asset;

        [SerializeField] private SceneManager scene;
        public static SceneManager Scene => Instance.scene;

        [SerializeField] private TimerManager timer;
        public static TimerManager Time => Instance.timer;

        [SerializeField] private AudioManager sound;
        public static AudioManager Audio => Instance.sound;

        [SerializeField] private EventManager @event;
        public static EventManager Event => Instance.@event;

        [SerializeField] private RequestManager request;
        public static RequestManager Request => Instance.request;
    }
}