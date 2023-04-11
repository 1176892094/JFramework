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
        /// <param name="value">传入生成的参数</param>
        void Spawn(params object[] value);
        
        /// <summary>
        /// 实体的更新方法
        /// </summary>
        void Update();
    }
}