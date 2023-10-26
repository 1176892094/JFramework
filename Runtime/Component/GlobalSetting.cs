// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-10-24  23:55
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using JFramework.Editor;
using JFramework.Interface;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JFramework.Core
{
    using Components = Dictionary<Type, ScriptableObject>;

    public sealed partial class GlobalManager
    {
        [HideInInspector] public bool isRemote;

        [PropertyOrder(-1), Button("关闭远端加载"), ShowIf("isRemote"), GUIColor(1f, 0.5f, 0.5f), FoldoutGroup("设置管理器")]
        public void LocalButton() => EditorSetting.AddSceneToBuildSettings(isRemote = !isRemote);

        [PropertyOrder(-1), Button("开启远端加载"), HideIf("isRemote"), GUIColor(1f, 1f, 1f), FoldoutGroup("设置管理器")]
        public void RemoteButton() => EditorSetting.AddSceneToBuildSettings(isRemote = !isRemote);

        [ShowInInspector, LabelText("游戏组件列表"), FoldoutGroup("对象池管理器")]
        private Dictionary<ICharacter, Components> characters => ControllerManager.characters;

        [ShowInInspector, LabelText("游戏对象数据"), FoldoutGroup("对象池管理器")]
        private Dictionary<string, IPool> pools => PoolManager.pools;

        [ShowInInspector, LabelText("游戏对象池组"), FoldoutGroup("对象池管理器")]
        private Dictionary<string, GameObject> parents => PoolManager.parents;

        [ShowInInspector, LabelText("事件管理数据"), FoldoutGroup("基本管理器")]
        private Dictionary<Type, HashSet<IEvent>> observers => EventManager.observers;

        [ShowInInspector, LabelText("玩家输入数据"), FoldoutGroup("基本管理器")]
        private Dictionary<KeyCode, IInput> inputs => InputManager.inputs;

        [ShowInInspector, LabelText("资源依赖数据"), FoldoutGroup("基本管理器")]
        private Dictionary<string, AssetBundle> depends => AssetManager.depends;

        [ShowInInspector, LabelText("资源加载数据"), FoldoutGroup("基本管理器")]
        private Dictionary<string, (string, string)> assets => AssetManager.assets;

        [ShowInInspector, LabelText("用户界面管理"), FoldoutGroup("面板管理器")]
        private Dictionary<Type, IPanel> panels => UIManager.panels;

        [ShowInInspector, LabelText("界面层级配置"), FoldoutGroup("面板管理器")]
        private Dictionary<UILayer, Transform> layers => UIManager.layers;

        [ShowInInspector, LabelText("数据表 (int)"), FoldoutGroup("数据管理器")]
        private Dictionary<Type, Dictionary<int, IData>> intDataTable => Data<int>.dataTable;

        [ShowInInspector, LabelText("数据表 (enum)"), FoldoutGroup("数据管理器")]
        private Dictionary<Type, Dictionary<Enum, IData>> enmDataTable => Data<Enum>.dataTable;

        [ShowInInspector, LabelText("数据表 (string)"), FoldoutGroup("数据管理器")]
        private Dictionary<Type, Dictionary<string, IData>> strDataTable => Data<string>.dataTable;

        [ShowInInspector, LabelText("计时器 (已完成)"), FoldoutGroup("计时器管理器")]
        private Queue<ITimer> queues => TimerManager.queues;

        [ShowInInspector, LabelText("计时器 (运行中)"), FoldoutGroup("计时器管理器")]
        private LinkedList<ITimer> timers => TimerManager.timers;

        [ShowInInspector, LabelText("背景音乐大小"), FoldoutGroup("音效管理器")]
        private float soundVolume => AudioManager.soundVolume;

        [ShowInInspector, LabelText("游戏音效大小"), FoldoutGroup("音效管理器")]
        private float audioVolume => AudioManager.audioVolume;

        [ShowInInspector, LabelText("游戏音效 (已完成)"), FoldoutGroup("音效管理器")]
        private Stack<AudioSource> stacks => AudioManager.stacks;

        [ShowInInspector, LabelText("游戏音效 (播放中)"), FoldoutGroup("音效管理器")]
        private HashSet<AudioSource> audios => AudioManager.audios;
    }
}
#endif