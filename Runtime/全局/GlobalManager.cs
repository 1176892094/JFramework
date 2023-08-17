using System;
using JFramework.Interface;
using UnityEngine;

namespace JFramework.Core
{
    [AddComponentMenu(""), DefaultExecutionOrder(-10)]
    public sealed partial class GlobalManager : MonoBehaviour
    {
        /// <summary>
        /// 安全的单例调用
        /// </summary>
        internal static GlobalManager Instance;

        /// <summary>
        /// 是否在运行模式
        /// </summary>
        public static bool Runtime;

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
        /// 全局管理器初始化
        /// </summary>
        private void Awake()
        {
            Runtime = true;
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Register();
        }

        /// <summary>
        /// 广播开始事件
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
                Runtime = false;
                OnQuit?.Invoke();
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }

        /// <summary>
        /// 全局管理器销毁
        /// </summary>
        private void OnDestroy()
        {
            Clear();
            UnRegister();
            OnQuit = null;
            OnStart = null;
            OnUpdate = null;
            Instance = null;
            GC.Collect();
        }

        /// <summary>
        /// 注册
        /// </summary>
        private static void Register()
        {
            UIManager.Register();
            PoolManager.Register();
            TimerManager.Register();
            AudioManager.Register();
        }

        /// <summary>
        /// 取消注册
        /// </summary>
        private static void UnRegister()
        {
            UIManager.UnRegister();
            PoolManager.UnRegister();
            TimerManager.UnRegister();
            AudioManager.UnRegister();
        }

        /// <summary>
        /// 在场景加载前进行清空
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Clear()
        {
            DataManager.Clear();
            EventManager.Clear();
            SceneManager.Clear();
            AssetManager.Clear();
            ControllerManager.Clear();
        }
    }
}