using System;
using System.Collections.Generic;
using JFramework.Interface;

namespace JFramework.Core
{
    public static class CommandManager
    {
        internal static Dictionary<Type, ICommand> commandDict;
        internal static void Awake() => commandDict = new Dictionary<Type, ICommand>();
        internal static void Destroy() => commandDict = null;

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
    }
}