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
        void Register(ICharacter owner);
    }

    /// <summary>
    /// 控制器注册接口
    /// </summary>
    public interface IRegister : IController
    {
        /// <summary>
        /// 继承后在Start后调用
        /// </summary>
        void Register();
    }
}