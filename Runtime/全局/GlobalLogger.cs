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
                Debug.Log($"{option}Manager {message}");
            }
        }
    }

    [Flags]
    internal enum Option
    {
        None = 0,
        Json = 1 << 0,
        Pool = 1 << 1,
        Data = 1 << 2,
        Scene = 1 << 3,
        Asset = 1 << 4,
        Audio = 1 << 5,
        Timer = 1 << 6,
        Event = 1 << 7,
    }
}