using JFramework.Core;
using JFramework.Interface;

namespace JFramework
{
    /// <summary>
    /// 计时器拓展
    /// </summary>
    public static class TimerExtension
    {
        /// <summary>
        /// 对Timer的拓展方法
        /// </summary>
        /// <param name="timer">timer自身</param>
        public static void Push(this ITimer timer)
        {
            TimerManager.Push(timer);
        }
    }
}