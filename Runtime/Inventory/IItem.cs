// *********************************************************************************
// # Project: Forest
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-11-12  01:07
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;

namespace JFramework.Interface
{
    /// <summary>
    /// 物品数据
    /// </summary>
    public interface IItem : IDisposable
    {
    }

    /// <summary>
    /// 泛型物品数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IItem<in T> : IItem
    {
        void SetItem(T item);
    }
}