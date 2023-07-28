using System;
using JFramework.Interface;
using UnityEngine;

namespace JFramework.Core
{
    [AddComponentMenu(""), DefaultExecutionOrder(-10)]
    public sealed partial class GlobalManager : MonoBehaviour
    {
        /// <summary>
        /// 私有的单例对象
        /// </summary>
        private static GlobalManager instance;

        /// <summary>
        /// 实体Id
        /// </summary>
        private static int entityId;

        /// <summary>
        /// 是否在运行模式
        /// </summary>
        private static bool runtime;

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
        /// 管理器名称
        /// </summary>
        private static string Name => nameof(GlobalManager);

        /// <summary>
        /// 安全的单例调用
        /// </summary>
        internal static GlobalManager Instance
        {
            get
            {
                if (instance != null) return instance;
                instance ??= FindObjectOfType<GlobalManager>();
                var obj = Resources.Load<GlobalManager>(Name);
                instance ??= Instantiate(obj);
                return instance;
            }
        }

        /// <summary>
        /// 进行日志输出
        /// </summary>
        public static bool Runtime
        {
            get
            {
                if (runtime) return runtime;
                Debug.Log($"{Name.Red()} 没有初始化！");
                return false;
            }
        }

        /// <summary>
        /// 全局管理器初始化
        /// </summary>
        private void Awake()
        {
            entityId = 0;
            runtime = true;
            instance ??= this;
            UIManager.Awake();
            PoolManager.Awake();
            DataManager.Awake();
            DateManager.Awake();
            TimerManager.Awake();
            AudioManager.Awake();
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// 全局管理器销毁
        /// </summary>
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
                CtrlManager.Destroy();
                TimerManager.Destroy();
                AudioManager.Destroy();
                EventManager.Destroy();
                SceneManager.Destroy();
                AssetManager.Destroy();
                OnQuit = null;
                OnStart = null;
                OnUpdate = null;
                instance = null;
                entities.Clear();
                GC.Collect();
            }
        }

        /// <summary>
        /// 广播管理器启动的事件
        /// </summary>
        private void Start() => OnStart?.Invoke();

        /// <summary>
        /// 广播管理器更新事件
        /// </summary>
        private void Update() => OnUpdate?.Invoke();

        /// <summary>
        /// 侦听实体的更新事件
        /// </summary>
        /// <param name="entity"></param>
        public static void Listen(IEntity entity)
        {
            if (!Instance) return;
            OnUpdate += entity.Update;
            if (entity.Id == 0) entity.Id = ++entityId;
            entities[entity.Id] = entity;
        }

        /// <summary>
        /// 移除实体的更新
        /// </summary>
        /// <param name="entity"></param>
        public static void Remove(IEntity entity)
        {
            if (!Runtime) return;
            OnUpdate -= entity.Update;
            entities.Remove(entity.Id);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Get<T>(int id) where T : IEntity
        {
            if (!Runtime) return default;
            if (entities.TryGetValue(id, out var entity))
            {
                return (T)entity;
            }

            return default;
        }
    }
}