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
        /// <param name="value">传入的参数</param>
        void OnExecute(IData value);
    }

    public struct Command: ICommand
    {
        public void OnExecute(IData value)
        {
            
        }
    }
}