using System;
using System.Collections.Generic;
using JFramework.Core;
using JFramework.Interface;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JFramework
{
    public sealed class GlobalManager : MonoSingleton<GlobalManager>
    {
        [ShowInInspector, LabelText("场景管理数据"), FoldoutGroup("通用管理器")] private List<SceneData> sceneList => LoadManager.Instance.sceneList;
        [ShowInInspector, LabelText("事件管理数据"), FoldoutGroup("通用管理器")] private Dictionary<int, EventData> eventDict => EventManager.Instance.eventDict;
        [ShowInInspector, LabelText("命令管理数据"), FoldoutGroup("通用管理器")] private Dictionary<string, ICommand> commandDict => CommandManager.Instance.commandDict;
        [ShowInInspector, LabelText("加密管理数据"), FoldoutGroup("通用管理器")] private Dictionary<string, JsonData> jsonDict => JsonManager.Instance.jsonDict;
        
        [ShowInInspector, LabelText("实体管理数据"), ReadOnly,FoldoutGroup("实体管理器")] private Dictionary<int, IEntity> entityDict;
        
        [ShowInInspector, LabelText("实体索引队列"), ReadOnly,FoldoutGroup("实体管理器")] private Queue<int> entityQueue;
        [ShowInInspector, LabelText("完成计时队列"), FoldoutGroup("计时器管理器")] private Queue<Timer> timerQueue => TimerManager.Instance.timerQueue;
        [ShowInInspector, LabelText("正在计时队列"), FoldoutGroup("计时器管理器")] private List<Timer> timerList => TimerManager.Instance.timerList;
        [ShowInInspector, LabelText("整数数据管理"), FoldoutGroup("数据管理器")] private Dictionary<Type, Dictionary<int, Data>> intDataDict => DataManager.Instance.IntDataDict;
        [ShowInInspector, LabelText("字符数据管理"), FoldoutGroup("数据管理器")] private Dictionary<Type, Dictionary<string, Data>> strDataDict => DataManager.Instance.StrDataDict;
        [ShowInInspector, LabelText("面板数据管理"), FoldoutGroup("面板管理器")] private Dictionary<string, UIPanel> panelDict => UIManager.Instance.panelDict;
        [ShowInInspector, LabelText("面板数据配置"), FoldoutGroup("面板管理器")] private Dictionary<string, UIPanelData> dataDict => UIManager.Instance.dataDict;
        [ShowInInspector, LabelText("面板层级配置"), FoldoutGroup("面板管理器")] private Transform[] layerGroup => UIManager.Instance.layerGroup;

        [ShowInInspector, LabelText("当前背景音乐"), ReadOnly,FoldoutGroup("音效管理器")] internal GameObject audioManager;
        [ShowInInspector, LabelText("当前背景音乐"), FoldoutGroup("音效管理器")] private AudioSource audioSource => AudioManager.Instance.audioSource;
        [ShowInInspector, LabelText("背景音乐大小"), FoldoutGroup("音效管理器")] private float soundVolume => AudioManager.Instance.soundVolume;
        [ShowInInspector, LabelText("游戏音乐大小"), FoldoutGroup("音效管理器")] private float audioVolume => AudioManager.Instance.audioVolume;
        [ShowInInspector, LabelText("完成音效队列"), FoldoutGroup("音效管理器")] private Queue<AudioSource> audioQueue => AudioManager.Instance.audioQueue;
        [ShowInInspector, LabelText("播放音效列表"), FoldoutGroup("音效管理器")] private List<AudioSource> audioList => AudioManager.Instance.audioList;
        [ShowInInspector, LabelText("对象池管理器"), FoldoutGroup("对象池管理器")] private GameObject poolManager => PoolManager.Instance.manager;
        [ShowInInspector, LabelText("对象数据管理"), FoldoutGroup("对象池管理器")] private Dictionary<string, IPool> poolDict => PoolManager.Instance.poolDict;

        private GlobalController controller;

        private GlobalController Controller
        {
            get
            {
                if (controller != null) return controller;
                if (entityDict != null) return controller;
                var obj = new GameObject(nameof(GlobalManager));
                controller = obj.AddComponent<GlobalController>();
      
                entityDict = new Dictionary<int, IEntity>();
                entityQueue = new Queue<int>();
                return controller;
            }
        }
        private event Action UpdateAction;

        private int entityId;

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
            audioManager = transform.Find("AudioSystem").gameObject;
            CommandManager.Instance.Awake();
            AssetManager.Instance.Awake();
            EventManager.Instance.Awake();
            TimerManager.Instance.Awake();
            AudioManager.Instance.Awake();
            PoolManager.Instance.Awake();
            LoadManager.Instance.Awake();
            JsonManager.Instance.Awake();
            DataManager.Instance.Awake();
            UIManager.Instance.Awake();
            UpdateAction += TimerManager.Instance.OnUpdate;
        }

        private void Update() => UpdateAction?.Invoke();

        /// <summary>
        /// 添加实体到管理器
        /// </summary>
        /// <param name="entity">传入实体</param>
        internal void Listen(IEntity entity)
        {
            Controller.Listen(entity.OnUpdate);
            entity.Id = entityQueue.Count > 0 ? entityQueue.Dequeue() : entityId++;
            entityDict.Add(entity.Id, entity);
        }

        /// <summary>
        /// 移除实体到管理器
        /// </summary>
        /// <param name="entity">传入实体</param>
        internal void Remove(IEntity entity)
        {
            Controller.Remove(entity.OnUpdate);
            entityQueue.Enqueue(entity.Id);
            entityDict.Remove(entity.Id);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id">传入实体的id</param>
        /// <returns>返回对应的实体</returns>
        public IEntity Get(int id)
        {
            return entityDict.ContainsKey(id) ? entityDict[id] : default;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            entityQueue = null;
            entityDict = null;
            CommandManager.Instance.Clear();
            AssetManager.Instance.Clear();
            EventManager.Instance.Clear();
            TimerManager.Instance.Clear();
            AudioManager.Instance.Clear();
            PoolManager.Instance.Clear();
            LoadManager.Instance.Clear();
            JsonManager.Instance.Clear();
            DataManager.Instance.Clear();
            UIManager.Instance.Clear();
            UpdateAction -= TimerManager.Instance.OnUpdate;
        }
    }
}