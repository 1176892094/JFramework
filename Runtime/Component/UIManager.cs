// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-10-25  00:04
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using JFramework.Interface;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
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
        /// UI画布
        /// </summary>
        public static Canvas canvas { get; private set; }

        /// <summary>
        /// UI管理器初始化
        /// </summary>
        internal static void Register()
        {
            var transform = GlobalManager.Instance.transform;
            canvas = transform.Find("UICanvas").GetComponent<Canvas>();
            layers[UILayer.Normal] = canvas.transform.Find("Layer1");
            layers[UILayer.Bottom] = canvas.transform.Find("Layer2");
            layers[UILayer.Middle] = canvas.transform.Find("Layer3");
            layers[UILayer.Height] = canvas.transform.Find("Layer4");
            layers[UILayer.Ignore] = canvas.transform.Find("Layer5");
        }

        /// <summary>
        /// UI管理器显示UI面板 (无委托值)
        /// </summary>
        /// <typeparam name="TPanel">可以使用所有继承IPanel的对象</typeparam>
        public static void ShowPanel<TPanel>() where TPanel : Component, IPanel
        {
            if (!GlobalManager.Runtime) return;
            if (panels.TryGetValue(typeof(TPanel), out var panel))
            {
                panel.Show();
                return;
            }

            LoadPanel<TPanel>(null);
        }

        /// <summary>
        /// UI管理器显示UI面板 (无委托值)
        /// </summary>
        /// <typeparam name="TPanel">可以使用所有继承IPanel的对象</typeparam>
        public static void ShowPanel<TPanel>(Action action) where TPanel : Component, IPanel
        {
            if (!GlobalManager.Runtime) return;
            if (panels.TryGetValue(typeof(TPanel), out var panel))
            {
                panel.Show();
                action?.Invoke();
                return;
            }

            LoadPanel<TPanel>(panel => action?.Invoke());
        }

        /// <summary>
        /// UI管理器显示UI面板 (有委托值)
        /// </summary>
        /// <typeparam name="TPanel">可以使用所有继承IPanel的对象</typeparam>
        public static void ShowPanel<TPanel>(Action<TPanel> action) where TPanel : Component, IPanel
        {
            if (!GlobalManager.Runtime) return;
            if (panels.TryGetValue(typeof(TPanel), out var panel))
            {
                panel.Show();
                action?.Invoke((TPanel)panel);
                return;
            }

            LoadPanel<TPanel>(action);
        }

        /// <summary>
        /// UI管理器加载面板
        /// </summary>
        /// <typeparam name="TPanel">可以使用所有继承IPanel的对象</typeparam>
        private static void LoadPanel<TPanel>(Action<TPanel> action) where TPanel : Component, IPanel
        {
            if (panels.ContainsKey(typeof(TPanel)))
            {
                Debug.LogWarning($"加载  {typeof(TPanel).Name.Red()} 失败，面板已经加载!");
                return;
            }

            AssetManager.LoadAsync<GameObject>(GlobalSetting.GetUIPath(typeof(TPanel).Name), obj =>
            {
                if (!obj.TryGetComponent<TPanel>(out var panel))
                {
                    obj.AddComponent<TPanel>();
                }

                panel.transform.SetParent(GetLayer(panel.layer));
                panel.transform.localPosition = Vector3.zero;
                panel.transform.localScale = Vector3.one;
                panels.Add(typeof(TPanel), panel);
                panel.Show();
                action?.Invoke((TPanel)panel);
            });
        }

        /// <summary>
        /// UI管理器隐藏UI面板
        /// </summary>
        public static void HidePanel<TPanel>() where TPanel : Component, IPanel
        {
            if (!GlobalManager.Runtime) return;
            if (panels.TryGetValue(typeof(TPanel), out var panel))
            {
                if (IsActive<TPanel>())
                {
                    panel.Hide();
                }
            }
        }

        /// <summary>
        /// UI管理器得到UI面板
        /// </summary>
        /// <typeparam name="TPanel">可以使用所有继承IPanel的对象</typeparam>
        /// <returns>返回获取到的UI面板</returns>
        public static TPanel GetPanel<TPanel>() where TPanel : Component, IPanel => (TPanel)GetPanel(typeof(TPanel));

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
        public static bool IsActive<TPanel>() where TPanel : Component, IPanel
        {
            return panels.TryGetValue(typeof(TPanel), out var panel) ? panel.gameObject.activeInHierarchy : false;
        }

        /// <summary>
        /// UI管理器得到层级
        /// </summary>
        /// <returns>返回得到的层级</returns>
        public static Transform GetLayer(UILayer type) => panels != null ? layers[type] : null;

        /// <summary>
        /// 注册按钮组
        /// </summary>
        /// <param name="buttonGroup"></param>
        /// <param name="action"></param>
        public static void SetButtons(Component buttonGroup, UnityAction action)
        {
            var buttons = buttonGroup.GetComponentsInChildren<Button>();
            foreach (var button in buttons)
            {
                button.onClick.AddListener(action);
            }
        }

        /// <summary>
        /// 注册开关组
        /// </summary>
        /// <param name="buttonGroup"></param>
        /// <param name="action"></param>
        public static void SetToggles(Component buttonGroup, UnityAction<bool> action)
        {
            var toggles = buttonGroup.GetComponentsInChildren<Toggle>();
            foreach (var toggle in toggles)
            {
                toggle.onValueChanged.AddListener(action);
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
                if (panels[type].state != UIState.DontDestroy)
                {
                    Object.Destroy(panels[type].gameObject);
                    panels.Remove(type);
                }
            }
        }

        /// <summary>
        /// UI管理器销毁
        /// </summary>
        internal static void UnRegister()
        {
            canvas = null;
            panels.Clear();
            layers.Clear();
        }
    }
}