using System.Collections.Generic;
using JFramework.Interface;
using UnityEngine;

namespace JFramework
{
    /// <summary>
    /// 命令管理器
    /// </summary>
    public sealed class CommandManager : Singleton<CommandManager>
    {
        /// <summary>
        /// 命令存储字典
        /// </summary>
        internal Dictionary<string, ICommand> commandDict;

        /// <summary>
        /// 命令管理器初始化
        /// </summary>
        internal override void Awake()
        {
            base.Awake();
            commandDict = new Dictionary<string, ICommand>();
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="args">传入的参数</param>
        /// <typeparam name="T">传入继承ICommand的对象</typeparam>
        public void Execute<T>(params object[] args) where T : ICommand, new()
        {
            if (commandDict == null)
            {
                Debug.Log($"{Name.Red()} 没有初始化!");
                return;
            }

            var key = typeof(T).Name;

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
        /// 移除命令
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Dispose<T>()
        {
            if (commandDict == null)
            {
                Debug.Log($"{Name.Red()} 没有初始化!");
                return;
            }

            var key = typeof(T).Name;

            if (commandDict.ContainsKey(key))
            {
                commandDict.Remove(key);
            }
        }
        
        /// <summary>
        /// 清除命令管理器
        /// </summary>
        internal override void Destroy()
        {
            base.Destroy();
            commandDict = null;
        }
    }
}