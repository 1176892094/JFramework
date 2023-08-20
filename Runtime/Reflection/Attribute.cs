using System;

// ReSharper disable All

namespace JFramework
{
    /// <summary>
    /// 数据表主键
    /// </summary>
    public class KeyAttribute : Attribute
    {
    }

    /// <summary>
    /// 根据 对象名称 调用方法
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class InvokeAttribute : Attribute
    {
        public readonly string name;

        public InvokeAttribute(string name) => this.name = name;
    }
}