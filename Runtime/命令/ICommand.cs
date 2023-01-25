namespace JFramework.Interface
{
    /// <summary>
    /// 命令接口
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// 命令接口执行命令
        /// </summary>
        /// <param name="args">传入命令执行的参数</param>
        void Execute(params object[] args);
    }
}