using System;
using JFramework.Interface;

namespace JFramework
{
    /// <summary>
    /// 命令的抽象类
    /// </summary>
    [Serializable]
    public abstract class Command : ICommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="args">命令执行的参数</param>
        protected abstract void OnExecute(params object[] args);

        /// <summary>
        /// 通过命令接口执行命令
        /// </summary>
        /// <param name="args">命令执行的参数</param>
        void ICommand.Execute(params object[] args) => OnExecute(args);
    }
}