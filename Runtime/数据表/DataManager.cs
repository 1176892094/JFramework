using System;
using System.Linq;
using System.Reflection;
using JFramework.Interface;
using JFramework.Table;
using UnityEngine;

namespace JFramework.Core
{
    /// <summary>
    /// 数据管理器
    /// </summary>
    public static class DataManager
    {
        /// <summary>
        /// 初始化加载数据
        /// </summary>
        public static void Awake()
        {
            AssetManager.OnLoadComplete += LoadDataTable;
        }

        /// <summary>
        /// 加载数据表
        /// ReSharper disable once MemberCanBePrivate.Global
        /// </summary>
        /// <param name="success"></param>
        public static void LoadDataTable(bool success)
        {
            var (assembly, types) = GetAssemblyAndTypes();
            if (types == null || types.Length == 0) return;
            foreach (var type in types)
            {
                try
                {
                    var table = AssetManager.Load<ScriptableObject>("DataTable/" + type.Name);
                    var dataType = GetDataType(assembly, type);
                    var keyInfo = GetKeyField<KeyFieldAttribute>(dataType);
                    if (keyInfo == null)
                    {
                        Debug.Log($"{nameof(DataManager).Sky()} 缺少主键 => {type.Name.Red()}");
                        return;
                    }

                    var keyType = keyInfo.FieldType;
                    if (keyType == typeof(int))
                    {
                        DataManager<int>.Add(dataType, keyInfo, (IDataTable)table);
                    }
                    else if (keyType == typeof(string))
                    {
                        DataManager<string>.Add(dataType, keyInfo, (IDataTable)table);
                    }
                    else if (keyType.IsEnum)
                    {
                        DataManager<Enum>.Add(dataType, keyInfo, (IDataTable)table);
                    }
                }
                catch (Exception)
                {
                    Debug.Log($"{nameof(DataManager).Sky()} 加载 => {type.Name.Red()} 数据失败");
                }
            }
        }

        /// <summary>
        /// 获取主键为int的数据
        /// </summary>
        /// <param name="key">传入的int主键</param>
        /// <typeparam name="T">可以使用所有继承IData类型的对象</typeparam>
        /// <returns>返回一个数据对象</returns>
        public static T Get<T>(int key) where T : IData => DataManager<int>.Get<T>(key);

        /// <summary>
        /// 获取主键为string的数据
        /// </summary>
        /// <param name="key">传入的string主键</param>
        /// <typeparam name="T">要获取数据的类型,必须继承自JFramework.Data</typeparam>
        /// <returns>返回一个数据对象</returns>
        public static T Get<T>(string key) where T : IData => DataManager<string>.Get<T>(key);

        /// <summary>
        /// 获取主键为Enum的数据
        /// </summary>
        /// <param name="key">传入的string主键</param>
        /// <typeparam name="T">要获取数据的类型,必须继承自JFramework.Data</typeparam>
        /// <returns>返回一个数据对象</returns>
        public static T Get<T>(Enum key) where T : IData => DataManager<Enum>.Get<T>(key);

        /// <summary>
        /// 通过数据管理器得到数据表
        /// </summary>
        /// <returns>返回一个Data的列表</returns>
        public static T[] GetTable<T>() where T : IData
        {
            T[] data = DataManager<int>.GetTable<T>();
            if (data != null) return data;
            data = DataManager<Enum>.GetTable<T>();
            if (data != null) return data;
            data = DataManager<string>.GetTable<T>();
            if (data != null) return data;
            Log.Info(DebugOption.Data, $"获取 => {typeof(T).Name.Red()} 列表失败");
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
                var types = assembly.GetTypes();
                types = Array.FindAll(types, type => typeof(IDataTable).IsAssignableFrom(type) && !type.IsAbstract);
                if (types.Length > 0)
                {
                    return (assembly, types);
                }
            }

            return (null, null);
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
        private static Type GetDataType(Assembly assembly, Type type)
        {
            return assembly.GetType("JFramework.Table." + type.Name[..^5]);
        }

        /// <summary>
        /// 清除数据管理器
        /// </summary>
        internal static void Destroy()
        {
            DataManager<int>.Clear();
            DataManager<string>.Clear();
            DataManager<Enum>.Clear();
            AssetManager.OnLoadComplete -= LoadDataTable;
        }
    }
}