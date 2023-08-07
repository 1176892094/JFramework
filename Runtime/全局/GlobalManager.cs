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
        /// 是否为活跃的
        /// </summary>
        private static bool isActive;

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
        /// 禁用单例访问 GlobalManager
        /// </summary>
        public static bool Runtime
        {
            get
            {
                if (isActive) return isActive;
                Debug.Log($"{nameof(GlobalManager).Red()} 没有初始化！");
                return isActive;
            }
        }

        /// <summary>
        /// 全局管理器初始化
        /// </summary>
        private async void Awake()
        {
            isActive = true;
            DontDestroyOnLoad(gameObject);
            UIManager.Awake();
            PoolManager.Awake();
            TimerManager.Awake();
            AudioManager.Awake();
            await AssetManager.Awake();
            OnStart?.Invoke();
        }
        
        /// <summary>
        /// 全局管理器销毁
        /// </summary>
        private void OnDestroy()
        {
            UIManager.Destroy();
            PoolManager.Destroy();
            TimerManager.Destroy();
            AudioManager.Destroy();
            RuntimeInitializeOnLoad();
            GC.Collect();
        }

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
                isActive = false;
                OnQuit?.Invoke();
            }
            catch
            {
                // OnQuit 有可能会发生异常 但可以忽略
            }
        }

        /// <summary>
        /// 侦听实体的更新事件
        /// </summary>
        /// <param name="entity"></param>
        public static void Listen(IEntity entity)
        {
            if (!Instance) return;
            OnUpdate += entity.Update;
        }

        /// <summary>
        /// 移除实体的更新
        /// </summary>
        /// <param name="entity"></param>
        public static void Remove(IEntity entity)
        {
            if (!Runtime) return;
            OnUpdate -= entity.Update;
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
            isActive = false;
        }
    }
}