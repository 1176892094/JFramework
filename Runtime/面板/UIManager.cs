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
        /// 存储所有UI的字典
        /// </summary>
        internal static readonly Dictionary<Type, UIPanel> panelDict = new Dictionary<Type, UIPanel>();

        /// <summary>
        /// UI层级数组
        /// </summary>
        internal static Transform[] layerGroup = Array.Empty<Transform>();

        /// <summary>
        /// 管理器名称
        /// </summary>
        private static string Name => nameof(UIManager);

        /// <summary>
        /// 界面管理器初始化数据
        /// </summary>
        internal static void Awake()
        {
            layerGroup = new Transform[5];
            var transform = GlobalManager.Instance.transform;
            layerGroup[0] = transform.Find("UICanvas/Layer1");
            layerGroup[1] = transform.Find("UICanvas/Layer2");
            layerGroup[2] = transform.Find("UICanvas/Layer3");
            layerGroup[3] = transform.Find("UICanvas/Layer4");
            layerGroup[4] = transform.Find("UICanvas/Layer5");
        }

        /// <summary>
        /// UI管理器加载面板
        /// </summary>
        /// <param name="name">传入面板的名称</param>
        /// <typeparam name="T">可以使用所有继承IPanel的对象</typeparam>
        private static async Task<T> LoadPanel<T>(string name) where T : UIPanel
        {
            var key = typeof(T);
            if (panelDict.ContainsKey(key)) return null;
            var obj = await AssetManager.LoadAsync<GameObject>("UI/" + name);
            obj.transform.SetParent(layerGroup[0], false);
            var panel = obj.GetComponent<T>();
            panelDict.Add(key, panel);
            panel.Show();
            return panel;
        }

        /// <summary>
        /// UI管理器显示UI面板
        /// </summary>
        /// <typeparam name="T">可以使用所有继承IPanel的对象</typeparam>
        public static async Task<T> ShowPanel<T>() where T : UIPanel
        {
            if (!GlobalManager.Runtime) return null;
            var key = typeof(T);
            if (panelDict.ContainsKey(key))
            {
                panelDict[key].Show();
                return (T)panelDict[key];
            }

            return await LoadPanel<T>(key.Name);
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
        public static UIPanel GetPanel(Type key) => panelDict.ContainsKey(key) ? panelDict?[key] : null;

        /// <summary>
        /// UI管理器得到层级
        /// </summary>
        /// <param name="layer">层级的类型</param>
        /// <returns>返回得到的层级</returns>
        public static Transform GetLayer(UILayerType layer) => panelDict != null ? layerGroup[(int)layer] : null;

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
        public static void Register<T>(UIPanel panel) where T: UIPanel
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
            foreach (var key in panelDict.Keys.ToList().Where(key => panelDict.ContainsKey(key) && panelDict[key].stateType != UIStateType.Ignore))
            {
                Object.Destroy(panelDict[key].gameObject);
                panelDict.Remove(key);
            }
        }

        /// <summary>
        /// UI管理器销毁
        /// </summary>
        internal static void Destroy()
        {
            panelDict.Clear();
            layerGroup = Array.Empty<Transform>();
        }
    }
}