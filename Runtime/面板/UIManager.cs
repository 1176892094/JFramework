using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace JFramework.Core
{
    /// <summary>
    /// UI界面管理器
    /// </summary>
    public sealed class UIManager : Singleton<UIManager>
    {
        /// <summary>
        /// 存储所有UI的字典
        /// </summary>
        internal Dictionary<string, UIPanel> panelDict;

        /// <summary>
        /// UI层级数组
        /// </summary>
        internal Transform[] layerGroup;

        /// <summary>
        /// 界面管理器初始化数据
        /// </summary>
        internal override void Awake()
        {
            base.Awake();
            layerGroup = new Transform[5];
            panelDict = new Dictionary<string, UIPanel>();
            if (GlobalManager.Instance == null) return;
            var obj = GlobalManager.Instance.gameObject;
            layerGroup[0] = obj.transform.Find("UICanvas/Layer1");
            layerGroup[1] = obj.transform.Find("UICanvas/Layer2");
            layerGroup[2] = obj.transform.Find("UICanvas/Layer3");
            layerGroup[3] = obj.transform.Find("UICanvas/Layer4");
            layerGroup[4] = obj.transform.Find("UICanvas/Layer5");
        }

        /// <summary>
        /// UI管理器加载面板
        /// </summary>
        /// <param name="name">加载UI面板的名称</param>
        /// <param name="action">显示面板的回调</param>
        /// <typeparam name="T">可以使用所有继承IPanel的对象</typeparam>
        private void LoadPanel<T>(string name, Action<T> action) where T : UIPanel
        {
            AssetManager.Instance.LoadAsync<GameObject>("UI/" + name, obj =>
            {
                if (panelDict.ContainsKey(name)) HidePanel<T>();
                obj.transform.SetParent(layerGroup[0], false);
                var panel = obj.GetComponent<T>();
                panelDict.Add(name, panel);
                panel.Show();
                action?.Invoke(panel);
            });
        }

        /// <summary>
        /// UI管理器显示UI面板
        /// </summary>
        /// <param name="action">显示面板的回调</param>
        /// <typeparam name="T">可以使用所有继承IPanel的对象</typeparam>
        public void ShowPanel<T>(Action<T> action = null) where T : UIPanel
        {
            if (panelDict == null) return;
            var key = typeof(T).Name;

            if (panelDict.ContainsKey(key))
            {
                panelDict[key].Show();
                action?.Invoke((T)panelDict[key]);
                return;
            }

            LoadPanel(key, action);
        }

        /// <summary>
        /// UI管理器隐藏UI面板
        /// </summary>
        /// <param name="type">隐藏UI的方式</param>
        /// <typeparam name="T">可以使用所有继承IPanel的对象</typeparam>
        public void HidePanel<T>(UIHideType type = UIHideType.Remove) where T : UIPanel
        {
            if (panelDict == null) return;
            var key = typeof(T).Name;

            if (panelDict.ContainsKey(key))
            {
                panelDict[key].Hide();

                if (type == UIHideType.Remove)
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
        public T GetPanel<T>() where T : UIPanel
        {
            if (panelDict == null) return null;
            var key = typeof(T).Name;

            if (panelDict.ContainsKey(key))
            {
                return (T)panelDict?[key];
            }

            return null;
        }

        /// <summary>
        /// UI管理器得到层级
        /// </summary>
        /// <param name="layer">层级的类型</param>
        /// <returns>返回得到的层级</returns>
        public Transform GetLayer(UILayerType layer) => panelDict == null ? null : layerGroup[(int)layer];

        /// <summary>
        /// UI管理器侦听UI面板事件
        /// </summary>
        /// <param name="target">传入的UI对象</param>
        /// <param name="type">事件触发类型</param>
        /// <param name="action">事件触发后的回调</param>
        public void Listen(MonoBehaviour target, EventTriggerType type, UnityAction<BaseEventData> action)
        {
            var trigger = target.GetComponent<EventTrigger>();
            if (trigger == null) trigger = target.gameObject.AddComponent<EventTrigger>();
            var entry = new EventTrigger.Entry { eventID = type };
            entry.callback.AddListener(action);
            trigger.triggers.Add(entry);
        }

        /// <summary>
        /// UI管理器清除所有面板
        /// </summary>
        public void Clear()
        {
            foreach (var key in panelDict.Keys.Where(key => panelDict.ContainsKey(key)))
            {
                Object.Destroy(panelDict[key].gameObject);
            }

            panelDict.Clear();
        }

        /// <summary>
        /// UI管理器销毁
        /// </summary>
        internal override void Destroy()
        {
            panelDict = null;
            layerGroup = null;
        }
    }
}