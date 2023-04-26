namespace JFramework.Interface
{
    /// <summary>
    /// 控制器接口
    /// </summary>
    public interface IController
    {
        /// <summary>
        /// 控制器初始化
        /// </summary>
        /// <param name="owner">控制器的所有者</param>
        void Spawn(IEntity owner);

        /// <summary>
        /// 控制器清除
        /// </summary>
        void Despawn();
    }
}