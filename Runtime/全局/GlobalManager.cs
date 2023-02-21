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
    public sealed class GlobalManager : MonoSingleton<GlobalManager>
    {
        [ShowInInspector, LabelText("实体管理数据"), FoldoutGroup("实体管理器"), ReadOnly]
        private Dictionary<int, IEntity> entityDict;

        [ShowInInspector, LabelText("实体索引队列"), FoldoutGroup("实体管理器"), ReadOnly]
        private Queue<int> entityQueue;

        [ShowInInspector, LabelText("场景管理数据"), FoldoutGroup("通用管理器")]
        private List<SceneData> sceneList => LoadManager.Instance.sceneList;

        [ShowInInspector, LabelText("事件管理数据"), FoldoutGroup("通用管理器")]
        private Dictionary<int, EventData> eventDict => EventManager.Instance.eventDict;

        [ShowInInspector, LabelText("命令管理数据"), FoldoutGroup("通用管理器")]
        private Dictionary<string, ICommand> commandDict => CommandManager.Instance.commandDict;

        [ShowInInspector, LabelText("加密管理数据"), FoldoutGroup("通用管理器")]
        private Dictionary<string, JsonData> jsonDict => JsonManager.Instance.jsonDict;

        [ShowInInspector, LabelText("完成计时队列"), FoldoutGroup("计时器管理器")]
        private Queue<Timer> timerQueue => TimerManager.Instance.timerQueue;

        [ShowInInspector, LabelText("正在计时队列"), FoldoutGroup("计时器管理器")]
        private List<Timer> timerList => TimerManager.Instance.timerList;

        [ShowInInspector, LabelText("整数数据管理"), FoldoutGroup("数据管理器")]
        private Dictionary<Type, Dictionary<int, IData>> intDataDict => DataManager.Instance.IntDataDict;

        [ShowInInspector, LabelText("字符数据管理"), FoldoutGroup("数据管理器")]
        private Dictionary<Type, Dictionary<string, IData>> strDataDict => DataManager.Instance.StrDataDict;

        [ShowInInspector, LabelText("面板数据管理"), FoldoutGroup("面板管理器")]
        private Dictionary<string, UIPanel> panelDict => UIManager.Instance.panelDict;

        [ShowInInspector, LabelText("面板数据配置"), FoldoutGroup("面板管理器")]
        private Dictionary<string, UIPanelData> dataDict => UIManager.Instance.dataDict;

        [ShowInInspector, LabelText("面板层级配置"), FoldoutGroup("面板管理器")]
        private Transform[] layerGroup => UIManager.Instance.layerGroup;
        
        [ShowInInspector, LabelText("音乐管理对象"), FoldoutGroup("音效管理器")]
        private GameObject audioManager => AudioManager.Instance.audioManager;

        [ShowInInspector, LabelText("当前背景音乐"), FoldoutGroup("音效管理器")]
        private AudioSource audioSource => AudioManager.Instance.audioSource;

        [ShowInInspector, LabelText("背景音乐大小"), FoldoutGroup("音效管理器")]
        private float soundVolume => AudioManager.Instance.SoundVolume;

        [ShowInInspector, LabelText("游戏音乐大小"), FoldoutGroup("音效管理器")]
        private float audioVolume => AudioManager.Instance.AudioVolume;

        [ShowInInspector, LabelText("完成音效队列"), FoldoutGroup("音效管理器")]
        private Queue<AudioSource> audioQueue => AudioManager.Instance.audioQueue;

        [ShowInInspector, LabelText("播放音效列表"), FoldoutGroup("音效管理器")]
        private List<AudioSource> audioList => AudioManager.Instance.audioList;

        [ShowInInspector, LabelText("对象池管理器"), FoldoutGroup("对象池管理器")]
        private GameObject poolManager => PoolManager.Instance.manager;

        [ShowInInspector, LabelText("对象数据管理"), FoldoutGroup("对象池管理器")]
        private Dictionary<string, IPool> poolDict => PoolManager.Instance.poolDict;

        /// <summary>
        /// 受保护的全局控制器
        /// </summary>
        private GlobalController _controller;

        /// <summary>
        /// 全局控制器
        /// </summary>
        private GlobalController controller
        {
            get
            {
                if (_controller != null) return _controller;
                if (entityDict != null) return _controller;
                var obj = new GameObject(nameof(GlobalManager));
                _controller = obj.AddComponent<GlobalController>();
                obj.hideFlags = HideFlags.HideAndDontSave;
                entityDict = new Dictionary<int, IEntity>();
                entityQueue = new Queue<int>();
                return _controller;
            }
        }

        /// <summary>
        /// 全局管理器醒来
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
            Register();
        }

        /// <summary>
        /// 注册管理器
        /// </summary>
        private void Register()
        {
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
            controller.Listen(TimerManager.Instance.OnUpdate);
        }

        /// <summary>
        /// 释放管理器
        /// </summary>
        private void Dispose()
        {
            controller.Remove(TimerManager.Instance.OnUpdate);
            CommandManager.Instance.Destroy();
            AssetManager.Instance.Destroy();
            EventManager.Instance.Destroy();
            TimerManager.Instance.Destroy();
            JsonManager.Instance.Destroy();
            AudioManager.Instance.Destroy();
            PoolManager.Instance.Destroy();
            LoadManager.Instance.Destroy();
            DataManager.Instance.Destroy();
            UIManager.Instance.Destroy();
        }

        /// <summary>
        /// 添加实体到管理器
        /// </summary>
        /// <param name="entity">传入实体</param>
        public void Listen(IEntity entity)
        {
            controller.Listen(entity.OnUpdate);
            entity.Id = entityQueue.Count > 0 ? entityQueue.Dequeue() : entityDict.Count + 1;
            entityDict.Add(entity.Id, entity);
        }

        /// <summary>
        /// 移除实体到管理器
        /// </summary>
        /// <param name="entity">传入实体</param>
        public void Remove(IEntity entity)
        {
            controller.Remove(entity.OnUpdate);
            entityQueue.Enqueue(entity.Id);
            entityDict.Remove(entity.Id);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id">传入实体的id</param>
        /// <returns>返回对应的实体</returns>
        public T Get<T>(int id) where T : IEntity => entityDict.ContainsKey(id) ? (T)entityDict[id] : default;

        /// <summary>
        /// 全局管理器被销毁
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();
            _controller = null;
            entityDict = null;
            entityQueue = null;
            Dispose();
        }
    }
}