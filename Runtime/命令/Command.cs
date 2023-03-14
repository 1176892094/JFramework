using JFramework.Interface;

namespace JFramework
{
    /// <summary>
    /// 命令的抽象类
    /// </summary>
    public abstract class Command : ICommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="value">传入的参数</param>
        protected abstract void OnExecute(params object[] value);

        /// <summary>
        /// 通过接口执行命令
        /// </summary>
        /// <param name="value">传入的参数</param>
        void ICommand.OnExecute(params object[] value) => OnExecute(value);
    }
}