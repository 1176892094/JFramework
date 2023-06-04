using System;
using UnityEngine;

namespace JFramework.Core
{
    /// <summary>
    /// 全局控制器
    /// </summary>
    public sealed partial class GlobalManager : MonoBehaviour
    {
        /// <summary>
        /// 全局管理器单例
        /// </summary>
        public static GlobalManager Instance;
        
        /// <summary>
        /// 全局管理器名称
        /// </summary>
        private static string Name => nameof(GlobalManager);

        /// <summary>
        /// 是否在运行
        /// </summary>
        private static bool runtime;
        
        /// <summary>
        /// 打印Runtime信息
        /// </summary>
        public static bool Runtime
        {
            get
            {
                if (!runtime)
                {
                    Debug.Log($"{Name.Red()} 没有初始化！");
                }

                return runtime;
            }
        }

        /// <summary>
        /// Start开始事件
        /// </summary>
        public static event Action OnStart;

        /// <summary>
        /// Update更新事件
        /// </summary>
        public static event Action OnUpdate;

        /// <summary>
        /// Destroy销毁事件
        /// </summary>
        public static event Action OnDestroy;

        /// <summary>
        /// 全局管理器醒来
        /// </summary>
        private void Awake() => DontDestroyOnLoad(gameObject);

        /// <summary>
        /// 全局管理器开始
        /// </summary>
        private void Start() => OnStart?.Invoke();

        /// <summary>
        /// 全局Update更新
        /// </summary>
        private void Update() => OnUpdate?.Invoke();

        /// <summary>
        /// 设置全局单例
        /// </summary>
        private static void Singleton()
        {
            runtime = true;
            Instance ??= FindObjectOfType<GlobalManager>();
            Instance ??= Instantiate(Resources.Load<GlobalManager>(Name));
        }

        /// <summary>
        /// 注册管理器
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Register()
        {
            Singleton();
            CommandManager.Awake();
            AssetManager.Awake();
            EventManager.Awake();
            TimerManager.Awake();
            JsonManager.Awake();
            DateManager.Awake();
            AudioManager.Awake();
            PoolManager.Awake();
            SceneManager.Awake();
            DataManager.Awake();
            UIManager.Awake();
        }

        /// <summary>
        /// 当程序退出
        /// </summary>
        private void OnApplicationQuit()
        {
            runtime = false;
            OnDestroy?.Invoke();
            UIManager.Destroy();
            PoolManager.Destroy();
            DataManager.Destroy();
            DateManager.Destroy();
            JsonManager.Destroy();
            SceneManager.Destroy();
            TimerManager.Destroy();
            AudioManager.Destroy();
            AssetManager.Destroy();
            EventManager.Destroy();
            CommandManager.Destroy();
            Instance = null;
            OnStart = null;
            OnUpdate = null;
            OnDestroy = null;
        }
    }
    
    [Flags]
    internal enum DebugOption
    {
        None = 0,
        Json = 1,
        Pool = 2,
        Data = 4,
        Scene = 8,
        Asset = 16,
        Audio = 32,
        Event = 64,
        Timer = 128,
        Custom = 256,
    }
}