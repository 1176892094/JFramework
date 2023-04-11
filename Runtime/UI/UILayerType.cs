namespace JFramework
{
    /// <summary>
    /// 用户面板层级类型枚举
    /// </summary>
    public enum UILayerType
    {
        /// <summary>
        /// 最底层
        /// </summary>
        Layer1 = 0,

        /// <summary>
        /// 较底层
        /// </summary>
        Layer2 = 1,

        /// <summary>
        /// 中间层
        /// </summary>
        Layer3 = 2,

        /// <summary>
        /// 较高层
        /// </summary>
        Layer4 = 3,

        /// <summary>
        /// 最高层 (忽视射线)
        /// </summary>
        Layer5 = 4,
    }
}