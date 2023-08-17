using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JFramework.Core;
using JFramework.Interface;
using UnityEngine;

namespace JFramework
{
    /// <summary>
    /// 泛型数据管理器
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    internal static class Data<TKey>
    {
        /// <summary>
        /// 泛型数据字典
        /// </summary>
        internal static readonly Dictionary<Type, Dictionary<TKey, IData>> dataDict = new Dictionary<Type, Dictionary<TKey, IData>>();

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
                    Debug.Log($"{table.GetType().Name.Orange()} 中有重复键值！ Key：{key.ToString().Red()}");
                }
            }

            dataDict[type] = dataList;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="TData"></typeparam>
        /// <returns></returns>
        public static TData Get<TData>(TKey key) where TData : IData
        {
            if (dataDict.TryGetValue(typeof(TData), out Dictionary<TKey, IData> dataList))
            {
                if (dataList.TryGetValue(key, out IData data))
                {
                    Log.Info($"获取 {typeof(TData).Name.Blue()} : {key.ToString().Green()} 数据成功", Option.Data);
                    return (TData)data;
                }
            }

            Log.Info($"获取 {typeof(TData).Name.Red()} : {key.ToString().Green()} 数据失败", Option.Data);
            return default;
        }

        /// <summary>
        /// 获取数据组
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <returns></returns>
        public static TData[] GetTable<TData>() where TData : IData
        {
            if (!dataDict.TryGetValue(typeof(TKey), out Dictionary<TKey, IData> dataList)) return null;
            Log.Info($"获取 {typeof(TData).Name.Blue()} 列表成功", Option.Data);
            return dataList.Values.Cast<TData>().ToArray();
        }

        /// <summary>
        /// 清除数据
        /// </summary>
        public static void Clear()
        {
            dataDict.Clear();
        }
    }
}