using System;
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
            TimerManager.Awake();
            DateManager.Awake();
            AudioManager.Awake();
            PoolManager.Awake();
            DataManager.Awake();
            UIManager.Awake();
        }

        private void Start() => OnStart?.Invoke();
        private void Update() => OnUpdate?.Invoke();

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
                instance = null;
                OnStart = null;
                OnUpdate = null;
                OnQuit = null;
                GC.Collect();
            }
        }
    }
}