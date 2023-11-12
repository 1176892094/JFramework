// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-10-24  23:38
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

namespace JFramework.Interface
{
    /// <summary>
    /// 控制器接口
    /// </summary>
    public interface IController
    {
        /// <summary>
        /// 控制器注册角色
        /// </summary>
        /// <param name="owner"></param>
        void Register(IEntity owner);
    }

    /// <summary>
    /// 泛型控制器接口
    /// </summary>
    /// <typeparam name="T">传入控制器的所有者</typeparam>
    public interface IController<out T> : IController where T : IEntity
    {
        /// <summary>
        /// 控制器的所有者
        /// </summary>
        T owner { get; }
    }
}