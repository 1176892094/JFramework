using System;
using System.Collections;
using System.Collections.Generic;
using JFramework.Interface;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JFramework.Core
{
    public sealed partial class GlobalManager
    {
        [ShowInInspector, LabelText("显示Json加载信息"), FoldoutGroup("调试管理器")]
        internal bool IsDebugJson;

        [ShowInInspector, LabelText("显示Pool存取信息"), FoldoutGroup("调试管理器")]
        internal bool IsDebugPool;

        [ShowInInspector, LabelText("显示Data加载信息"), FoldoutGroup("调试管理器")]
        internal bool IsDebugData;

        [ShowInInspector, LabelText("显示Asset加载信息"), FoldoutGroup("调试管理器")]
        internal bool IsDebugAsset;

        [ShowInInspector, LabelText("显示Event事件信息"), FoldoutGroup("调试管理器")]
        internal bool IsDebugEvent;

        [ShowInInspector, LabelText("显示Scene加载信息"), FoldoutGroup("调试管理器")]
        internal bool IsDebugScene;
        
        [ShowInInspector, LabelText("当前日期时间"), FoldoutGroup("日期管理器")]
        private string dateTime => DateTime.Now.ToString("yyyy-M-d / hh:mm:ss");

        [ShowInInspector, LabelText("下次刷新时间"), FoldoutGroup("日期管理器")]
        private string refreshTime => DateManager.tomorrow.ToString("yyyy-M-d / hh:mm:ss");

        [ShowInInspector, LabelText("场景管理数据"), FoldoutGroup("通用管理器")]
        private Dictionary<int, SceneData> sceneList => SceneManager.sceneDict;

        [ShowInInspector, LabelText("资源管理数据"), FoldoutGroup("通用管理器")]
        private Dictionary<string, IEnumerator> assetList => AssetManager.assetDict;

        [ShowInInspector, LabelText("事件管理数据"), FoldoutGroup("通用管理器")]
        private Dictionary<int, EventData> eventDict => EventManager.eventDict;

        [ShowInInspector, LabelText("命令管理数据"), FoldoutGroup("通用管理器")]
        private Dictionary<Type, ICommand> commandDict => CommandManager.commandDict;

        [ShowInInspector, LabelText("加密管理数据"), FoldoutGroup("通用管理器")]
        private Dictionary<string, JsonData> jsonDict => JsonManager.jsonDict;

        [ShowInInspector, LabelText("完成计时队列"), FoldoutGroup("计时器管理器")]
        private Queue<Timer> timerQueue => TimerManager.timerQueue;

        [ShowInInspector, LabelText("正在计时队列"), FoldoutGroup("计时器管理器")]
        private List<Timer> timerList => TimerManager.timerList;

        [ShowInInspector, LabelText("整数数据管理"), FoldoutGroup("数据管理器")]
        private Dictionary<Type, Dictionary<int, IData>> intDataDict => DataManager.IntDataDict;

        [ShowInInspector, LabelText("字符数据管理"), FoldoutGroup("数据管理器")]
        private Dictionary<Type, Dictionary<string, IData>> strDataDict => DataManager.StrDataDict;

        [ShowInInspector, LabelText("枚举数据管理"), FoldoutGroup("数据管理器")]
        private Dictionary<Type, Dictionary<Enum, IData>> enmDataDict => DataManager.EnmDataDict;

        [ShowInInspector, LabelText("面板数据管理"), FoldoutGroup("面板管理器")]
        private Dictionary<Type, UIPanel> panelDict => UIManager.panelDict;

        [ShowInInspector, LabelText("面板层级配置"), FoldoutGroup("面板管理器")]
        private Transform[] layerGroup => UIManager.layerGroup;

        [ShowInInspector, LabelText("音乐管理对象"), FoldoutGroup("音效管理器")]
        private GameObject audioManager => AudioManager.poolManager;

        [ShowInInspector, LabelText("当前背景音乐"), FoldoutGroup("音效管理器")]
        private AudioSource audioSource => AudioManager.audioSource;

        [ShowInInspector, LabelText("背景音乐大小"), FoldoutGroup("音效管理器")]
        private float soundVolume => AudioManager.SoundVolume;

        [ShowInInspector, LabelText("游戏音乐大小"), FoldoutGroup("音效管理器")]
        private float audioVolume => AudioManager.AudioVolume;

        [ShowInInspector, LabelText("完成音效队列"), FoldoutGroup("音效管理器")]
        private Queue<AudioSource> audioQueue => AudioManager.audioQueue;

        [ShowInInspector, LabelText("播放音效列表"), FoldoutGroup("音效管理器")]
        private List<AudioSource> audioList => AudioManager.audioList;

        [ShowInInspector, LabelText("对象池管理器"), FoldoutGroup("对象池管理器")]
        private Transform poolManager => PoolManager.poolManager;

        [ShowInInspector, LabelText("对象数据管理"), FoldoutGroup("对象池管理器")]
        private Dictionary<string, IPool> poolDict => PoolManager.poolDict;
    }
}