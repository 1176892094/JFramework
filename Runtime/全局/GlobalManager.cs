using System;
using UnityEngine;

namespace JFramework.Core
{
    [AddComponentMenu("")]
    public sealed partial class GlobalManager : MonoBehaviour
    {
        /// <summary>
        /// 全局管理器的单例
        /// </summary>
        internal static GlobalManager Instance;
        
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
        public static event Action OnDestroy;
        
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
                    Log.Info(DebugOption.Global, "没有初始化！".Red());
                }

                return runtime;
            }
        }

        private void Awake() => DontDestroyOnLoad(gameObject);
        private void Start() => OnStart?.Invoke();
        private void Update() => OnUpdate?.Invoke();

        /// <summary>
        /// 在场景加载之前进行初始化
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Register()
        {
            runtime = true;
            Instance ??= FindObjectOfType<GlobalManager>();
            Instance ??= Instantiate(Resources.Load<GlobalManager>(nameof(GlobalManager)));
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
        /// 程序退出时执行的方法
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
}