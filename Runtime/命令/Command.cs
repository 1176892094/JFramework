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
        /// <param name="values">传入的参数</param>
        protected abstract void OnExecute(params object[] values);

        /// <summary>
        /// 通过接口执行命令
        /// </summary>
        /// <param name="values">传入的参数</param>
        void ICommand.OnExecute(params object[] values) => OnExecute(values);
    }
}