using System;
using JFramework.Interface;
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
        /// 全局管理器单例
        /// </summary>
        public static GlobalManager Instance;

        /// <summary>
        /// Update更新事件
        /// </summary>
        internal event Action UpdateEvent;

        /// <summary>
        /// 全局管理器醒来
        /// </summary>
        private void Awake() => DontDestroyOnLoad(gameObject);

        /// <summary>
        /// 全局Update更新
        /// </summary>
        private void Update() => UpdateEvent?.Invoke();

        /// <summary>
        /// 添加实体到管理器
        /// </summary>
        /// <param name="entity">传入实体</param>
        public void Listen(IEntity entity) => UpdateEvent += entity.Update;

        /// <summary>
        /// 移除实体到管理器
        /// </summary>
        /// <param name="entity">传入实体</param>
        public void Remove(IEntity entity) => UpdateEvent -= entity.Update;

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
            DailyManager.Awake();
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
            UIManager.Destroy();
            PoolManager.Destroy();
            DataManager.Destroy();
            DailyManager.Destroy();
            JsonManager.Destroy();
            SceneManager.Destroy();
            TimerManager.Destroy();
            AudioManager.Destroy();
            AssetManager.Destroy();
            EventManager.Destroy();
            CommandManager.Destroy();
            Instance = null;
            UpdateEvent = null;
        }
    }
}