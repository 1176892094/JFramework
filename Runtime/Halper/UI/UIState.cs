namespace JFramework
{
    public enum UIState
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
}