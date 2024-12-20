// *********************************************************************************
// # Project: Forest
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-08-25  01:08
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;

namespace JFramework.Interface
{
    public interface IGrid: IEntity
    {
        void Select();
    }

    public interface IGrid<T> : IGrid, IDisposable
    {
        T item { get; }

        void SetItem(T item);
    }
}