using System;
using UnityEngine;

namespace JFramework.Core
{
    /// <summary>
    /// 全局管理器调试日志
    /// </summary>
    internal static class Log
    {
        /// <summary>
        /// 日志输出方法
        /// </summary>
        /// <param name="option"></param>
        /// <param name="message"></param>
        public static void Info(string message, Option option)
        {
            if (GlobalManager.Runtime && (GlobalManager.Instance.option & option) != Option.None)
            {
                Debug.Log($"{option} {message}");
            }
        }
    }

    [Flags]
    internal enum Option
    {
        None = 0,
        JsonManager = 1 << 0,
        PoolManager = 1 << 1,
        DataManager = 1 << 2,
        SceneManager = 1 << 3,
        AssetManager = 1 << 4,
        AudioManager = 1 << 5,
        TimerManager = 1 << 6,
        EventManager = 1 << 7,
    }
}