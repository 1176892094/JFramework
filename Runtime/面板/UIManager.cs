using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

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
        /// UI层级数组
        /// </summary>
        internal Transform[] layerGroup;

        /// <summary>
        /// 界面管理器初始化数据
        /// </summary>
        public override void Awake()
        {
            base.Awake();
            var obj = GlobalManager.Instance.gameObject;
            layerGroup = new Transform[5];
            layerGroup[0] = obj.transform.Find("UICanvas/Layer1");
            layerGroup[1] = obj.transform.Find("UICanvas/Layer2");
            layerGroup[2] = obj.transform.Find("UICanvas/Layer3");
            layerGroup[3] = obj.transform.Find("UICanvas/Layer4");
            layerGroup[4] = obj.transform.Find("UICanvas/Layer5");
            panelDict = new Dictionary<string, UIPanel>();
            var textAsset = Resources.Load<TextAsset>("UIPanelData"); //读取UI面版相关的文本数据
            dataDict = JsonConvert.DeserializeObject<Dictionary<string, UIPanelData>>(textAsset.text); //将文本数据转换为string:UIPanelData字典
        }

        /// <summary>
        /// UI管理器加载面板
        /// </summary>
        /// <param name="name">加载UI面板的名称</param>
        /// <param name="action">显示面板的回调</param>
        /// <typeparam name="T">可以使用所有继承IPanel的对象</typeparam>
        private void LoadPanel<T>(string name, Action<T> action) where T : UIPanel
        {
            var data = dataDict[name]; //获取对应名称Panel的数据
            AssetManager.Instance.LoadAsync<GameObject>(data.Path, obj =>
            {
                //通过Panel数据的路径 加载 Panel
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
            if (panelDict == null)
            {
                Debug.Log("面板管理器没有初始化!");
                return;
            }
            
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
            if (panelDict == null)
            {
                Debug.Log("面板管理器没有初始化!");
                return;
            }
            
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
                case UILayerType.Layer1:
                    return layerGroup[0];
                case UILayerType.Layer2:
                    return layerGroup[1];
                case UILayerType.Layer3:
                    return layerGroup[2];
                case UILayerType.Layer4:
                    return layerGroup[3];
                case UILayerType.Layer5:
                    return layerGroup[4];
                default:
                    return null;
            }
        }

        public void HideAll()
        {
            foreach (var key in panelDict.Keys)
            {
                if (panelDict.ContainsKey(key))
                {
                    Object.Destroy(panelDict[key].gameObject);
                }
            }

            panelDict.Clear();
        }

        /// <summary>
        /// UI管理器清除所有面板
        /// </summary>
        public override void Clear()
        {
            dataDict = null;
            panelDict = null;
            layerGroup = null;
        }

        /// <summary>
        /// UI管理器侦听UI面板事件
        /// </summary>
        /// <param name="obj">传入的UI对象</param>
        /// <param name="type">事件触发类型</param>
        /// <param name="action">事件触发后的回调</param>
        public void Listen(MonoBehaviour obj, EventTriggerType type, UnityAction<BaseEventData> action)
        {
            EventTrigger trigger = obj.GetComponent<EventTrigger>();
            if (trigger == null) trigger = obj.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry { eventID = type };
            entry.callback.AddListener(action);
            trigger.triggers.Add(entry);
        }
    }
}