// *********************************************************************************
// # Project: Forest
// # Unity: 2022.3.5f1c1
// # Author: jinyijie
// # Version: 1.0.0
// # History: 2024-08-25  01:08
// # Copyright: 2024, jinyijie
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using JFramework.Interface;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JFramework.Core
{
    public sealed partial class GlobalManager
    {
        public static AssetMode mode => SettingManager.Instance.assetMode;
#if UNITY_EDITOR
        [ShowInInspector] private static Dictionary<IEntity, Dictionary<Type, IComponent>> entities = new();
        [ShowInInspector] private static Dictionary<string, IPool<GameObject>> pools = new();
        [ShowInInspector] private static Dictionary<Type, IPool> streams = new();
        [ShowInInspector] private static Dictionary<Type, IEvent> events = new();
        [ShowInInspector] private static Dictionary<string, UIPanel> panels = new();
        [ShowInInspector] private static Dictionary<Type, List<UIPanel>> groups = new();
        [ShowInInspector] private static Dictionary<Type, InputManager.InputData> inputs = new();
        [ShowInInspector] private static Dictionary<Type, Dictionary<int, IData>> intData = new();
        [ShowInInspector] private static Dictionary<Type, Dictionary<Enum, IData>> enumData = new();
        [ShowInInspector] private static Dictionary<Type, Dictionary<string, IData>> stringData = new();
        [ShowInInspector] private static Dictionary<int, List<Timer>> timers = new();
        [ShowInInspector] private static Dictionary<int, List<Tween>> motions = new();
        [ShowInInspector] private static List<AudioSource> audios = new();
        [ShowInInspector] private static Vector2 audioManager => new Vector2(AudioManager.mainVolume, AudioManager.audioVolume);
        [ShowInInspector] private static Vector2 inputManager => new Vector2(InputManager.horizontal, InputManager.vertical);

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void RuntimeInitializeOnLoad()
        {
            var field = typeof(PoolManager).GetField("streams", Reflection.Static)?.GetValue(null);
            streams = (Dictionary<Type, IPool>)field;
            field = typeof(PoolManager).GetField("pools", Reflection.Static)?.GetValue(null);
            pools = (Dictionary<string, IPool<GameObject>>)field;
            field = typeof(EntityManager).GetField("entities", Reflection.Static)?.GetValue(null);
            entities = (Dictionary<IEntity, Dictionary<Type, IComponent>>)field;
            field = typeof(EventManager).GetField("events", Reflection.Static)?.GetValue(null);
            events = (Dictionary<Type, IEvent>)field;
            field = typeof(UIManager).GetField("panels", Reflection.Static)?.GetValue(null);
            panels = (Dictionary<string, UIPanel>)field;
            field = typeof(UIManager).GetField("groups", Reflection.Static)?.GetValue(null);
            groups = (Dictionary<Type, List<UIPanel>>)field;
            field = typeof(InputManager).GetField("inputs", Reflection.Static)?.GetValue(null);
            inputs = (Dictionary<Type, InputManager.InputData>)field;
            field = typeof(DataManager).GetField("intData", Reflection.Static)?.GetValue(null);
            intData = (Dictionary<Type, Dictionary<int, IData>>)field;
            field = typeof(DataManager).GetField("enumData", Reflection.Static)?.GetValue(null);
            enumData = (Dictionary<Type, Dictionary<Enum, IData>>)field;
            field = typeof(DataManager).GetField("stringData", Reflection.Static)?.GetValue(null);
            stringData = (Dictionary<Type, Dictionary<string, IData>>)field;
            field = typeof(AudioManager).GetField("audios", Reflection.Static)?.GetValue(null);
            audios = (List<AudioSource>)field;
            field = typeof(TimerManager).GetField("timers", Reflection.Static)?.GetValue(null);
            timers = (Dictionary<int, List<Timer>>)field;
            field = typeof(TweenManager).GetField("motions", Reflection.Static)?.GetValue(null);
            motions = (Dictionary<int, List<Tween>>)field;
        }

        public static void EditorWindow(string path, object editor) => EditorSetting.editors[path] = editor;
#endif
    }
}