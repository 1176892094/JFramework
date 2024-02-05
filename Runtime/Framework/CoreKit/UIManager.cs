// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-02-04  18:20
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JFramework.Interface;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

// ReSharper disable All

namespace JFramework.Core
{
    public sealed class UIManager : Component<GlobalManager>
    {
        public Canvas canvas { get; private set; }
        [ShowInInspector] private readonly Dictionary<Type, IPanel> panels = new Dictionary<Type, IPanel>();
        [ShowInInspector] private readonly Dictionary<UILayer, Transform> layers = new Dictionary<UILayer, Transform>();

        private void Awake()
        {
            canvas = owner.transform.Find("UICanvas").GetComponent<Canvas>();
            layers[UILayer.Bottom] = canvas.transform.Find("Layer1");
            layers[UILayer.Normal] = canvas.transform.Find("Layer2");
            layers[UILayer.Middle] = canvas.transform.Find("Layer3");
            layers[UILayer.Height] = canvas.transform.Find("Layer4");
            layers[UILayer.Ignore] = canvas.transform.Find("Layer5");
        }

        public async void ShowPanel<TPanel>() where TPanel : UIPanel
        {
            if (!GlobalManager.Instance) return;
            if (panels.TryGetValue(typeof(TPanel), out var panel))
            {
                panel.Show();
                return;
            }

            await LoadPanel<TPanel>();
        }

        public async void ShowPanel<TPanel>(Action action) where TPanel : UIPanel
        {
            if (!GlobalManager.Instance) return;
            if (panels.TryGetValue(typeof(TPanel), out var panel))
            {
                panel.Show();
                action?.Invoke();
                return;
            }

            await LoadPanel<TPanel>();
            action?.Invoke();
        }

        public async void ShowPanel<TPanel>(Action<TPanel> action) where TPanel : UIPanel
        {
            if (!GlobalManager.Instance) return;
            if (panels.TryGetValue(typeof(TPanel), out var panel))
            {
                panel.Show();
                action?.Invoke((TPanel)panel);
                return;
            }

            panel = await LoadPanel<TPanel>();
            action?.Invoke((TPanel)panel);
        }

        private async Task<TPanel> LoadPanel<TPanel>() where TPanel : UIPanel
        {
            if (panels.ContainsKey(typeof(TPanel)))
            {
                Debug.LogWarning($"加载  {typeof(TPanel).Name.Red()} 失败，面板已经加载!");
                return default;
            }

            var obj = await GlobalManager.Asset.Load<GameObject>(SettingManager.GetUIPath(typeof(TPanel).Name));
            if (!obj.TryGetComponent<TPanel>(out var panel))
            {
                panel = obj.AddComponent<TPanel>();
            }

            panel.transform.SetParent(GetLayer(panel.layer), false);
            panels.Add(typeof(TPanel), panel);
            panel.Show();
            return panel;
        }

        public void HidePanel<TPanel>() where TPanel : IPanel
        {
            if (!GlobalManager.Instance) return;
            if (panels.TryGetValue(typeof(TPanel), out var panel))
            {
                if (IsActive<TPanel>())
                {
                    panel.Hide();
                }
            }
        }

        public TPanel GetPanel<TPanel>() where TPanel : IPanel => (TPanel)GetPanel(typeof(TPanel));

        public IPanel GetPanel(Type key) => panels.TryGetValue(key, out var panel) ? panel : null;

        public Transform GetLayer(UILayer type) => panels != null ? layers[type] : null;

        public bool IsActive<TPanel>() where TPanel : IPanel
        {
            return panels.TryGetValue(typeof(TPanel), out var panel) && panel.gameObject.activeInHierarchy;
        }

        public static void Register<T>(T panel) where T : IPanel
        {
            GlobalManager.UI.panels.Add(typeof(T), panel);
        }

        public static void UnRegister<T>(T panel) where T : IPanel
        {
            Object.Destroy(panel.gameObject);
            GlobalManager.UI.panels.Remove(typeof(T));
        }

        public void Clear()
        {
            if (!GlobalManager.Instance) return;
            var copies = panels.Keys.Where(type => panels.ContainsKey(type)).ToList();
            foreach (var type in copies)
            {
                if (panels[type].state != UIState.DontDestroy)
                {
                    Destroy(panels[type].gameObject);
                    panels.Remove(type);
                }
            }
        }

        private void OnDestroy()
        {
            canvas = null;
            panels.Clear();
            layers.Clear();
        }
    }
}