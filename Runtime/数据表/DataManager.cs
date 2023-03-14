using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace JFramework.Core
{
    using IntDataDict = Dictionary<int, IData>;
    using StrDataDict = Dictionary<string, IData>;

    /// <summary>
    /// 数据管理器
    /// </summary>
    public sealed class DataManager : Singleton<DataManager>
    {
        /// <summary>
        /// 存储int为主键类型的数据字典
        /// </summary>
        internal Dictionary<Type, IntDataDict> IntDataDict;

        /// <summary>
        /// 存储string为主键的数据字典
        /// </summary>
        internal Dictionary<Type, StrDataDict> StrDataDict;

        /// <summary>
        /// 资源路径
        /// </summary>
        private const string AssetPath = "DataTable/";

        /// <summary>
        /// 命名空间名称
        /// </summary>
        private const string Namespace = "JFramework.Table";

        /// <summary>
        /// 构造函数初始化数据
        /// </summary>
        internal override void Awake()
        {
            base.Awake();
            IntDataDict = new Dictionary<Type, IntDataDict>();
            StrDataDict = new Dictionary<Type, StrDataDict>();
            var assembly = GetAssembly(out IEnumerable<Type> types);
            foreach (var type in types)
            {
                try
                {
                    var keyName = type.Name;
                    AssetManager.Instance.LoadAsync<ScriptableObject>(AssetPath + keyName, obj =>
                    {
                        var dataTable = (IDataTable)obj;
                        var keyData = GetKeyField(assembly, type);
                        var keyField = GetKeyField(keyData);

                        if (keyData == null)
                        {
                            Debug.Log($"{Name.Sky()} <= Missing => {keyName.Red()} Key");
                            return;
                        }

                        var keyType = keyField.FieldType;
                        if (keyType == typeof(int))
                        {
                            IntDataDict.Add(keyData, Add<int>(keyField, dataTable));
                        }
                        else if (keyType == typeof(string))
                        {
                            StrDataDict.Add(keyData, Add<string>(keyField, dataTable));
                        }
                        else if (keyType.IsEnum)
                        {
                            var dataDict = new StrDataDict();
                            for (var i = 0; i < dataTable.Count; ++i)
                            {
                                var data = (IData)dataTable.GetData(i);
                                var key  = keyField.GetValue(data);
                                dataDict.Add(key.ToString(), data);
                            }

                            StrDataDict.Add(keyData, dataDict);
                        }
                        
                        if (DebugManager.IsDebugData)
                        {
                            Debug.Log($"{Name.Sky()} <= Success => {type.Name.Blue()}");
                        }
                    });
                }
                catch
                {
                    Debug.Log($"{Name.Sky()} <= Failure => {type.Name.Red()}");
                }
            }
        }

        /// <summary>
        /// 增加字典数据
        /// </summary>
        /// <param name="field"></param>
        /// <param name="table"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>返回数据字典</returns>
        private Dictionary<T, IData> Add<T>(FieldInfo field, IDataTable table)
        {
            var dataDict = new Dictionary<T, IData>();
            for (var i = 0; i < table.Count; ++i)
            {
                var data = (IData)table.GetData(i);
                var key  = (T)field.GetValue(data);
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
        public T Get<T>(int key) where T : IData
        {
            IntDataDict.TryGetValue(typeof(T), out IntDataDict soDict);
            if (soDict == null) return default;
            soDict.TryGetValue(key, out IData data);
            if (DebugManager.IsDebugData)
            {
                Debug.Log($"{Name.Sky()} <= Get => {typeof(T).Name.Blue()}");
            }
            return (T)data;
        }

        /// <summary>
        /// 获取对应类型数据下主键为Key的数据
        /// </summary>
        /// <param name="key">传入的string主键</param>
        /// <typeparam name="T">要获取数据的类型,必须继承自JFramework.Data</typeparam>
        /// <returns>返回一个数据对象</returns>
        public T Get<T>(string key) where T : IData
        {
            StrDataDict.TryGetValue(typeof(T), out StrDataDict soDict);
            if (soDict == null) return default;
            soDict.TryGetValue(key, out IData data);
            if (DebugManager.IsDebugData)
            {
                Debug.Log($"{Name.Sky()} <= Get => {typeof(T).Name.Blue()}");
            }
            return (T)data;
        }
        
        /// <summary>
        /// 获取对应类型数据下主键为Key的数据
        /// </summary>
        /// <param name="key">传入的string主键</param>
        /// <typeparam name="T">要获取数据的类型,必须继承自JFramework.Data</typeparam>
        /// <returns>返回一个数据对象</returns>
        public T Get<T>(Enum key) where T : IData
        {
            StrDataDict.TryGetValue(typeof(T), out StrDataDict soDict);
            if (soDict == null) return default;
            soDict.TryGetValue(key.ToString(), out IData data);
            if (DebugManager.IsDebugData)
            {
                Debug.Log($"{Name.Sky()} <= Get => {typeof(T).Name.Blue()}");
            }
            return (T)data;
        }
        

        /// <summary>
        /// 通过数据管理器得到数据表
        /// </summary>
        /// <typeparam name="T">可以使用所有继承Data类型的对象</typeparam>
        /// <returns>返回泛型列表</returns>
        public List<T> GetTable<T>() where T : IData
        {
            var table = GetTable(typeof(T));
            if (table == null) return null;
            var dataList = new List<T>();
            table.ForEach(data => dataList.Add((T)data));
            return dataList;
        }

        /// <summary>
        /// 通过数据管理器得到数据表
        /// </summary>
        /// <param name="type">传入的类型</param>
        /// <returns>返回一个Data的列表</returns>
        private List<IData> GetTable(Type type)
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
        private Assembly GetAssembly(out IEnumerable<Type> types)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                types = assembly.GetTypes().Where(_ => _.GetInterfaces().Contains(typeof(IDataTable)));
                var tableTypes = types as Type[] ?? types.ToArray();
                if (tableTypes.Any())
                {
                    return assembly;
                }
            }

            types = null;
            return null;
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
        private Type GetKeyField(Assembly assembly, Type type)
        {
            var name = type.Name;
            name = name.Substring(0, name.Length - 5);
            return assembly.GetType(Namespace + "." + name);
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
    }
}