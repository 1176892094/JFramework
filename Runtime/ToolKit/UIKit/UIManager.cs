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
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// ReSharper disable All

namespace JFramework
{
    public sealed partial class GlobalManager
    {
        /// <summary>
        /// UI界面管理器
        /// </summary>
        public sealed class UIManager : Controller
        {
            /// <summary>
            /// UI层级字典
            /// </summary>
            [ShowInInspector] private readonly Dictionary<UILayer, Transform> layers = new Dictionary<UILayer, Transform>();

            /// <summary>
            /// 存储所有UI的字典
            /// </summary>
            [ShowInInspector] internal readonly Dictionary<Type, IPanel> panels = new Dictionary<Type, IPanel>();

            /// <summary>
            /// UI画布
            /// </summary>
            [ShowInInspector]
            public Canvas canvas { get; private set; }

            /// <summary>
            /// UI管理器初始化
            /// </summary>
            private void Awake()
            {
                canvas = owner.transform.Find("UICanvas").GetComponent<Canvas>();
                layers[UILayer.Bottom] = canvas.transform.Find("Layer1");
                layers[UILayer.Normal] = canvas.transform.Find("Layer2");
                layers[UILayer.Middle] = canvas.transform.Find("Layer3");
                layers[UILayer.Height] = canvas.transform.Find("Layer4");
                layers[UILayer.Ignore] = canvas.transform.Find("Layer5");
            }

            /// <summary>
            /// UI管理器显示UI面板 (无委托值)
            /// </summary>
            /// <typeparam name="TPanel">可以使用所有继承IPanel的对象</typeparam>
            public void ShowPanel<TPanel>() where TPanel : Component, IPanel
            {
                if (!Runtime) return;
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
            public void ShowPanel<TPanel>(Action action) where TPanel : Component, IPanel
            {
                if (!Runtime) return;
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
            public void ShowPanel<TPanel>(Action<TPanel> action) where TPanel : Component, IPanel
            {
                if (!Runtime) return;
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
            private void LoadPanel<TPanel>(Action<TPanel> action) where TPanel : Component, IPanel
            {
                if (panels.ContainsKey(typeof(TPanel)))
                {
                    Debug.LogWarning($"加载  {typeof(TPanel).Name.Red()} 失败，面板已经加载!");
                    return;
                }

                Asset.LoadAsync<GameObject>(GlobalSetting.GetUIPath(typeof(TPanel).Name), obj =>
                {
                    if (!obj.TryGetComponent<TPanel>(out var panel))
                    {
                        panel = obj.AddComponent<TPanel>();
                    }

                    panel.transform.SetParent(GetLayer(panel.layer), false);
                    panels.Add(typeof(TPanel), panel);
                    panel.Show();
                    action?.Invoke((TPanel)panel);
                });
            }

            /// <summary>
            /// UI管理器隐藏UI面板
            /// </summary>
            public void HidePanel<TPanel>() where TPanel : Component, IPanel
            {
                if (!Runtime) return;
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
            public TPanel GetPanel<TPanel>() where TPanel : Component, IPanel => (TPanel)GetPanel(typeof(TPanel));

            /// <summary>
            /// UI管理器得到UI面板
            /// </summary>
            /// <returns>返回获取到的UI面板</returns>
            public IPanel GetPanel(Type key) => panels.TryGetValue(key, out var panel) ? panel : null;

            /// <summary>
            /// UI面板是否活跃
            /// </summary>
            /// <typeparam name="TPanel"></typeparam>
            /// <returns></returns>
            public bool IsActive<TPanel>() where TPanel : Component, IPanel
            {
                return panels.TryGetValue(typeof(TPanel), out var panel) && panel.gameObject.activeInHierarchy;
            }

            /// <summary>
            /// UI管理器得到层级
            /// </summary>
            /// <returns>返回得到的层级</returns>
            public Transform GetLayer(UILayer type) => panels != null ? layers[type] : null;

            /// <summary>
            /// 注册按钮组
            /// </summary>
            /// <param name="group"></param>
            /// <param name="action"></param>
            public void SetButtons(Component group, UnityAction action)
            {
                var buttons = group.GetComponentsInChildren<Button>();
                foreach (var button in buttons)
                {
                    button.onClick.AddListener(action);
                }
            }

            /// <summary>
            /// 注册开关组
            /// </summary>
            /// <param name="group"></param>
            /// <param name="action"></param>
            public void SetToggles(Component group, UnityAction<bool> action)
            {
                var toggles = group.GetComponentsInChildren<Toggle>();
                foreach (var toggle in toggles)
                {
                    toggle.onValueChanged.AddListener(action);
                }
            }

            /// <summary>
            /// UI管理器清除可销毁的面板
            /// </summary>
            public void Clear()
            {
                if (!Runtime) return;
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

            /// <summary>
            /// UI管理器销毁
            /// </summary>
            private void OnDestroy()
            {
                canvas = null;
                panels.Clear();
                layers.Clear();
            }
        }
    }
}