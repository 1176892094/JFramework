using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JFramework.Interface;
using JFramework.Table;
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
        /// 数据数量
        /// </summary>
        private static int DataCount;
        
        /// <summary>
        /// 加载进度
        /// </summary>
        private static float LoadProgress;

        /// <summary>
        /// 数据加载完成的回调
        /// </summary>
        public static event Action OnCompleted;

        /// <summary>
        /// 构造函数初始化数据
        /// </summary>
        internal static async void Awake()
        {
            IntDataDict = new Dictionary<Type, IntDataDict>();
            StrDataDict = new Dictionary<Type, StrDataDict>();
            EnmDataDict = new Dictionary<Type, EnmDataDict>();
            var (assembly, types) = GetAssemblyAndTypes();
            if (types == null || types.Length == 0) return;
            LoadProgress = 0f;
            float time = Time.time;
            DataCount = types.Length;
            foreach (var type in types)
            {
                var keyName = type.Name;
                try
                {
                    var table = await AssetManager.LoadAsync<ScriptableObject>("DataTable/" + keyName);
                    var dataTable = table.As<IDataTable>();
                    var keyData = GetKeyField(assembly, type);
                    var keyInfo = GetKeyField<KeyFieldAttribute>(keyData);
                    if (keyData == null)
                    {
                        Log.Info($"{Name.Sky()} 缺少主键 => {type.Name.Red()}");
                        return;
                    }

                    LoadProgress += 1f / DataCount;
                    var progress = (LoadProgress * 100).ToString("F") + "%";
                    Log.Info(DebugOption.Data, $"加载 => {type.Name.Blue()} 进度: {progress.Green()}");
                    
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
                }
                catch (Exception)
                {
                    Log.Info($"{Name.Sky()} 加载 => {type.Name.Red()} 数据失败");
                }
            }

            var totalTime = (Time.time - time).ToString("F");
            Log.Info($"{Name.Sky()} 加载 => 所有数据完成, 耗时 {totalTime.Yellow()} 秒");
            OnCompleted?.Invoke();
        }

        /// <summary>
        /// 增加字典数据
        /// </summary>
        /// <param name="field"></param>
        /// <param name="table"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <returns>返回数据字典</returns>
        private static Dictionary<TKey, IData> Add<TKey>(FieldInfo field, IDataTable table)
        {
            var dataDict = new Dictionary<TKey, IData>();
            for (var i = 0; i < table.Count; ++i)
            {
                var data = table.GetData(i);
                var key = (TKey)field.GetValue(data);
                if (!dataDict.ContainsKey(key))
                {
                    dataDict.Add(key, data);
                }
                else
                {
                    Log.Info($"{Name.Sky()} 加载 => {table.GetType().Name.Orange()} 已经存在 {key.ToString().Red()} 键值!");
                }
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
            if (IntDataDict.TryGetValue(typeof(T), out IntDataDict soDict))
            {
                if (soDict.TryGetValue(key, out IData data))
                {
                    Log.Info(DebugOption.Data, $"获取 => {typeof(T).Name.Blue()} : {key.ToString().Green()} 数据成功");
                    return (T)data;
                }
            }

            Log.Info(DebugOption.Data, $"获取 => {typeof(T).Name.Red()} : {key.ToString().Green()} 数据失败");
            return default;
        }

        /// <summary>
        /// 获取对应类型数据下主键为Key的数据
        /// </summary>
        /// <param name="key">传入的string主键</param>
        /// <typeparam name="T">要获取数据的类型,必须继承自JFramework.Data</typeparam>
        /// <returns>返回一个数据对象</returns>
        public static T Get<T>(string key) where T : IData
        {
            if (StrDataDict.TryGetValue(typeof(T), out StrDataDict soDict))
            {
                if (soDict.TryGetValue(key, out IData data))
                {
                    Log.Info(DebugOption.Data, $"获取 => {typeof(T).Name.Blue()} : {key.Green()} 数据成功");
                    return (T)data;
                }
            }

            Log.Info(DebugOption.Data, $"获取 => {typeof(T).Name.Red()} : {key.Green()} 数据失败");
            return default;
        }

        /// <summary>
        /// 获取对应类型数据下主键为Key的数据
        /// </summary>
        /// <param name="key">传入的string主键</param>
        /// <typeparam name="T">要获取数据的类型,必须继承自JFramework.Data</typeparam>
        /// <returns>返回一个数据对象</returns>
        public static T Get<T>(Enum key) where T : IData
        {
            if (EnmDataDict.TryGetValue(typeof(T), out EnmDataDict soDict))
            {
                if (soDict.TryGetValue(key, out IData data))
                {
                    Log.Info(DebugOption.Data, $"获取 => {typeof(T).Name.Blue()} : {key.ToString().Green()} 数据成功");
                    return (T)data;
                }
            }

            Log.Info(DebugOption.Data, $"获取 => {typeof(T).Name.Red()} : {key.ToString().Green()} 数据失败");
            return default;
        }


        /// <summary>
        /// 通过数据管理器得到数据表
        /// </summary>
        /// <typeparam name="T">可以使用所有继承Data类型的对象</typeparam>
        /// <returns>返回泛型列表</returns>
        public static T[] GetTable<T>() where T : IData
        {
            var table = GetTable(typeof(T));
            if (table != default)
            {
                Log.Info(DebugOption.Data, $"获取 => {typeof(T).Name.Blue()} 列表成功");
            }

            return table?.Cast<T>().ToArray();
        }

        /// <summary>
        /// 通过数据管理器得到数据表
        /// </summary>
        /// <param name="type">传入的类型</param>
        /// <returns>返回一个Data的列表</returns>
        private static IData[] GetTable(Type type)
        {
            if (IntDataDict.TryGetValue(type, out IntDataDict dictInt))
            {
                return dictInt.Values.ToArray();
            }

            if (EnmDataDict.TryGetValue(type, out EnmDataDict dictEnm))
            {
                return dictEnm.Values.ToArray();
            }

            if (StrDataDict.TryGetValue(type, out StrDataDict dictStr))
            {
                return dictStr?.Values.ToArray();
            }

            return default;
        }

        /// <summary>
        /// 获取当前程序集中的数据表对象类
        /// </summary>
        /// <returns></returns>
        private static (Assembly, Type[]) GetAssemblyAndTypes()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = GetTypes(assembly);
                if (types.Length > 0)
                {
                    return (assembly, types);
                }
            }

            return (null, null);
        }
        
        /// <summary>
        /// 获取程序集中的类型
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        private static Type[] GetTypes(Assembly assembly)
        {
            return assembly.GetTypes().Where(type => typeof(IDataTable).IsAssignableFrom(type) && !type.IsAbstract).ToArray();
        }

        /// <summary>
        /// 获取数据的主键
        /// </summary>
        /// <param name="type">传入的数据类型</param>
        /// <returns>返回字段的信息</returns>
        private static FieldInfo GetKeyField<T>(Type type) where T : class
        {
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            return fields.FirstOrDefault(field => field.GetCustomAttributes(typeof(T), false).Length > 0);
        }

        /// <summary>
        /// 得到数据表对象的类型
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Type GetKeyField(Assembly assembly, Type type)
        {
            return assembly.GetType("JFramework.Table." + type.Name[..^5]);
        }

        /// <summary>
        /// 清除数据管理器
        /// </summary>
        internal static void Destroy()
        {
            DataCount = 0;
            LoadProgress = 0;
            IntDataDict = null;
            StrDataDict = null;
            EnmDataDict = null;
            OnCompleted = null;
        }
    }
}