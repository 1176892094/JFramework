using System;
using System.Collections.Generic;
using JFramework.Core;
using JFramework.Interface;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JFramework
{
    public sealed partial class GlobalManager
    {
        [ShowInInspector, LabelText("场景管理数据"), FoldoutGroup("通用管理器")]
        private List<SceneData> sceneList => LoadManager.Instance.sceneList;

        [ShowInInspector, LabelText("事件管理数据"), FoldoutGroup("通用管理器")]
        private Dictionary<int, EventData> eventDict => EventManager.Instance.eventDict;

        [ShowInInspector, LabelText("命令管理数据"), FoldoutGroup("通用管理器")]
        private Dictionary<string, ICommand> commandDict => CommandManager.Instance.commandDict;

        [ShowInInspector, LabelText("加密管理数据"), FoldoutGroup("通用管理器")]
        private Dictionary<string, JsonData> jsonDict => JsonManager.Instance.jsonDict;

        [ShowInInspector, LabelText("完成计时队列"), FoldoutGroup("计时器管理器")]
        private Queue<Timer> timerQueue => TimerManager.Instance.timerQueue;

        [ShowInInspector, LabelText("正在计时队列"), FoldoutGroup("计时器管理器")]
        private List<Timer> timerList => TimerManager.Instance.timerList;

        [ShowInInspector, LabelText("整数数据管理"), FoldoutGroup("数据管理器")]
        private Dictionary<Type, Dictionary<int, IData>> intDataDict => DataManager.Instance.IntDataDict;

        [ShowInInspector, LabelText("字符数据管理"), FoldoutGroup("数据管理器")]
        private Dictionary<Type, Dictionary<string, IData>> strDataDict => DataManager.Instance.StrDataDict;

        [ShowInInspector, LabelText("面板数据管理"), FoldoutGroup("面板管理器")]
        private Dictionary<string, UIPanel> panelDict => UIManager.Instance.panelDict;

        [ShowInInspector, LabelText("面板层级配置"), FoldoutGroup("面板管理器")]
        private Transform[] layerGroup => UIManager.Instance.layerGroup;

        [ShowInInspector, LabelText("音乐管理对象"), FoldoutGroup("音效管理器")]
        private GameObject audioManager => AudioManager.Instance.audioManager;

        [ShowInInspector, LabelText("当前背景音乐"), FoldoutGroup("音效管理器")]
        private AudioSource audioSource => AudioManager.Instance.audioSource;

        [ShowInInspector, LabelText("背景音乐大小"), FoldoutGroup("音效管理器")]
        private float soundVolume => AudioManager.Instance.SoundVolume;

        [ShowInInspector, LabelText("游戏音乐大小"), FoldoutGroup("音效管理器")]
        private float audioVolume => AudioManager.Instance.AudioVolume;

        [ShowInInspector, LabelText("完成音效队列"), FoldoutGroup("音效管理器")]
        private Queue<AudioSource> audioQueue => AudioManager.Instance.audioQueue;

        [ShowInInspector, LabelText("播放音效列表"), FoldoutGroup("音效管理器")]
        private List<AudioSource> audioList => AudioManager.Instance.audioList;

        [ShowInInspector, LabelText("对象池管理器"), FoldoutGroup("对象池管理器")]
        private GameObject poolManager => PoolManager.Instance.manager;

        [ShowInInspector, LabelText("对象数据管理"), FoldoutGroup("对象池管理器")]
        private Dictionary<string, IPool> poolDict => PoolManager.Instance.poolDict;
    }
}