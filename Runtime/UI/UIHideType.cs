using Sirenix.OdinInspector;

namespace JFramework
{
    public enum UIHideType
    {
        /// <summary>
        /// HidePanel => 移除并销毁
        /// Clear => 移除并销毁
        /// </summary>
        [LabelText("移除并销毁")] Destroy = 0,

        /// <summary>
        /// HidePanel => 移除不销毁
        /// Clear => 移除并销毁
        /// </summary>
        [LabelText("仅移除")] Remove = 1,

        /// <summary>
        /// HidePanel => 不移除不销毁
        /// Clear => 不移除不销毁
        /// </summary>
        [LabelText("忽视移除")] Ignore = 2,
    }
}