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
        /// 实体更新
        /// </summary>
        void Update();

        /// <summary>
        /// 实体销毁
        /// </summary>
        void Despawn();
    }
}