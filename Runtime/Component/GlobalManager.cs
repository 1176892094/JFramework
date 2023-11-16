// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-10-24  23:45
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

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

        /// <summary>
        /// 全剧管理器开始事件
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
            Runtime = false;
            Instance = null;
            OnQuit?.Invoke();
        }

        /// <summary>
        /// 全剧管理器销毁
        /// </summary>
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
            OnQuit = null;
            OnStart = null;
            OnUpdate = null;
            JsonManager.Clear();
            DataManager.Clear();
            SceneManager.Clear();
            EventManager.Clear();
            AssetManager.Clear();
            ControllerManager.Clear();
        }
    }
}