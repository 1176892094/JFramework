using System;

namespace JFramework
{
    /// <summary>
    /// 调试器自定义组件属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    internal class DebugAttribute : Attribute
    {
        /// <summary>
        /// 获取自定义组件的类型
        /// </summary>
        public readonly Type type;

        /// <summary>
        /// 构造函数初始化数据
        /// </summary>
        /// <param name="type">传入自定义组件的类型</param>
        public DebugAttribute(Type type) => this.type = type;
    }
}