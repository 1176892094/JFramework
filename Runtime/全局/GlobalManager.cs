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
        /// 全局管理器名称
        /// </summary>
        private static string Name => nameof(GlobalManager);

        /// <summary>
        /// 受保护的ChatGPT类
        /// </summary>
        private static ChatGPT chatGpt;
        
        /// <summary>
        /// 公开的ChatGPT类
        /// </summary>
        private static ChatGPT ChatGpt => chatGpt ??= new ChatGPT();

        /// <summary>
        /// 全局管理器单例
        /// </summary>
        public static GlobalManager Instance;

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
            if (Instance != null) return;
            Instance = FindObjectOfType<GlobalManager>();
            if (Instance != null) return;
            var obj = Resources.Load<GameObject>(Name);
            Instance = Instantiate(obj).GetComponent<GlobalManager>();
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