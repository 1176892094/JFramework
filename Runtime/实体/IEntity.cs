namespace JFramework.Interface
{
    /// <summary>
    /// 实体接口
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// 实体接口初始化方法
        /// </summary>
        /// <param name="values">可以传入任何参数进行初始化</param>
        void Start(params object[] values);

        /// <summary>
        /// 实体的更新方法
        /// </summary>
        void OnUpdate();
    }
}