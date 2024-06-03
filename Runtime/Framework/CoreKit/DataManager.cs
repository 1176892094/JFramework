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
using Debug = UnityEngine.Debug;

namespace JFramework.Core
{
    public static class DataManager
    {
        internal static readonly Dictionary<Type, Dictionary<int, IData>> intData = new();
        internal static readonly Dictionary<Type, Dictionary<Enum, IData>> enumData = new();
        internal static readonly Dictionary<Type, Dictionary<string, IData>> stringData = new();

        public static async Task LoadDataTable()
        {
            var assembly = Reflection.GetAssembly(SettingManager.Instance.dataAssembly);
            var types = Reflection.GetTypes<IDataTable>(assembly);
            if (types == null || types.Length == 0) return;
            foreach (var type in types)
            {
                try
                {
                    var obj = await AssetManager.Load<ScriptableObject>(SettingManager.GetTablePath(type.Name));
                    var table = (IDataTable)obj;
                    if (type.FullName == null) return;
                    var data = assembly.GetType(type.FullName[..^5]);
                    var property = Reflection.GetProperty<KeyAttribute>(data);
                    if (property == null)
                    {
                        Debug.LogWarning($"{data.Name.Red()} 缺少主键。");
                        return;
                    }

                    if (property.PropertyType.IsEnum)
                    {
                        enumData.Add(data, Add<Enum>(property, table));
                    }
                    else if (property.PropertyType == typeof(int))
                    {
                        intData.Add(data, Add<int>(property, table));
                    }
                    else if (property.PropertyType == typeof(string))
                    {
                        stringData.Add(data, Add<string>(property, table));
                    }
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"加载 {type.Name.Red()} 数据失败\n{e}");
                }
            }
        }

        private static Dictionary<T, IData> Add<T>(PropertyInfo property, IDataTable table)
        {
            var dataList = new Dictionary<T, IData>();
            for (int i = 0; i < table.Count; i++)
            {
                var data = table.GetData(i);
                var key = (T)property.GetValue(data);
                if (!dataList.TryAdd(key, data))
                {
                    Debug.LogWarning($"{table.GetType().Name.Orange()} 键值重复。 键值：{key.ToString().Red()}");
                }
            }

            return dataList;
        }

        public static T Get<T>(int key) where T : IData
        {
            if (!GlobalManager.Instance) return default;
            if (!intData.TryGetValue(typeof(T), out var dataList))
            {
                return default;
            }

            if (!dataList.TryGetValue(key, out IData data))
            {
                return default;
            }

            return (T)data;
        }

        public static T Get<T>(Enum key) where T : IData
        {
            if (!GlobalManager.Instance) return default;
            if (!enumData.TryGetValue(typeof(T), out var dataList))
            {
                return default;
            }

            if (!dataList.TryGetValue(key, out IData data))
            {
                return default;
            }

            return (T)data;
        }

        public static T Get<T>(string key) where T : IData
        {
            if (!GlobalManager.Instance) return default;
            if (!stringData.TryGetValue(typeof(T), out var dataList))
            {
                return default;
            }

            if (!dataList.TryGetValue(key, out IData data))
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