using System;
using System.Collections.Generic;
using JFramework.Interface;
using UnityEngine;

namespace JFramework.Core
{
    [AddComponentMenu(""), DefaultExecutionOrder(-10)]
    public sealed partial class GlobalManager : MonoBehaviour
    {
        /// <summary>
        /// 实体字典
        /// </summary>
        private static readonly Dictionary<int, IEntity> entities = new Dictionary<int, IEntity>();

        /// <summary>
        /// 私有的单例对象
        /// </summary>
        private static GlobalManager instance;

        /// <summary>
        /// 安全的单例调用
        /// </summary>
        internal static GlobalManager Instance
        {
            get
            {
                if (instance != null) return instance;
                instance ??= FindObjectOfType<GlobalManager>();
                instance ??= Instantiate(Resources.Load<GlobalManager>(nameof(GlobalManager)));
                return instance;
            }
        }

        /// <summary>
        /// 对象池管理器
        /// </summary>
        internal static Transform poolManager;

        /// <summary>
        /// 全局管理器开始事件
        /// </summary>
        public static event Action OnStart;

        /// <summary>
        /// 全局管理器更新事件
        /// </summary>
        public static event Action OnUpdate;

        /// <summary>
        /// 全局管理器销毁事件
        /// </summary>
        public static event Action OnQuit;

        /// <summary>
        /// 实体Id
        /// </summary>
        private static int entityId;

        /// <summary>
        /// 是否在运行模式
        /// </summary>
        private static bool runtime;

        /// <summary>
        /// 进行日志输出
        /// </summary>
        public static bool Runtime
        {
            get
            {
                if (!runtime)
                {
                    Debug.Log($"{nameof(GlobalManager).Red()} 没有初始化！");
                }

                return runtime;
            }
        }

        private void Awake()
        {
            runtime = true;
            instance ??= this;
            DontDestroyOnLoad(gameObject);
            poolManager = transform.Find("PoolManager");
            TimerManager.Awake();
            DateManager.Awake();
            AudioManager.Awake();
            DataManager.Awake();
            UIManager.Awake();
        }

        private void Start() => OnStart?.Invoke();
        private void Update() => OnUpdate?.Invoke();

        public static void Listen(IEntity entity)
        {
            if (!Instance) return;
            OnUpdate += entity.Update;
            if (entity.Id == 0) entity.Id = ++entityId;
            entities[entity.Id] = entity;
        }

        public static T Get<T>(int id) where T : IEntity
        {
            return entities.TryGetValue(id, out var entity) ? (T)entity : default;
        }

        public static void Remove(IEntity entity)
        {
            if (!Runtime) return;
            OnUpdate -= entity.Update;
            entities.Remove(entity.Id);
        }

        private void OnDestroy()
        {
            try
            {
                runtime = false;
                OnQuit?.Invoke();
            }
            finally
            {
                UIManager.Destroy();
                PoolManager.Destroy();
                DataManager.Destroy();
                DateManager.Destroy();
                SceneManager.Destroy();
                TimerManager.Destroy();
                AudioManager.Destroy();
                AssetManager.Destroy();
                EventManager.Destroy();
                Controllers.Destroy();
                entities.Clear();
                instance = null;
                OnStart = null;
                OnUpdate = null;
                OnQuit = null;
                entityId = 0;
                GC.Collect();
            }
        }
    }
}