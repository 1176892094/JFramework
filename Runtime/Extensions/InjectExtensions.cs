using JFramework.Interface;

namespace JFramework
{
    /// <summary>
    /// 注入接口的拓展
    /// </summary>
    public static class InjectExtensions
    {
        /// <summary>
        /// 对自身进行依赖注入
        /// </summary>
        /// <param name="inject"></param>
        public static void Inject(this IInject inject)
        {
            inject.Inject(inject.transform);
        }
    }
}