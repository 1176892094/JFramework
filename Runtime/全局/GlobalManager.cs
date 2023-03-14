using System;
using System.Collections.Generic;
using JFramework.Core;
using JFramework.Interface;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JFramework
{
    /// <summary>
    /// 全局控制器
    /// </summary>
    public sealed partial class GlobalManager : MonoSingleton<GlobalManager>
    {
        [ShowInInspector, LabelText("实体管理数据"), FoldoutGroup("实体管理器"), ReadOnly]
        private static Dictionary<int, IEntity> entityDict;

        [ShowInInspector, LabelText("实体索引队列"), FoldoutGroup("实体管理器"), ReadOnly]
        private static Queue<int> entityQueue;

        private static string Name => nameof(GlobalManager);

        /// <summary>
        /// Update更新事件
        /// </summary>
        internal event Action UpdateAction;

        /// <summary>
        /// 全局管理器醒来
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// 全局Update更新
        /// </summary>
        private void Update() => UpdateAction?.Invoke();

        /// <summary>
        /// 添加实体到管理器
        /// </summary>
        /// <param name="entity">传入实体</param>
        public void Listen(IEntity entity)
        {
            UpdateAction += entity.OnUpdate;
            entity.Id = entityQueue.Count > 0 ? entityQueue.Dequeue() : entityDict.Count + 1;
            entityDict?.Add(entity.Id, entity);
        }

        /// <summary>
        /// 移除实体到管理器
        /// </summary>
        /// <param name="entity">传入实体</param>
        public void Remove(IEntity entity)
        {
            UpdateAction -= entity.OnUpdate;
            entityQueue?.Enqueue(entity.Id);
            entityDict?.Remove(entity.Id);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id">传入实体的id</param>
        /// <returns>返回对应的实体</returns>
        public static T Get<T>(int id) where T : IEntity
        {
            return entityDict.ContainsKey(id) ? (T)entityDict[id] : default;
        }

        private static void AutoCreate()
        {
            if (FindObjectOfType(typeof(GlobalManager))) return;
            Instantiate(Resources.Load<GameObject>(Name));
        }

        /// <summary>
        /// 注册管理器
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Register()
        {
            AutoCreate();
            entityDict = new Dictionary<int, IEntity>();
            entityQueue = new Queue<int>();
            CommandManager.Instance.Awake();
            AssetManager.Instance.Awake();
            EventManager.Instance.Awake();
            TimerManager.Instance.Awake();
            JsonManager.Instance.Awake();
            AudioManager.Instance.Awake();
            PoolManager.Instance.Awake();
            LoadManager.Instance.Awake();
            DataManager.Instance.Awake();
            UIManager.Instance.Awake();
        }

        /// <summary>
        /// 当程序退出
        /// </summary>
        private void OnApplicationQuit()
        {
            UIManager.Instance.Destroy();
            PoolManager.Instance.Destroy();
            LoadManager.Instance.Destroy();
            DataManager.Instance.Destroy();
            JsonManager.Instance.Destroy();
            TimerManager.Instance.Destroy();
            AudioManager.Instance.Destroy();
            AssetManager.Instance.Destroy();
            EventManager.Instance.Destroy();
            CommandManager.Instance.Destroy();
            entityDict = null;
            entityQueue = null;
            UpdateAction = null;
        }
    }
}