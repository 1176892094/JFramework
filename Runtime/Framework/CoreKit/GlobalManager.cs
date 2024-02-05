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
using JFramework.Core;
using JFramework.Interface;
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
            entity = ScriptableObject.CreateInstance<EntityManager>();
            entity.name = nameof(EntityManager);
            this.Inject();
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
            Destroy(entity);
            GC.Collect();
        }
    }

    public sealed partial class GlobalManager
    {
        public static bool isRemote => SettingManager.Instance.remoteLoad;

        [SerializeField] private EntityManager entity;
        internal static EntityManager Entity => Instance.entity;

        [Inject, SerializeField] private UIManager view;
        public static UIManager UI => Instance.view;

        [Inject, SerializeField] private JsonManager json;
        public static JsonManager Json => Instance.json;

        [Inject, SerializeField] private PoolManager pool;
        public static PoolManager Pool => Instance.pool;

        [Inject, SerializeField] private DataManager data;
        public static DataManager Data => Instance.data;

        [Inject, SerializeField] private AssetManager asset;
        public static AssetManager Asset => Instance.asset;

        [Inject, SerializeField] private SceneManager scene;
        public static SceneManager Scene => Instance.scene;

        [Inject, SerializeField] private TimerManager timer;
        public static TimerManager Time => Instance.timer;

        [Inject, SerializeField] private AudioManager audios;
        public static AudioManager Audio => Instance.audios;

        [Inject, SerializeField] private EventManager events;
        public static EventManager Event => Instance.events;

        [Inject, SerializeField] private RequestManager request;
        public static RequestManager Request => Instance.request;
    }
}