namespace JFramework
{
    public enum UIStateType
    {
        /// <summary>
        /// 默认状态
        /// </summary>
        Default = 0,

        /// <summary>
        /// 冻结状态 (无法隐藏和按钮响应)
        /// </summary>
        Freeze = 1,

        /// <summary>
        /// 永不销毁(能够隐藏和显示)
        /// </summary>
        DontDestroy = 2,
    }

    public enum UILayerType
    {
        /// <summary>
        /// 默认层
        /// </summary>
        Normal,

        /// <summary>
        /// 底层
        /// </summary>
        Bottom,

        /// <summary>
        /// 中层
        /// </summary>
        Middle,

        /// <summary>
        /// 高层
        /// </summary>
        Height,

        /// <summary>
        /// 忽视射线层
        /// </summary>
        Ignore
    }
}