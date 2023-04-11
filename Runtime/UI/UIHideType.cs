using Sirenix.OdinInspector;

namespace JFramework
{
    public enum UIHideType
    {
        /// <summary>
        /// 移除并销毁
        /// </summary>
        [LabelText("移除并销毁")] Destroy,

        /// <summary>
        /// 移除不销毁
        /// </summary>
        [LabelText("移除不销毁")] Remove,

        /// <summary>
        /// 无视移除操作
        /// </summary>
        [LabelText("忽视移除")] Ignore,
    }
}