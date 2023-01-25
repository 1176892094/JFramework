using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace JFramework.Core
{
    /// <summary>
    /// UI界面管理器
    /// </summary>
    public class UIManager : Singleton<UIManager>
    {
        /// <summary>
        /// 存储UI层级的字典
        /// </summary>
        [ShowInInspector, ReadOnly, LabelText("游戏界面数据"), FoldoutGroup("游戏界面视图")]
        internal Dictionary<string, UIPanelData> dataDict;

        /// <summary>
        /// 存储所有UI的字典
        /// </summary>
        [ShowInInspector, ReadOnly, LabelText("当前游戏面板"), FoldoutGroup("游戏界面视图")]
        internal Dictionary<string, UIPanel> panelDict;

        /// <summary>
        /// UI最底层
        /// </summary>
        private Transform bottom;

        /// <summary>
        /// UI中间层
        /// </summary>
        private Transform middle;

        /// <summary>
        /// UI最高层
        /// </summary>
        private Transform height;

        /// <summary>
        /// UI普通层
        /// </summary>
        private Transform common;

        /// <summary>
        /// 界面管理器初始化数据
        /// </summary>
        protected override void OnInit(params object[] args)
        {
            bottom = transform.Find("Canvas/Bottom");
            middle = transform.Find("Canvas/Middle");
            height = transform.Find("Canvas/Height");
            common = transform.Find("Canvas/Common");
            panelDict = new Dictionary<string, UIPanel>();
            var textAsset = Resources.Load<TextAsset>(Global.UIPanelData);
            dataDict = JsonConvert.DeserializeObject<Dictionary<string, UIPanelData>>(textAsset.text);
        }

        /// <summary>
        /// UI管理器加载面板
        /// </summary>
        /// <param name="name">加载UI面板的名称</param>
        /// <param name="action">显示面板的回调</param>
        /// <typeparam name="T">可以使用所有继承IPanel的对象</typeparam>
        private void LoadPanel<T>(string name, Action<T> action) where T : UIPanel
        {
            var data = dataDict[name];
            AssetManager.Instance.LoadAsync<GameObject>(data.Path, obj =>
            {
                var layer = GetLayer((UILayerType)data.Layer);
                obj.transform.SetParent(layer, false);
                var panel = obj.GetComponent<T>();
                action?.Invoke(panel);
                panel.Show();
                if (panelDict.ContainsKey(name)) HidePanel<T>();
                panelDict.Add(name, panel);
            });
        }

        /// <summary>
        /// UI管理器显示UI面板
        /// </summary>
        /// <param name="action">显示面板的回调</param>
        /// <typeparam name="T">可以使用所有继承IPanel的对象</typeparam>
        public void ShowPanel<T>(Action<T> action = null) where T : UIPanel
        {
            var key = typeof(T).Name;

            if (panelDict.ContainsKey(key))
            {
                action?.Invoke((T)panelDict[key]);
                panelDict[key].Show();
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
            var key = typeof(T).Name;

            if (panelDict.ContainsKey(key))
            {
                panelDict[key].Hide();

                if (type == UIHideType.Remove)
                {
                    Destroy(panelDict[key].gameObject);
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
            var key = typeof(T).Name;
            if (panelDict.ContainsKey(key))
            {
                return (T)panelDict[key];
            }

            return default;
        }


        /// <summary>
        /// UI管理器得到层级
        /// </summary>
        /// <param name="layer">层级的类型</param>
        /// <returns>返回得到的层级</returns>
        public Transform GetLayer(UILayerType layer)
        {
            switch (layer)
            {
                case UILayerType.Bottom:
                    return bottom;
                case UILayerType.Middle:
                    return middle;
                case UILayerType.Height:
                    return height;
                case UILayerType.Common:
                    return common;
                default:
                    return null;
            }
        }

        /// <summary>
        /// UI管理器清除所有面板
        /// </summary>
        public void Clear()
        {
            foreach (var key in panelDict.Keys)
            {
                if (panelDict.ContainsKey(key))
                {
                    Destroy(panelDict[key].gameObject);
                }
            }

            panelDict.Clear();
        }

        /// <summary>
        /// UI管理器侦听UI面板事件
        /// </summary>
        /// <param name="obj">传入的UI对象</param>
        /// <param name="type">事件触发类型</param>
        /// <param name="action">事件触发后的回调</param>
        public void Listen(UIBehaviour obj, EventTriggerType type, UnityAction<BaseEventData> action)
        {
            UnityEngine.EventSystems.EventTrigger trigger = obj.GetComponent<UnityEngine.EventSystems.EventTrigger>();
            if (trigger == null) trigger = obj.gameObject.AddComponent<UnityEngine.EventSystems.EventTrigger>();
            UnityEngine.EventSystems.EventTrigger.Entry entry = new UnityEngine.EventSystems.EventTrigger.Entry { eventID = type };
            entry.callback.AddListener(action);
            trigger.triggers.Add(entry);
        }
    }
}