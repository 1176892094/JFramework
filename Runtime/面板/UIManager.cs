using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JFramework.Interface;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

// ReSharper disable All
namespace JFramework.Core
{
    /// <summary>
    /// UI界面管理器
    /// </summary>
    public static class UIManager
    {
        /// <summary>
        /// UI层级字典
        /// </summary>
        internal static readonly Dictionary<Type, Transform> layers = new Dictionary<Type, Transform>();

        /// <summary>
        /// 存储所有UI的字典
        /// </summary>
        internal static readonly Dictionary<Type, IPanel> panels = new Dictionary<Type, IPanel>();

        /// <summary>
        /// 管理器名称
        /// </summary>
        private static string Name => nameof(UIManager);

        /// <summary>
        /// 界面管理器初始化数据
        /// </summary>
        internal static void Awake()
        {
            var transform = GlobalManager.Instance.transform;
            layers.Add(typeof(UINormal), transform.Find("UICanvas/Layer1"));
            layers.Add(typeof(UIBottom), transform.Find("UICanvas/Layer2"));
            layers.Add(typeof(UIMiddle), transform.Find("UICanvas/Layer3"));
            layers.Add(typeof(UIHeight), transform.Find("UICanvas/Layer4"));
            layers.Add(typeof(UIIgnore), transform.Find("UICanvas/Layer5"));
        }

        /// <summary>
        /// UI管理器加载面板
        /// </summary>
        /// <typeparam name="T">可以使用所有继承IPanel的对象</typeparam>
        private static async Task<T> LoadPanel<T>() where T : IPanel
        {
            var key = typeof(T);
            if (panels.ContainsKey(key)) return default;
            var obj = await AssetManager.LoadAsync<GameObject>("UI/" + key.Name);
            if (obj == null) return default;
            var panel = obj.GetComponent<T>();
            SetLayer<T>(panel);
            panels.Add(key, panel);
            panel.Show();
            return panel;
        }
        
        /// <summary>
        /// UI管理器显示UI面板 (有委托值)
        /// </summary>
        /// <typeparam name="T">可以使用所有继承IPanel的对象</typeparam>
        public static async void ShowPanel<T>(Action<T> action = null) where T : IPanel
        {
            if (!GlobalManager.Runtime) return;
            if (panels.TryGetValue(typeof(T), out var panel))
            {
                panel.Show();
                action?.Invoke((T)panel);
            }

            panel = await LoadPanel<T>();
            action?.Invoke((T)panel);
        }

        /// <summary>
        /// UI管理器隐藏UI面板
        /// </summary>
        public static void HidePanel<T>() where T : IPanel
        {
            if (!GlobalManager.Runtime) return;
            var key = typeof(T);
            if (panels.ContainsKey(key))
            {
                if (panels[key].state == UIStateType.Freeze)
                {
                    Debug.Log($"{Name} 隐藏 => {key.Name.Red()} 失败,面板处于冻结状态!");
                    return;
                }

                panels[key].Hide();
                if (panels[key].state == UIStateType.Common)
                {
                    Object.Destroy(GlobalManager.Get(panels[key]));
                    panels.Remove(key);
                }
            }
        }

        /// <summary>
        /// UI管理器得到UI面板
        /// </summary>
        /// <typeparam name="T">可以使用所有继承IPanel的对象</typeparam>
        /// <returns>返回获取到的UI面板</returns>
        public static T GetPanel<T>() where T : UIPanel => (T)GetPanel(typeof(T));

        /// <summary>
        /// UI管理器得到UI面板
        /// </summary>
        /// <returns>返回获取到的UI面板</returns>
        public static IPanel GetPanel(Type key) => panels.TryGetValue(key, out var panel) ? panel : null;

        /// <summary>
        /// 设置UI面板层级
        /// </summary>
        /// <param name="panel"></param>
        /// <typeparam name="T"></typeparam>
        private static void SetLayer<T>(T panel) where T : IPanel
        {
            var layer = typeof(UILayer);
            var types = typeof(T).GetInterfaces();
            types = types.Where(type => type != layer && layer.IsAssignableFrom(type)).ToArray();
            if (types.Length > 0)
            {
                foreach (var type in types)
                {
                    if (layers.TryGetValue(type, out var transform))
                    {
                        GlobalManager.Get(panel).transform.SetParent(transform, false);
                        return;
                    }
                }
            }
            
            GlobalManager.Get(panel).transform.SetParent(layers[typeof(UINormal)], false);
        }

        /// <summary>
        /// UI管理器得到层级
        /// </summary>
        /// <returns>返回得到的层级</returns>
        public static Transform GetLayer<T>() where T : UILayer => panels != null ? layers[typeof(T)] : null;

        /// <summary>
        /// UI管理器侦听UI面板事件
        /// </summary>
        /// <param name="target">传入的UI对象</param>
        /// <param name="type">事件触发类型</param>
        /// <param name="action">事件触发后的回调</param>
        public static void Listen<T>(Component target, EventTriggerType type, UnityAction<T> action) where T : BaseEventData
        {
            var trigger = target.GetComponent<EventTrigger>();
            if (trigger == null) trigger = target.gameObject.AddComponent<EventTrigger>();
            var entry = new EventTrigger.Entry { eventID = type };
            entry.callback.AddListener(eventData => action?.Invoke((T)eventData));
            trigger.triggers.Add(entry);
        }

        /// <summary>
        /// 手动注册到UI管理器
        /// </summary>
        public static void Register<T>(T panel) where T : IPanel
        {
            var key = typeof(T);
            if (!panels.ContainsKey(key))
            {
                UIManager.panels.Add(key, panel);
            }
        }

        /// <summary>
        /// UI管理器清除可销毁的面板
        /// </summary>
        public static void Clear()
        {
            if (!GlobalManager.Runtime) return;
            var copyList = panels.Keys.ToList();
            foreach (var key in copyList.Where(key => panels.ContainsKey(key)))
            {
                if (panels[key].state != UIStateType.Ignore)
                {
                    Object.Destroy(GlobalManager.Get(panels[key]));
                    panels.Remove(key);
                }
            }
        }

        /// <summary>
        /// UI管理器销毁
        /// </summary>
        internal static void Destroy()
        {
            panels.Clear();
            layers.Clear();
        }
    }
}