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
        [FoldoutGroup("设置管理器"), ShowInInspector, LabelText("控制台输出选项")]
        internal DebugOption debugOption;

        [ShowInInspector, LabelText("场景管理数据"), FoldoutGroup("通用管理器")]
        private Dictionary<int, SceneData> sceneList => SceneManager.sceneDict;

        [ShowInInspector, LabelText("资源管理数据"), FoldoutGroup("通用管理器")]
        private Dictionary<string, IEnumerator> assetList => AssetManager.assetDict;

        [ShowInInspector, LabelText("事件管理数据"), FoldoutGroup("通用管理器")]
        private Dictionary<int, EventHandler> eventDict => EventManager.eventDict;

        [ShowInInspector, LabelText("命令管理数据"), FoldoutGroup("通用管理器")]
        private Dictionary<Type, ICommand> commandDict => CommandManager.commandDict;

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
        private float soundVolume => AudioManager.soundVolume;

        [ShowInInspector, LabelText("游戏音乐大小"), FoldoutGroup("音效管理器")]
        private float audioVolume => AudioManager.audioVolume;

        [ShowInInspector, LabelText("完成音效队列"), FoldoutGroup("音效管理器")]
        private Queue<AudioSource> audioQueue => AudioManager.audioQueue;

        [ShowInInspector, LabelText("播放音效列表"), FoldoutGroup("音效管理器")]
        private List<AudioSource> audioList => AudioManager.audioList;

        [ShowInInspector, LabelText("对象池管理器"), FoldoutGroup("对象池管理器")]
        private Transform poolManager => PoolManager.poolManager;

        [ShowInInspector, LabelText("对象数据管理"), FoldoutGroup("对象池管理器")]
        private Dictionary<string, IPool> poolDict => PoolManager.poolDict;
    }

    internal static class Log
    {
        private static readonly Dictionary<DebugOption, string> debugDict = new Dictionary<DebugOption, string>()
        {
            { DebugOption.Json, "JsonManager " },
            { DebugOption.Pool, "PoolManager " },
            { DebugOption.Data, "DataManager " },
            { DebugOption.Scene, "SceneManager " },
            { DebugOption.Asset, "AssetManager " },
            { DebugOption.Audio, "AudioManager " },
            { DebugOption.Timer, "TimerManager " },
        };


        public static void Info(DebugOption option, string message)
        {
            if (!GlobalManager.Runtime) return;
            if ((GlobalManager.Instance.debugOption & option) == 0) return;
            Debug.Log(debugDict.ContainsKey(option) ? debugDict[option].Sky() + message : message);
        }

        public static void Info(string message)
        {
            Debug.Log(message);
        }

        public static void Warn(string message)
        {
            Debug.LogWarning(message);
        }
    }

    [Flags]
    internal enum DebugOption
    {
        None = 0,
        Json = 1,
        Pool = 2,
        Data = 4,
        Scene = 8,
        Asset = 16,
        Audio = 32,
        Timer = 64,
        Custom = 128,
    }
}