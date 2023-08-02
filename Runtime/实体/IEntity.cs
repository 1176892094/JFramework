namespace JFramework.Interface
{
    /// <summary>
    /// 实体接口
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// 实体更新
        /// </summary>
        void Update();
    }
    
    /// <summary>
    /// 角色接口
    /// </summary>
    public interface ICharacter : IEntity
    {
    }
}