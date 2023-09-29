using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JFramework.Interface;
using UnityEngine;

namespace JFramework
{
    /// <summary>
    /// 泛型数据管理器
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    internal struct Data<TKey>
    {
        /// <summary>
        /// 泛型数据字典
        /// </summary>
        internal static readonly Dictionary<Type, Dictionary<TKey, IData>> dataTable = new Dictionary<Type, Dictionary<TKey, IData>>();

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="field"></param>
        /// <param name="table"></param>
        public static void Add(Type type, FieldInfo field, IDataTable table)
        {
            var dataList = new Dictionary<TKey, IData>();
            for (int i = 0; i < table.Count; i++)
            {
                var data = table.GetData(i);
                var key = (TKey)field.GetValue(data);
                if (!dataList.ContainsKey(key))
                {
                    dataList.Add(key, data);
                }
                else
                {
                    Debug.LogWarning($"{table.GetType().Name.Orange()} 键值重复。 键值：{key.ToString().Red()}");
                }
            }

            dataTable[type] = dataList;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="TData"></typeparam>
        /// <returns></returns>
        public static TData Get<TData>(TKey key) where TData : IData
        {
            if (dataTable.TryGetValue(typeof(TData), out Dictionary<TKey, IData> dataList))
            {
                if (dataList.TryGetValue(key, out IData data))
                {
                    return (TData)data;
                }
            }
            
            return default;
        }

        /// <summary>
        /// 获取数据组
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <returns></returns>
        public static TData[] GetTable<TData>() where TData : IData
        {
            if (!dataTable.TryGetValue(typeof(TData), out Dictionary<TKey, IData> dataList)) return null;
            return dataList.Values.Cast<TData>().ToArray();
        }

        /// <summary>
        /// 清除数据
        /// </summary>
        public static void Clear()
        {
            dataTable.Clear();
        }
    }
}