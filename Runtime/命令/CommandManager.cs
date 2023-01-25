using System.Collections.Generic;
using JFramework.Interface;
using Sirenix.OdinInspector;

namespace JFramework.Core
{
    /// <summary>
    /// 命令管理器
    /// </summary>
    public sealed class CommandManager : Singleton<CommandManager>
    {
        /// <summary>
        /// 存储命令的字典
        /// </summary>
        [ShowInInspector, ReadOnly, LabelText("命令管理器"), FoldoutGroup("命令管理视图")]
        private Dictionary<string, ICommand> commandDict;


        /// <summary>
        /// 命令管理器初始化
        /// </summary>
        protected override void OnInit(params object[] args)
        {
            commandDict = new Dictionary<string, ICommand>();
        }

        /// <summary>
        /// 通过命令管理器接口执行命令
        /// </summary>
        /// <param name="args">接口执行命令的参数</param>
        /// <typeparam name="T">可以使用所有继承ICommand的类</typeparam>
        public void Execute<T>(params object[] args) where T : ICommand, new()
        {
            var key = typeof(T).Name;
            if (!commandDict.ContainsKey(key))
            {
                var command = new T();
                commandDict.Add(key, command);
                command.Execute(args);
                return;
            }

            commandDict[key].Execute(args);
        }

        /// <summary>
        /// 通过命令管理器接口释放命令
        /// </summary>
        /// <typeparam name="T">可以使用所有继承ICommand的类</typeparam>
        public void Dispose<T>()
        {
            var key = typeof(T).Name;
            if (commandDict.ContainsKey(key))
            {
                commandDict.Remove(key);
            }

            commandDict.Clear();
        }
    }
}