// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-02-04  18:16
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using JFramework.Interface;
using UnityEngine;

namespace JFramework
{
    public static class DataManager
    {
        private static readonly Dictionary<Type, Dictionary<int, IData>> intData = new();
        private static readonly Dictionary<Type, Dictionary<Enum, IData>> enumData = new();
        private static readonly Dictionary<Type, Dictionary<string, IData>> stringData = new();

        public static async Task LoadDataTable()
        {
            var assembly = Reflection.GetAssembly(GlobalSetting.Instance.dataAssembly);
            var types = assembly.GetTypes().Where(type => typeof(IDataTable).IsAssignableFrom(type)).ToArray();
            if (types.Length == 0) return;
            foreach (var type in types)
            {
                try
                {
                    var table = await AssetManager.Load<ScriptableObject>(GlobalSetting.GetTablePath(type.Name));
                    if (type.FullName == null) continue;
                    var children = assembly.GetType(type.FullName[..^5]);
                    var properties = children.GetProperties(Reflection.Instance);
                    var property = properties.FirstOrDefault(info => info.GetCustomAttributes(typeof(KeyAttribute), false).Length > 0);
                    if (property == null)
                    {
                        Debug.LogWarning($"{children.Name.Red()} 缺少主键。");
                        return;
                    }

                    if (property.PropertyType.IsEnum)
                    {
                        enumData.Add(children, Add<Enum>(property, (IDataTable)table));
                    }
                    else if (property.PropertyType == typeof(int))
                    {
                        intData.Add(children, Add<int>(property, (IDataTable)table));
                    }
                    else if (property.PropertyType == typeof(string))
                    {
                        stringData.Add(children, Add<string>(property, (IDataTable)table));
                    }
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"加载 {type.Name.Red()} 数据失败\n{e}");
                }
            }
        }

        private static Dictionary<T, IData> Add<T>(PropertyInfo property, IDataTable dataTable)
        {
            var dataList = new Dictionary<T, IData>();
            for (int i = 0; i < dataTable.Count; i++)
            {
                var data = dataTable.GetData(i);
                var key = (T)property.GetValue(data);
                if (!dataList.TryAdd(key, data))
                {
                    Debug.LogWarning($"{dataTable.GetType().Name.Orange()} 键值重复。 键值：{key.ToString().Red()}");
                }
            }

            return dataList;
        }

        public static T Get<T>(int key) where T : IData
        {
            if (!GlobalManager.Instance) return default;
            if (!intData.TryGetValue(typeof(T), out var dataTable))
            {
                return default;
            }

            if (!dataTable.TryGetValue(key, out IData data))
            {
                return default;
            }

            return (T)data;
        }

        public static T Get<T>(Enum key) where T : IData
        {
            if (!GlobalManager.Instance) return default;
            if (!enumData.TryGetValue(typeof(T), out var dataTable))
            {
                return default;
            }

            if (!dataTable.TryGetValue(key, out IData data))
            {
                return default;
            }

            return (T)data;
        }

        public static T Get<T>(string key) where T : IData
        {
            if (!GlobalManager.Instance) return default;
            if (!stringData.TryGetValue(typeof(T), out var dataTable))
            {
                return default;
            }

            if (!dataTable.TryGetValue(key, out IData data))
            {
                return default;
            }

            return (T)data;
        }

        public static T[] GetTable<T>() where T : IData
        {
            if (!GlobalManager.Instance) return default;
            if (intData.TryGetValue(typeof(T), out var intList))
            {
                return intList?.Values.Cast<T>().ToArray();
            }

            if (enumData.TryGetValue(typeof(T), out var enumList))
            {
                return enumList?.Values.Cast<T>().ToArray();
            }

            if (stringData.TryGetValue(typeof(T), out var stringList))
            {
                return stringList?.Values.Cast<T>().ToArray();
            }

            Debug.LogWarning($"获取 {typeof(T).Name.Red()} 失败！");
            return default;
        }

        internal static void UnRegister()
        {
            intData.Clear();
            enumData.Clear();
            stringData.Clear();
        }
    }
}