// *********************************************************************************
// # Project: Forest
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-11-12  01:13
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using JFramework.Interface;

namespace JFramework.Core
{
    /// <summary>
    /// 物品管理器
    /// </summary>
    public static class ItemManager
    {
        /// <summary>
        /// 存储所有数据
        /// </summary>
        private static readonly Dictionary<Type, IItemData> items = new Dictionary<Type, IItemData>();

        /// <summary>
        /// 添加指定类型的数据
        /// </summary>
        /// <param name="data"></param>
        /// <typeparam name="TItem"></typeparam>
        public static void AddItems<TItem>(IItemData data) where TItem : IItem => items.Add(typeof(TItem), data);
        
        /// <summary>
        /// 获取数据组
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <returns></returns>
        public static TItem[] GetItems<TItem>() where TItem : IItem
        {
            return items.TryGetValue(typeof(TItem), out var data) ? data.GetItems().Cast<TItem>().ToArray() : null;
        }
    }
}