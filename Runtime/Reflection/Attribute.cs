using System;

// ReSharper disable All

namespace JFramework
{
    /// <summary>
    /// 数据表主键
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class KeyAttribute : Attribute
    {
    }
    
    
    /// <summary>
    /// 根据 对象名称 进行赋值
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class UIFieldAttribute : Attribute
    {
        public readonly string name;

        public UIFieldAttribute(string name) => this.name = name;
    }

    /// <summary>
    /// 根据 对象名称 调用方法
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class UIMethodAttribute : Attribute
    {
        public readonly string name;
        public UIMethodAttribute(string name) => this.name = name;
    }
}