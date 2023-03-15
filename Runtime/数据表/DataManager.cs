using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace JFramework.Core
{
    using IntDataDict = Dictionary<int, IData>;
    using StrDataDict = Dictionary<string, IData>;
    using EnmDataDict = Dictionary<Enum, IData>;

    public static class DataManager
    {
        /// <summary>
        /// 存储int为主键类型的数据字典
        /// </summary>
        internal static Dictionary<Type, IntDataDict> IntDataDict;

        /// <summary>
        /// 存储string为主键的数据字典
        /// </summary>
        internal static Dictionary<Type, StrDataDict> StrDataDict;

        /// <summary>
        /// 存储enum为主键的数据字典
        /// </summary>
        internal static Dictionary<Type, EnmDataDict> EnmDataDict;

        /// <summary>
        /// 管理器名称
        /// </summary>
        private static string Name => nameof(DataManager);

        /// <summary>
        /// 构造函数初始化数据
        /// </summary>
        internal static void Awake()
        {
            IntDataDict = new Dictionary<Type, IntDataDict>();
            StrDataDict = new Dictionary<Type, StrDataDict>();
            EnmDataDict = new Dictionary<Type, EnmDataDict>();
            var assembly = GetAssembly(out var types);
            if (types == null || types.Length == 0) return;
            foreach (var type in types)
            {
                var keyName = type.Name;
                AssetManager.LoadAsync<ScriptableObject>("DataTable/" + keyName, table =>
                {
                    var dataTable = table.As<IDataTable>();
                    var keyData = GetKeyField(assembly, type);
                    var keyInfo = GetKeyField(keyData);
                    if (keyData == null)
                    {
                        Debug.Log($"{Name.Sky()} 缺少主键 => {type.Name.Red()}");
                        return;
                    }

                    if (GlobalManager.Instance.IsDebugData)
                    {
                        Debug.Log($"{Name.Sky()} 加载 => {type.Name.Blue()} 数据表");
                    }

                    var keyType = keyInfo.FieldType;
                    if (keyType == typeof(int))
                    {
                        IntDataDict.Add(keyData, Add<int>(keyInfo, dataTable));
                    }
                    else if (keyType == typeof(string))
                    {
                        StrDataDict.Add(keyData, Add<string>(keyInfo, dataTable));
                    }
                    else if (keyType.IsEnum)
                    {
                        EnmDataDict.Add(keyData, Add<Enum>(keyInfo, dataTable));
                    }
                    else
                    {
                        Debug.Log($"{Name.Sky()} 加载 => {type.Name.Red()} 数据失败");
                    }
                });
            }
        }

        /// <summary>
        /// 增加字典数据
        /// </summary>
        /// <param name="field"></param>
        /// <param name="table"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>返回数据字典</returns>
        private static Dictionary<T, IData> Add<T>(FieldInfo field, IDataTable table)
        {
            var dataDict = new Dictionary<T, IData>();
            for (var i = 0; i < table.Count; ++i)
            {
                var data = (IData)table.GetData(i);
                var key = (T)field.GetValue(data);
                dataDict.Add(key, data);
            }

            return dataDict;
        }

        /// <summary>
        /// 获取对应类型数据下主键为Key的数据
        /// </summary>
        /// <param name="key">传入的int主键</param>
        /// <typeparam name="T">可以使用所有继承IData类型的对象</typeparam>
        /// <returns>返回一个数据对象</returns>
        public static T Get<T>(int key) where T : IData
        {
            IntDataDict.TryGetValue(typeof(T), out IntDataDict soDict);
            if (soDict == null) return default;
            soDict.TryGetValue(key, out IData data);
            if (GlobalManager.Instance.IsDebugData)
            {
                Debug.Log(data != null
                    ? $"{Name.Sky()} 获取 => {typeof(T).Name.Blue()} : {key.ToString().Green()} 数据成功"
                    : $"{Name.Sky()} 获取 => {typeof(T).Name.Red()} : {key.ToString().Green()} 数据失败");
            }

            return (T)data;
        }

        /// <summary>
        /// 获取对应类型数据下主键为Key的数据
        /// </summary>
        /// <param name="key">传入的string主键</param>
        /// <typeparam name="T">要获取数据的类型,必须继承自JFramework.Data</typeparam>
        /// <returns>返回一个数据对象</returns>
        public static T Get<T>(string key) where T : IData
        {
            StrDataDict.TryGetValue(typeof(T), out StrDataDict soDict);
            if (soDict == null) return default;
            soDict.TryGetValue(key, out IData data);
            if (GlobalManager.Instance.IsDebugData)
            {
                Debug.Log(data != null
                    ? $"{Name.Sky()} 获取 => {typeof(T).Name.Blue()} : {key.Green()} 数据成功"
                    : $"{Name.Sky()} 获取 => {typeof(T).Name.Red()} : {key.Green()} 数据失败");
            }

            return (T)data;
        }

        /// <summary>
        /// 获取对应类型数据下主键为Key的数据
        /// </summary>
        /// <param name="key">传入的string主键</param>
        /// <typeparam name="T">要获取数据的类型,必须继承自JFramework.Data</typeparam>
        /// <returns>返回一个数据对象</returns>
        public static T Get<T>(Enum key) where T : IData
        {
            StrDataDict.TryGetValue(typeof(T), out StrDataDict soDict);
            if (soDict == null) return default;
            soDict.TryGetValue(key.ToString(), out IData data);
            if (GlobalManager.Instance.IsDebugData)
            {
                Debug.Log(data != null
                    ? $"{Name.Sky()} 获取 => {typeof(T).Name.Blue()} : {key.ToString().Green()} 数据成功"
                    : $"{Name.Sky()} 获取 => {typeof(T).Name.Red()} : {key.ToString().Green()} 数据失败");
            }

            return (T)data;
        }


        /// <summary>
        /// 通过数据管理器得到数据表
        /// </summary>
        /// <typeparam name="T">可以使用所有继承Data类型的对象</typeparam>
        /// <returns>返回泛型列表</returns>
        public static List<T> GetTable<T>() where T : IData
        {
            var table = GetTable(typeof(T));
            if (GlobalManager.Instance.IsDebugData && table != null)
            {
                Debug.Log($"{Name.Sky()} 获取 => {typeof(T).Name.Blue()} 列表成功");
            }

            return table?.Cast<T>().ToList();
        }

        //table.ForEach(data => dataList.Add((T)data));

        /// <summary>
        /// 通过数据管理器得到数据表
        /// </summary>
        /// <param name="type">传入的类型</param>
        /// <returns>返回一个Data的列表</returns>
        private static List<IData> GetTable(Type type)
        {
            IntDataDict.TryGetValue(type, out IntDataDict dictInt);
            if (dictInt != null) return dictInt.Values.ToList();
            StrDataDict.TryGetValue(type, out StrDataDict dictStr);
            return dictStr?.Values.ToList();
        }

        /// <summary>
        /// 获取当前程序集中的数据表对象类
        /// </summary>
        /// <returns></returns>
        private static Assembly GetAssembly(out Type[] types)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var typeList = assembly.GetTypes();
                types = typeList.Where(IsSupport).ToArray();
                if (types.Length > 0) return assembly;
            }

            types = null;
            return null;
        }

        /// <summary>
        /// 是否支持类型
        /// </summary>
        /// <param name="type">传入类型</param>
        /// <returns>返回是否支持</returns>
        private static bool IsSupport(Type type)
        {
            if (!type.GetInterfaces().Contains(typeof(IDataTable))) return false;
            return type.BaseType != typeof(ScriptableObject);
        }

        /// <summary>
        /// 获取数据的主键
        /// </summary>
        /// <param name="type">传入的数据类型</param>
        /// <returns>返回字段的信息</returns>
        private static FieldInfo GetKeyField(Type type)
        {
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            return (from field in fields
                let attrs = field.GetCustomAttributes(typeof(KeyAttribute), false)
                where attrs.Length > 0
                select field).FirstOrDefault();
        }

        /// <summary>
        /// 得到数据表对象的类型
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Type GetKeyField(Assembly assembly, Type type)
        {
            var name = type.Name;
            name = name.Substring(0, name.Length - 5);
            return assembly.GetType("JFramework.Table." + name);
        }

        /// <summary>
        /// 获取数据的键值
        /// </summary>
        /// <returns>返回数据的主键</returns>
        public static object GetKeyField(IData data)
        {
            var key = GetKeyField(data.GetType());
            return key == null ? null : key.GetValue(data);
        }

        /// <summary>
        /// 清除数据管理器
        /// </summary>
        internal static void Destroy()
        {
            IntDataDict = null;
            StrDataDict = null;
        }
    }
}