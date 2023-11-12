// *********************************************************************************
// # Project: Forest
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-11-11  21:46
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;

namespace JFramework.Interface
{
    /// <summary>
    /// 格子对象
    /// </summary>
    /// <typeparam name="T">可以传入任何数据源</typeparam>
    public interface IGrid<T> : IEntity, IDisposable
    {
        /// <summary>
        /// 格子数据
        /// </summary>
        T item { get; }

        /// <summary>
        /// 设置数据源
        /// </summary>
        /// <param name="item">传入数据源</param>
        void SetItem(T item);

        /// <summary>
        /// 选中格子
        /// </summary>
        void Select();
    }
}