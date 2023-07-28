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
    /// <typeparam name="T"></typeparam>
    internal static class DataManager<T>
    {
        /// <summary>
        /// 泛型数据字典
        /// </summary>
        internal static readonly Dictionary<Type, Dictionary<T, IData>> dataDict = new Dictionary<Type, Dictionary<T, IData>>();

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="field"></param>
        /// <param name="table"></param>
        public static void Add(Type type, FieldInfo field, IDataTable table)
        {
            var dataList = new Dictionary<T, IData>();
            for (int i = 0; i < table.Count; i++)
            {
                var data = table.GetData(i);
                var key = (T)field.GetValue(data);
                if (!dataList.ContainsKey(key))
                {
                    dataList.Add(key, data);
                }
                else
                {
                    Debug.Log($"{nameof(DataManager).Sky()} 加载 => {table.GetType().Name.Orange()} 已经存在 {key.ToString().Red()} 键值!");
                }
            }

            dataDict.Add(type, dataList);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="TData"></typeparam>
        /// <returns></returns>
        public static TData Get<TData>(T key) where TData : IData
        {
            if (dataDict.TryGetValue(typeof(TData), out Dictionary<T, IData> dataList))
            {
                if (dataList.TryGetValue(key, out IData data))
                {
                    Log.Info(DebugOption.Data, $"获取 => {typeof(TData).Name.Blue()} : {key.ToString().Green()} 数据成功");
                    return (TData)data;
                }
            }

            Log.Info(DebugOption.Data, $"获取 => {typeof(TData).Name.Red()} : {key.ToString().Green()} 数据失败");
            return default;
        }

        /// <summary>
        /// 获取数据组
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <returns></returns>
        public static TData[] GetTable<TData>() where TData : IData
        {
            if (!dataDict.TryGetValue(typeof(T), out Dictionary<T, IData> dataList)) return null;
            Log.Info(DebugOption.Data, $"获取 => {typeof(TData).Name.Blue()} 列表成功");
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