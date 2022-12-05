using System;
using System.Collections.Generic;
using JFramework.Basic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace JFramework
{
    public class UIManager : SingletonMono<UIManager>
    {
        private readonly Dictionary<string, IPanel> panelDict = new Dictionary<string, IPanel>();
        private Transform bottom;
        private Transform middle;
        private Transform height;
        private Transform common;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            EventManager.AddListener("UIManager", Register);
        }

        private void Register()
        {
            bottom = transform.Find("Canvas/Bottom");
            middle = transform.Find("Canvas/Middle");
            height = transform.Find("Canvas/Height");
            common = transform.Find("Canvas/Common");
        }

        public void ShowPanel<T>(string path, UILayerType layer = UILayerType.Bottom, Action<T> callback = null) where T : IPanel
        {
            if (!panelDict.ContainsKey(path))
            {
                LoadPanel(path, layer, callback);
                return;
            }

            callback?.Invoke((T)panelDict[path]);
            panelDict[path].Show();
        }

        private void LoadPanel<T>(string path, UILayerType layer, Action<T> callback) where T : IPanel
        {
            ResourceManager.LoadAsync<GameObject>(path, obj =>
            {
                Transform parent = GetLayer(layer);
                obj.transform.SetParent(parent, false);
                T panel = obj.GetComponent<T>();
                callback?.Invoke(panel);
                panel.Path = path;
                panel.Show();
                if (panelDict.ContainsKey(path))
                {
                    HidePanel(path);
                }

                panelDict.Add(path, panel);
            });
        }

        public void HidePanel(string path, bool destroy = true)
        {
            if (panelDict.ContainsKey(path))
            {
                if (panelDict[path] != null)
                {
                    panelDict[path].Hide();
                    if (!destroy) return;
                    Destroy(((BasePanel)panelDict[path]).gameObject);
                }

                panelDict.Remove(path);
            }
        }

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

        public T GetPanel<T>(string path) where T : BasePanel
        {
            if (panelDict.ContainsKey(path))
            {
                return (T)panelDict[path];
            }

            return null;
        }


        public void Clear()
        {
            foreach (string path in panelDict.Keys)
            {
                if (panelDict[path] != null)
                {
                    Destroy(((BasePanel)panelDict[path]).gameObject);
                }
            }

            panelDict.Clear();
        }

        public static void AddListener(UIBehaviour control, EventTriggerType type, UnityAction<BaseEventData> callback)
        {
            EventTrigger trigger = control.GetComponent<EventTrigger>();
            if (trigger == null) trigger = control.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry { eventID = type };
            entry.callback.AddListener(callback);
            trigger.triggers.Add(entry);
        }

        private void OnDestroy()
        {
            EventManager.RemoveListener("UIManager", Register);
        }
    }
}

public enum UILayerType
{
    Bottom = 0,
    Middle = 1,
    Height = 2,
    Common = 3,
}