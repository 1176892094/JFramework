using System;
using UnityEngine;

// ReSharper disable All

namespace JFramework.Core
{
    [AddComponentMenu(""), DefaultExecutionOrder(-10)]
    public sealed partial class GlobalManager : MonoBehaviour
    {
        /// <summary>
        /// 安全的单例调用
        /// </summary>
        internal static GlobalManager Instance { get; private set; }
        
        /// <summary>
        /// 是否在运行模式
        /// </summary>
        public static bool Runtime { get; private set; }

        /// <summary>
        /// 全局管理器开始事件
        /// </summary>
        public static event Action OnStart;

        /// <summary>
        /// 全局管理器更新事件
        /// </summary>
        public static event Action OnUpdate;
        
        /// <summary>
        /// 应用程序退出事件
        /// </summary>
        public static event Action OnQuit;

        /// <summary>
        /// 全局管理器初始化
        /// </summary>
        private void Awake()
        {
            Runtime = true;
            Instance = this;
            Register();
        }

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
            Runtime = false;
            Instance = null;
            OnQuit?.Invoke();
            OnStart = null;
            OnUpdate = null;
            OnQuit = null;
        }

        private void OnDestroy()
        {
            Clear();
            UnRegister();
            GC.Collect();
        }

        /// <summary>
        /// 注册
        /// </summary>
        private static void Register()
        {
            UIManager.Register();
            PoolManager.Register();
            AudioManager.Register();
            TimerManager.Register();
        }

        /// <summary>
        /// 取消注册
        /// </summary>
        private static void UnRegister()
        {
            UIManager.UnRegister();
            PoolManager.UnRegister();
            AudioManager.UnRegister();
            TimerManager.UnRegister();
        }

        /// <summary>
        /// 在游戏运行前初始化静态类
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Clear()
        {
            DataManager.Clear();
            SceneManager.Clear();
            EventManager.Clear();
            AssetManager.Clear();
            ControllerManager.Clear();
        }
    }
}