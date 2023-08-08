namespace JFramework.Interface
{
    /// <summary>
    /// 状态机泛型接口
    /// </summary>
    /// <typeparam name="TCharacter"></typeparam>
    public interface IState<in TCharacter> : IState where TCharacter : ICharacter
    {
        /// <summary>
        /// 状态的初始化方法
        /// </summary>
        /// <param name="owner">状态的所有者</param>
        /// <param name="baseMachine">状态机</param>
        void OnAwake(TCharacter owner, IStateMachine baseMachine);
    }
}