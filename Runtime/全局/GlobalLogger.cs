using System;
using System.Collections.Generic;
using UnityEngine;

namespace JFramework.Core
{
    /// <summary>
    /// 全局管理器调试日志
    /// </summary>
    internal static class Log
    {
        /// <summary>
        /// 存储选项字典
        /// </summary>
        private static readonly Dictionary<Option, string> options = new Dictionary<Option, string>()
        {
            { Option.Json, nameof(JsonManager) },
            { Option.Pool, nameof(PoolManager) },
            { Option.Data, nameof(DataManager) },
            { Option.Scene, nameof(SceneManager) },
            { Option.Asset, nameof(AssetManager) },
            { Option.Audio, nameof(AudioManager) },
            { Option.Timer, nameof(TimerManager) },
            { Option.Event, nameof(EventManager) },
        };

        /// <summary>
        /// 日志输出方法
        /// </summary>
        /// <param name="option"></param>
        /// <param name="message"></param>
        public static void Info(Option option, string message)
        {
            if (GlobalManager.Runtime && (GlobalManager.Instance.option & option) != Option.None)
            {
                Debug.Log(options.TryGetValue(option, out var value) ? $"{value.Sky()} {message}" : message);
            }
        }
    }

    [Flags]
    internal enum Option
    {
        None = 0,
        Json = 1,
        Pool = 2,
        Data = 4,
        Scene = 8,
        Asset = 16,
        Audio = 32,
        Timer = 64,
        Event = 128,
    }
}