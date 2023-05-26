using Sirenix.OdinInspector;

namespace JFramework
{
    /// <summary>
    /// 计时器状态
    /// </summary>
    internal enum TimerState
    {
        /// <summary>
        /// 运行状态
        /// </summary>
        [LabelText("运行")] Run = 0,

        /// <summary>
        /// 暂停状态
        /// </summary>
        [LabelText("停止")] Stop = 1,

        /// <summary>
        /// 完成状态
        /// </summary>
        [LabelText("完成")] Finish = 2,
    }
}