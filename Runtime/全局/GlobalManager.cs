using System;
using System.Collections.Generic;
using JFramework.Interface;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JFramework.Core
{
    public class GlobalManager : Singleton<GlobalManager>
    {
        [ShowInInspector, ReadOnly, LabelText("全局管理器"), FoldoutGroup("全局管理视图")]
        private GlobalController controller;

        [ShowInInspector, ReadOnly, LabelText("实体列表"), FoldoutGroup("全局管理视图")]
        private List<IEntity> entityList;

        private GlobalController Controller
        {
            get
            {
                if (controller != null) return controller;
                if (entityList != null) return controller;
                var obj = new GameObject(Global.GlobalManager);
                controller = obj.AddComponent<GlobalController>();
                obj.hideFlags = HideFlags.HideInHierarchy;
                entityList = new List<IEntity>();
                return controller;
            }
        }

        /// <summary>
        /// 全局管理器初始化
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
            var managerList = new List<IEntity>
            {
                CommandManager.Instance,
                AssetManager.Instance,
                EventManager.Instance,
                TimeManager.Instance,
                PoolManager.Instance,
                AudioManager.Instance,
                SceneManager.Instance,
                JsonManager.Instance,
                DataManager.Instance,
                UIManager.Instance,
            };
            foreach (var manager in managerList)
            {
                manager.OnInit();
            }
        }

        /// <summary>
        /// 在控制器中添加侦听的方法
        /// </summary>
        /// <param name="action">侦听的事件</param>
        public void Listen(Action action)
        {
            Controller.Listen(action);
            entityList.Add((IEntity)action.Target);
        }

        /// <summary>
        /// 在控制器中添加侦听的方法
        /// </summary>
        /// <param name="action">侦听的事件</param>
        public void Remove(Action action)
        {
            entityList.Remove((IEntity)action.Target);
            Controller.Remove(action);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            entityList = null;
        }
    }
}