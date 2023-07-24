using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        internal static readonly Dictionary<Type, Transform> layerDict = new Dictionary<Type, Transform>();
        
        /// <summary>
        /// 存储所有UI的字典
        /// </summary>
        internal static readonly Dictionary<Type, UIPanel> panelDict = new Dictionary<Type, UIPanel>();

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
            layerDict.Add(typeof(UILayer1), transform.Find("UICanvas/Layer1"));
            layerDict.Add(typeof(UILayer2), transform.Find("UICanvas/Layer2"));
            layerDict.Add(typeof(UILayer3), transform.Find("UICanvas/Layer3"));
            layerDict.Add(typeof(UILayer4), transform.Find("UICanvas/Layer4"));
            layerDict.Add(typeof(UILayer5), transform.Find("UICanvas/Layer5"));
        }

        /// <summary>
        /// UI管理器加载面板
        /// </summary>
        /// <typeparam name="T">可以使用所有继承IPanel的对象</typeparam>
        private static async Task<T> LoadPanel<T>() where T : UIPanel
        {
            var key = typeof(T);
            if (panelDict.ContainsKey(key)) return null;
            var obj = await AssetManager.LoadAsync<GameObject>("UI/" + key.Name);
            var panel = obj.GetComponent<T>();
            SetLayer<T>(obj,typeof(UILayer));
            panelDict.Add(key, panel);
            panel.Show();
            return panel;
        }
        
        /// <summary>
        /// UI管理器显示UI面板 (有返回值)
        /// </summary>
        /// <typeparam name="T">可以使用所有继承IPanel的对象</typeparam>
        public static async Task<T> ShowPanelTask<T>() where T : UIPanel
        {
            if (!GlobalManager.Runtime) return null;
            if (panelDict.TryGetValue(typeof(T), out var panel))
            {
                panel.Show();
                return (T)panel;
            }

            return await LoadPanel<T>();
        }
        
        /// <summary>
        /// UI管理器显示UI面板 (无返回值)
        /// </summary>
        /// <typeparam name="T">可以使用所有继承IPanel的对象</typeparam>
        public static async void ShowPanel<T>() where T : UIPanel
        {
            if (!GlobalManager.Runtime) return;
            if (panelDict.TryGetValue(typeof(T), out var panel))
            {
                panel.Show();
                return;
            }

            await LoadPanel<T>();
        }

        /// <summary>
        /// UI管理器隐藏UI面板
        /// </summary>
        public static void HidePanel<T>() where T : UIPanel
        {
            if (!GlobalManager.Runtime) return;
            var key = typeof(T);
            if (panelDict.ContainsKey(key))
            {
                if (panelDict[key].stateType == UIStateType.Freeze)
                {
                    Debug.Log($"{Name} 隐藏 => {key.Name.Red()} 失败,面板处于冻结状态!");
                    return;
                }

                panelDict[key].Hide();
                if (panelDict[key].stateType == UIStateType.Common)
                {
                    Object.Destroy(panelDict[key].gameObject);
                    panelDict.Remove(key);
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
        public static UIPanel GetPanel(Type key) => panelDict.TryGetValue(key, out var panel) ? panel : null;

        /// <summary>
        /// 设置UI面板层级
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="layer"></param>
        /// <typeparam name="T"></typeparam>
        private static void SetLayer<T>(GameObject obj, Type layer) where T : UIPanel
        {
            var types = typeof(T).GetInterfaces().Where(t => t != layer && layer.IsAssignableFrom(t)).ToArray();
            if (types.Length > 0)
            {
                foreach (var type in types)
                {
                    if (layerDict.TryGetValue(type, out var transform))
                    {
                        obj.transform.SetParent(transform, false);
                        return;
                    }
                }
            }

            obj.transform.SetParent(layerDict[typeof(UILayer1)], false);
        }

        /// <summary>
        /// UI管理器得到层级
        /// </summary>
        /// <returns>返回得到的层级</returns>
        public static Transform GetLayer<T>() where T : UILayer => panelDict != null ? layerDict[typeof(T)] : null;

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
        public static void Register<T>(UIPanel panel) where T : UIPanel
        {
            var key = typeof(T);
            if (!panelDict.ContainsKey(key))
            {
                UIManager.panelDict.Add(key, panel);
            }
        }

        /// <summary>
        /// UI管理器清除可销毁的面板
        /// </summary>
        public static void Clear()
        {
            if (!GlobalManager.Runtime) return;
            foreach (var key in panelDict.Keys.ToList().Where(key => panelDict.ContainsKey(key)))
            {
                if (panelDict[key].stateType != UIStateType.Ignore)
                {
                    Object.Destroy(panelDict[key].gameObject);
                    panelDict.Remove(key);
                }
            }
        }

        /// <summary>
        /// UI管理器销毁
        /// </summary>
        internal static void Destroy()
        {
            panelDict.Clear();
            layerDict.Clear();
        }
    }
}