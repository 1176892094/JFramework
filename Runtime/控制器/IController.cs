namespace JFramework.Interface
{
    /// <summary>
    /// 控制器接口
    /// </summary>
    public interface IController
    {
        /// <summary>
        /// 控制器生成时调用
        /// </summary>
        /// <param name="owner">传入控制器的拥有者</param>
        /// <returns></returns>
        void Spawn(IEntity owner);
    }
}