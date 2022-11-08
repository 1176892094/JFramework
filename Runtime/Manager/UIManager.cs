using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace JFramework
{
    public class UIManager : SingletonMono<UIManager>
    {
        private readonly Dictionary<string, BasePanel> panelDict = new Dictionary<string, BasePanel>();
        private Transform bottom;
        private Transform middle;
        private Transform height;
        private Transform common;

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
            bottom = transform.Find("Canvas/Bottom");
            middle = transform.Find("Canvas/Middle");
            height = transform.Find("Canvas/Height");
            common = transform.Find("Canvas/Common");
        }

        public void ShowPanel<T>(string path, UILayerType layer = UILayerType.Bottom, UnityAction<T> callback = null) where T : BasePanel
        {
            if (!panelDict.ContainsKey(path))
            {
                LoadPanel(path, layer, callback);
                return;
            }

            callback?.Invoke((T)panelDict[path]);
            panelDict[path].Show();
        }

        private void LoadPanel<T>(string path, UILayerType layer, UnityAction<T> callback) where T : BasePanel
        {
            ResourceManager.LoadAsync<GameObject>(path, obj =>
            {
                Transform parent = GetLayer(layer);
                obj.transform.SetParent(parent, false);
                T panel = obj.GetComponent<T>();
                callback?.Invoke(panel);
                panel.path = path;
                panel.Show();
                if (panelDict.ContainsKey(path))
                {
                    HidePanel(path);
                }

                panelDict.Add(path, panel);
            });
        }

        public void HidePanel(string path)
        {
            if (panelDict.ContainsKey(path))
            {
                if (panelDict[path] != null)
                {
                    panelDict[path].Hide();
                    Destroy(panelDict[path].gameObject);
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
                    Destroy(panelDict[path].gameObject);
                }
            }
            
            panelDict.Clear();
        }

        public static void AddEventListener(UIBehaviour control, EventTriggerType type, UnityAction<BaseEventData> callback)
        {
            EventTrigger trigger = control.GetComponent<EventTrigger>();
            if (trigger == null) trigger = control.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry { eventID = type };
            entry.callback.AddListener(callback);
            trigger.triggers.Add(entry);
        }
    }
}

public enum UILayerType
{
    Bottom = 0,
    Middle = 1,
    Height = 2,
    Common =3,
}