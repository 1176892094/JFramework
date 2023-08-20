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
        internal static readonly Dictionary<UILayer, Transform> layers = new Dictionary<UILayer, Transform>();

        /// <summary>
        /// 存储所有UI的字典
        /// </summary>
        internal static readonly Dictionary<Type, IPanel> panels = new Dictionary<Type, IPanel>();

        /// <summary>
        /// 拷贝面板列表
        /// </summary>
        private static readonly List<Type> copies = new List<Type>();

        /// <summary>
        /// UI画布
        /// </summary>
        public static Canvas canvas;

        /// <summary>
        /// UI管理器初始化
        /// </summary>
        internal static void Register()
        {
            canvas = GlobalManager.Instance.transform.Find("UICanvas").GetComponent<Canvas>();
            for (int i = 0; i < 5; i++)
            {
                layers.Add((UILayer)i, canvas.transform.Find($"Layer{i + 1}"));
            }
        }

        /// <summary>
        /// UI管理器显示UI面板
        /// </summary>
        /// <typeparam name="TPanel">可以使用所有继承IPanel的对象</typeparam>
        public static async void ShowPanel<TPanel>() where TPanel : IPanel
        {
            if (!GlobalManager.Runtime) return;
            if (panels.TryGetValue(typeof(TPanel), out var panel))
            {
                panel.Show();
                return;
            }

            panel = await LoadPanel<TPanel>();
            panel.Show();
        }

        /// <summary>
        /// UI管理器显示UI面板 (无委托值)
        /// </summary>
        /// <typeparam name="TPanel">可以使用所有继承IPanel的对象</typeparam>
        public static async void ShowPanel<TPanel>(Action action) where TPanel : IPanel
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
                Debug.LogWarning($"加载  {typeof(TPanel).Name.Red()} 失败，面板已经加载!");
                return default;
            }

            var obj = await AssetManager.LoadAsync<GameObject>("Prefabs/" + typeof(TPanel).Name);
            var panel = obj.GetComponent<TPanel>();

            if (panel == null)
            {
                Debug.LogWarning($"加载 {typeof(TPanel).Name.Red()} 失败，面板挂载没有组件!");
                return default;
            }

            panels.Add(typeof(TPanel), panel);
            panel.transform.SetParent(GetLayer(panel.layer), false);
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
                    return;
                }

                if (panel.state == UIState.Freeze)
                {
                    Debug.Log($"隐藏 {typeof(TPanel).Name.Red()} 失败,面板处于冻结状态!");
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
        /// 手动注册到UI管理器
        /// </summary>
        /// <param name="panel"></param>
        /// <typeparam name="TPanel"></typeparam>
        public static void Register<TPanel>(TPanel panel) where TPanel : IPanel => panels[typeof(TPanel)] = panel;

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
        /// UI管理器得到层级
        /// </summary>
        /// <returns>返回得到的层级</returns>
        public static Transform GetLayer(UILayer type) => panels != null ? layers[type] : null;

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
            copies.AddRange(panels.Keys.Where(type => panels.ContainsKey(type)));
            foreach (var type in copies)
            {
                if (panels[type].state != UIState.DontDestroy)
                {
                    Object.Destroy(panels[type].gameObject);
                    panels.Remove(type);
                }
            }

            copies.Clear();
        }

        /// <summary>
        /// UI管理器侦听UI面板事件
        /// </summary>
        /// <param name="target">传入的UI对象</param>
        /// <param name="type">事件触发类型</param>
        /// <param name="action">事件触发后的回调</param>
        public static void Listen(Component target, EventTriggerType type, Action<PointerEventData> action)
        {
            var trigger = target.GetComponent<EventTrigger>();
            if (trigger == null) trigger = target.gameObject.AddComponent<EventTrigger>();
            var entry = new EventTrigger.Entry { eventID = type };
            entry.callback.AddListener(eventData => action?.Invoke((PointerEventData)eventData));
            trigger.triggers.Add(entry);
        }

        /// <summary>
        /// UI管理器销毁
        /// </summary>
        internal static void UnRegister()
        {
            panels.Clear();
            layers.Clear();
        }
    }
}