namespace JFramework.Interface
{
    /// <summary>
    /// 实体接口
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// 实体生成
        /// </summary>
        /// <param name="args">传入生成时的参数</param>
        void Spawn(params object[] args);
        
        /// <summary>
        /// 实体销毁
        /// </summary>
        void Despawn();
    }

    public interface IUpdate
    {
        void Update();
    }
}