using JFramework.Interface;

namespace JFramework
{
    /// <summary>
    /// 控制器拓展
    /// </summary>
    public static class ControllerExtension
    {
        /// <summary>
        /// 转化为对应的控制器
        /// </summary>
        /// <param name="controller">当前控制器对象</param>
        /// <typeparam name="T">目标控制器对象</typeparam>
        /// <returns>返回目标控制器对象</returns>
        public static T As<T>(this IController controller) where T : IController => (T)controller;
    }
}