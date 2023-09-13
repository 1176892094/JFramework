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
}