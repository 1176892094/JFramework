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
    public class UIFindAttribute : Attribute
    {
        public readonly string find;

        public UIFindAttribute(string find) => this.find = find;
    }
}