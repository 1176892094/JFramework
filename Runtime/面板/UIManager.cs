using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JFramework.Interface;
using UnityEngine;
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
        internal static readonly Dictionary<UILayerType, Transform> layers = new Dictionary<UILayerType, Transform>();

        /// <summary>
        /// 存储所有UI的字典
        /// </summary>
        internal static readonly Dictionary<Type, IPanel> panels = new Dictionary<Type, IPanel>();

        /// <summary>
        /// UI画布
        /// </summary>
        public static Canvas canvas;

        /// <summary>
        /// 界面管理器初始化数据
        /// </summary>
        internal static void Awake()
        {
            var transform = GlobalManager.Instance.transform;
            canvas = transform.Find("UICanvas").GetComponent<Canvas>();
            layers.Add(UILayerType.Normal, transform.Find("UICanvas/Layer1"));
            layers.Add(UILayerType.Bottom, transform.Find("UICanvas/Layer2"));
            layers.Add(UILayerType.Middle, transform.Find("UICanvas/Layer3"));
            layers.Add(UILayerType.Height, transform.Find("UICanvas/Layer4"));
            layers.Add(UILayerType.Ignore, transform.Find("UICanvas/Layer5"));
        }

        /// <summary>
        /// UI管理器显示UI面板 (无委托值)
        /// </summary>
        /// <typeparam name="TPanel">可以使用所有继承IPanel的对象</typeparam>
        public static async void ShowPanel<TPanel>(Action action = null) where TPanel : IPanel
        {
            if (!GlobalManager.Runtime) return;
            if (panels.TryGetValue(typeof(TPanel), out var panel))
            {
                panel.Show();
                action?.Invoke();
                return;
            }

            panel = await LoadPanel<TPanel>();
            panel.Show();
            action?.Invoke();
        }

        /// <summary>
        /// UI管理器显示UI面板 (有委托值)
        /// </summary>
        /// <typeparam name="TPanel">可以使用所有继承IPanel的对象</typeparam>
        public static async void ShowPanel<TPanel>(Action<TPanel> action) where TPanel : IPanel
        {
            if (!GlobalManager.Runtime) return;
            if (panels.TryGetValue(typeof(TPanel), out var panel))
            {
                panel.Show();
                action?.Invoke((TPanel)panel);
                return;
            }

            panel = await LoadPanel<TPanel>();
            panel.Show();
            action?.Invoke((TPanel)panel);
        }

        /// <summary>
        /// UI管理器加载面板
        /// </summary>
        /// <typeparam name="TPanel">可以使用所有继承IPanel的对象</typeparam>
        private static async Task<TPanel> LoadPanel<TPanel>() where TPanel : IPanel
        {
            if (panels.ContainsKey(typeof(TPanel)))
            {
                Debug.Log($"{nameof(UIManager)} 加载 => {typeof(TPanel).Name.Red()} 失败,面板已经加载!");
                return default;
            }

            var obj = await AssetManager.LoadAsync<GameObject>("Prefabs/" + typeof(TPanel).Name);
            var panel = obj.GetComponent<TPanel>();

            if (panel == null)
            {
                Debug.Log($"{nameof(UIManager)} 加载 => {typeof(TPanel).Name.Red()} 失败,面板挂载没有组件!");
                return default;
            }

            panels.Add(typeof(TPanel), panel);
            panel.SetLayer(panel.layerType);
            return panel;
        }

        /// <summary>
        /// UI管理器隐藏UI面板
        /// </summary>
        public static void HidePanel<TPanel>() where TPanel : IPanel
        {
            if (!GlobalManager.Runtime) return;
            if (panels.TryGetValue(typeof(TPanel), out var panel))
            {
                if (!IsActive<TPanel>())
                {
                    Debug.Log($"{nameof(UIManager).Sky()} 隐藏 => {typeof(TPanel).Name.Red()} 失败,面板已经隐藏!");
                    return;
                }

                if (panel.stateType == UIStateType.Freeze)
                {
                    Debug.Log($"{nameof(UIManager).Sky()} 隐藏 => {typeof(TPanel).Name.Red()} 失败,面板处于冻结状态!");
                    return;
                }

                panel.Hide();
            }
        }

        /// <summary>
        /// UI管理器得到UI面板
        /// </summary>
        /// <typeparam name="TPanel">可以使用所有继承IPanel的对象</typeparam>
        /// <returns>返回获取到的UI面板</returns>
        public static TPanel GetPanel<TPanel>() where TPanel : IPanel => (TPanel)GetPanel(typeof(TPanel));

        /// <summary>
        /// UI管理器得到UI面板
        /// </summary>
        /// <returns>返回获取到的UI面板</returns>
        public static IPanel GetPanel(Type key) => panels.TryGetValue(key, out var panel) ? panel : null;

        /// <summary>
        /// UI面板是否活跃
        /// </summary>
        /// <typeparam name="TPanel"></typeparam>
        /// <returns></returns>
        public static bool IsActive<TPanel>() where TPanel : IPanel
        {
            return panels.TryGetValue(typeof(TPanel), out var panel) ? panel.gameObject.activeInHierarchy : false;
        }

        /// <summary>
        /// 手动注册到UI管理器
        /// </summary>
        public static void Register<T>(T panel) where T : IPanel
        {
            if (!panels.ContainsKey(typeof(T)))
            {
                UIManager.panels.Add(typeof(T), panel);
            }
        }

        /// <summary>
        /// UI管理器得到层级
        /// </summary>
        /// <returns>返回得到的层级</returns>
        public static Transform GetLayer(UILayerType type) => panels != null ? layers[type] : null;

        /// <summary>
        /// UI管理器侦听UI面板事件
        /// </summary>
        /// <param name="target">传入的UI对象</param>
        /// <param name="type">事件触发类型</param>
        /// <param name="action">事件触发后的回调</param>
        public static void Listen<T>(Component target, EventTriggerType type, Action<T> action) where T : BaseEventData
        {
            var trigger = target.GetComponent<EventTrigger>();
            if (trigger == null) trigger = target.gameObject.AddComponent<EventTrigger>();
            var entry = new EventTrigger.Entry { eventID = type };
            entry.callback.AddListener(eventData => action?.Invoke((T)eventData));
            trigger.triggers.Add(entry);
        }

        /// <summary>
        /// 销毁面板
        /// </summary>
        /// <typeparam name="TPanel"></typeparam>
        public static void Destroy<TPanel>() where TPanel : IPanel
        {
            if (!GlobalManager.Runtime) return;
            if (panels.ContainsKey(typeof(TPanel)))
            {
                panels[typeof(TPanel)].Hide();
                Object.Destroy(panels[typeof(TPanel)].gameObject);
                panels.Remove(typeof(TPanel));
            }
        }

        /// <summary>
        /// UI管理器清除可销毁的面板
        /// </summary>
        public static void Clear()
        {
            if (!GlobalManager.Runtime) return;
            var copies = panels.Keys.Where(type => panels.ContainsKey(type)).ToList();
            foreach (var type in copies)
            {
                if (panels[type].stateType != UIStateType.DontDestroy)
                {
                    Object.Destroy(panels[type].gameObject);
                    panels.Remove(type);
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