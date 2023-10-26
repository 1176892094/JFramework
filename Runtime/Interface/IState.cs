// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-10-24  23:36
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

namespace JFramework.Interface
{
    /// <summary>
    /// 状态接口
    /// </summary>
    public interface IState : IEnter, IUpdate, IExit
    {
        /// <summary>
        /// 状态的初始化方法
        /// </summary>
        /// <param name="owner">状态的所有者</param>
        /// <param name="machine">状态机</param>
        void OnAwake(ICharacter owner, IStateMachine machine);
    }
}