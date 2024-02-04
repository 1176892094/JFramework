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
using Sirenix.OdinInspector;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace JFramework.Core
{
    public sealed class DataManager : Component<GlobalManager>
    {
        [ShowInInspector]
        private readonly Dictionary<Type, Dictionary<int, IData>> intData = new Dictionary<Type, Dictionary<int, IData>>();

        [ShowInInspector]
        private readonly Dictionary<Type, Dictionary<Enum, IData>> enumData = new Dictionary<Type, Dictionary<Enum, IData>>();

        [ShowInInspector]
        private readonly Dictionary<Type, Dictionary<string, IData>> stringData = new Dictionary<Type, Dictionary<string, IData>>();

        public async Task LoadDataTable()
        {
            var assembly = Reflection.GetAssembly("HotUpdate");
            if (assembly == null)
            {
                assembly = Reflection.GetAssembly("Assembly-CSharp");
            }

            var types = Reflection.GetTypes<IDataTable>(assembly);
            if (types == null || types.Length == 0) return;
            foreach (var type in types)
            {
                try
                {
                    var obj = await GlobalManager.Asset.Load<ScriptableObject>(GlobalSetting.GetTablePath(type.Name));
                    var table = (IDataTable)obj;
                    if (type.FullName == null) return;
                    var data = assembly.GetType(type.FullName[..^5]);
                    var field = Reflection.GetField<KeyAttribute>(data);
                    if (field == null)
                    {
                        Debug.LogWarning($"{data.Name.Red()} 缺少主键。");
                        return;
                    }

                    if (field.FieldType.IsEnum)
                    {
                        enumData.Add(data, Add<Enum>(field, table));
                    }
                    else if (field.FieldType == typeof(int))
                    {
                        intData.Add(data, Add<int>(field, table));
                    }
                    else if (field.FieldType == typeof(string))
                    {
                        stringData.Add(data, Add<string>(field, table));
                    }
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"加载 {type.Name.Red()} 数据失败\n{e}");
                }
            }
        }

        private static Dictionary<T, IData> Add<T>(FieldInfo field, IDataTable table)
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
                    Debug.LogWarning($"{table.GetType().Name.Orange()} 键值重复。 键值：{key.ToString().Red()}");
                }
            }

            return dataList;
        }

        public T Get<T>(int key) where T : IData
        {
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

        public T Get<T>(Enum key) where T : IData
        {
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

        public T Get<T>(string key) where T : IData
        {
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

        public T[] GetTable<T>() where T : IData
        {
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

        private void OnDestroy()
        {
            intData.Clear();
            enumData.Clear();
            stringData.Clear();
        }
    }
}