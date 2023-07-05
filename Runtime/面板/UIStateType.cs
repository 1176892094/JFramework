namespace JFramework
{
    public enum UIStateType
    {
        /// <summary>
        /// HidePanel => 移除并销毁
        /// Clear => 移除并销毁
        /// </summary>
        Common = 0,

        /// <summary>
        /// HidePanel => 不移除不销毁
        /// Clear => 移除并销毁
        /// </summary>
        Freeze = 1,

        /// <summary>
        /// HidePanel => 不移除不销毁
        /// Clear => 不移除不销毁
        /// </summary>
        Ignore = 2,
    }
}