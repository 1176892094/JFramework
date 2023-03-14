namespace JFramework.Interface
{
    /// <summary>
    /// 命令接口
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="values">传入的参数</param>
        void OnExecute(params object[] values);
    }
}