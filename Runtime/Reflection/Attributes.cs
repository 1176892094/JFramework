// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-10-24  23:42
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

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
    public class InjectAttribute : Attribute
    {
        public readonly string name;

        public InjectAttribute(string name) => this.name = name;
    }
}