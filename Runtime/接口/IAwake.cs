namespace JFramework.Interface
{
    /// <summary>
    /// 状态机醒来的接口
    /// </summary>
    public interface IAwake : IState
    {
        void OnAwake();
    }
}