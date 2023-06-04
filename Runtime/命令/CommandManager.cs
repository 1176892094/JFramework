using System;
using System.Collections.Generic;
using JFramework.Interface;
using UnityEngine;

namespace JFramework.Core
{
    /// <summary>
    /// 命令管理器
    /// </summary>
    public static class CommandManager
    {
        /// <summary>
        /// 命令存储字典
        /// </summary>
        internal static Dictionary<Type, ICommand> commandDict;

        /// <summary>
        /// 命令管理器初始化
        /// </summary>
        internal static void Awake() => commandDict = new Dictionary<Type, ICommand>();

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="args">传入的参数</param>
        /// <typeparam name="T">传入继承ICommand的对象</typeparam>
        public static void Execute<T>(params object[] args) where T : struct, ICommand
        {
            if (!GlobalManager.Runtime) return;
            var key = typeof(T);
            if (!commandDict.ContainsKey(key))
            {
                var command = new T();
                commandDict.Add(key, command);
                command.OnExecute(args);
                return;
            }

            commandDict[key].OnExecute(args);
        }

        /// <summary>
        /// 清除命令管理器
        /// </summary>
        internal static void Destroy() => commandDict = null;
    }
}