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
        /// 是否已经初始化
        /// </summary>
        private static bool initialize;

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
        /// 安全的单例调用
        /// </summary>
        internal static GlobalManager Instance
        {
            get
            {
                if (instance != null) return instance;
                instance ??= FindObjectOfType<GlobalManager>();
                var obj = Resources.Load<GlobalManager>(nameof(GlobalManager));
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
                if (initialize) return initialize;
                Debug.Log($"{nameof(GlobalManager).Red()} 没有初始化！");
                return initialize;
            }
        }

        /// <summary>
        /// 全局管理器初始化
        /// </summary>
        private void Awake()
        {
            initialize = true;
            DontDestroyOnLoad(gameObject);
            UIManager.Awake();
            PoolManager.Awake();
            DataManager.Awake();
            AudioManager.Awake();
            TimerManager.Awake();
            AssetManager.Awake();
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
        /// 当应用退出
        /// </summary>
        private void OnApplicationQuit()
        {
            try
            {
                initialize = false;
                OnQuit?.Invoke();
            }
            finally
            {
                entities.Clear();
            }
        }

        /// <summary>
        /// 全局管理器销毁
        /// </summary>
        private void OnDestroy()
        {
            UIManager.Destroy();
            DataManager.Destroy();
            TimerManager.Destroy();
            AudioManager.Destroy();
            RuntimeInitializeOnLoad();
            GC.Collect();
        }

        /// <summary>
        /// 侦听实体的更新事件
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="gameObject"></param>
        public static void Listen(IEntity entity, GameObject gameObject)
        {
            if (!Instance) return;
            OnUpdate += entity.Update;
            entities[entity] = gameObject;
        }

        /// <summary>
        /// 移除实体的更新
        /// </summary>
        /// <param name="entity"></param>
        public static void Remove(IEntity entity)
        {
            if (!Runtime) return;
            OnUpdate -= entity.Update;
            entities.Remove(entity);
        }

        /// <summary>
        /// 获取组件
        /// </summary>
        /// <param name="entity"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Get<T>(IEntity entity)
        {
            if (!Runtime) return default;
            return entities.TryGetValue(entity, out var @object) ? @object.GetComponent<T>() : default;
        }

        /// <summary>
        /// 获取游戏对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static GameObject Get(IEntity entity)
        {
            if (!Runtime) return default;
            return entities.TryGetValue(entity, out var @object) ? @object : null;
        }

        /// <summary>
        /// 在场景加载之前重置
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void RuntimeInitializeOnLoad()
        {
            OnQuit = null;
            OnStart = null;
            OnUpdate = null;
            instance = null;
            initialize = false;
            CtrlManager.RuntimeInitializeOnLoad();
            PoolManager.RuntimeInitializeOnLoad();
            EventManager.RuntimeInitializeOnLoad();
            SceneManager.RuntimeInitializeOnLoad();
            AssetManager.RuntimeInitializeOnLoad();
        }
    }
}