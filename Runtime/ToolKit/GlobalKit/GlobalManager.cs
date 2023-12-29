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
using JFramework.Interface;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using System.Collections.Generic;
using JFramework.Editor;
#endif

// ReSharper disable All

namespace JFramework
{
    [AddComponentMenu(""), DefaultExecutionOrder(-10)]
    public sealed partial class GlobalManager : MonoBehaviour, IInject
    {
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
            this.Inject();
            Runtime = true;
        }

        /// <summary>
        /// 全剧管理器开始事件
        /// </summary>
        private void Start()
        {
            try
            {
                OnStart?.Invoke();
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }

        /// <summary>
        /// 广播管理器更新事件
        /// </summary>
        private void Update()
        {
            try
            {
                OnUpdate?.Invoke();
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }

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
        /// 全剧管理器销毁
        /// </summary>
        private void OnDestroy()
        {
            OnQuit = null;
            OnStart = null;
            OnUpdate = null;
            Destroy(Entity);
            GC.Collect();
        }

        /// <summary>
        /// 场景加载前
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void RuntimeInitializeOnLoad()
        {
            Entity = ScriptableObject.CreateInstance<EntityManager>();
        }
    }

    public sealed partial class GlobalManager
    {
#if UNITY_EDITOR
        public static bool isRemote => BuildSetting.Instance.isRemote;
        public static void Add(int id, string name, object item) => EditorSetting.Add(id, name, item);
        [ShowInInspector] public static Dictionary<Type, IPool> streams => StreamPool.streams;
#endif
        [ShowInInspector, LabelText("实体")] internal static EntityManager Entity;

        [ShowInInspector, LabelText("界面"), Inject]
        public static UIManager UI;

        [ShowInInspector, LabelText("持久化"), Inject]
        public static JsonManager Json;

        [ShowInInspector, LabelText("数据表"), Inject]
        public static DataManager Data;

        [ShowInInspector, LabelText("对象池"), Inject]
        public static PoolManager Pool;

        [ShowInInspector, LabelText("音效"), Inject]
        public static AudioManager Audio;

        [ShowInInspector, LabelText("场景"), Inject]
        public static SceneManager Scene;

        [ShowInInspector, LabelText("计时器"), Inject]
        public static TimerManager Time;

        [ShowInInspector, LabelText("事件中心"), Inject]
        public static EventManager Event;

        [ShowInInspector, LabelText("资源管理"), Inject]
        public static AssetManager Asset;

        [ShowInInspector, LabelText("资源请求"), Inject]
        public static AssetRequest Request;
    }
}