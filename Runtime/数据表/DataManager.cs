using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirenix.Utilities;
using UnityEngine;

namespace JFramework.Core
{
    using IntDataDict = Dictionary<int, IData>;
    using StrDataDict = Dictionary<string, IData>;

    /// <summary>
    /// 数据管理器
    /// </summary>
    public class DataManager : Singleton<DataManager>
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
        /// 数据表名称
        /// </summary>
        private const string Table = "Table";

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
        public override void Awake()
        {
            base.Awake();
            IntDataDict = new Dictionary<Type, IntDataDict>();
            StrDataDict = new Dictionary<Type, StrDataDict>();
            var assembly = GetAssembly();
            var types = assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(DataTable)));
            foreach (var type in types)
            {
                try
                {
                    var keyName = type.Name;
                    AssetManager.Instance.LoadAsync<DataTable>(AssetPath + keyName, table =>
                    {
                        table.InitData();
                        var classType = GetClassType(assembly, type);
                        var keyField = KeyValue(classType);

                        if (keyField == null)
                        {
                            Debug.LogError($"DataManager没有找到主键:{keyName}!");
                            return;
                        }

                        var keyType = keyField.FieldType;

                        if (keyType == typeof(int))
                        {
                            var dataDict = new IntDataDict();
                            for (var i = 0; i < table.Count; ++i)
                            {
                                var data = table.GetData(i);
                                int key = (int)keyField.GetValue(data);
                                dataDict.Add(key, data);
                            }

                            IntDataDict.Add(classType, dataDict);
                        }
                        else if (keyType == typeof(string))
                        {
                            var dataDict = new StrDataDict();
                            for (var i = 0; i < table.Count; ++i)
                            {
                                var data = table.GetData(i);
                                string key = (string)keyField.GetValue(data);
                                dataDict.Add(key, data);
                            }

                            StrDataDict.Add(classType, dataDict);
                        }
                        else
                        {
                            Debug.LogError($"DataManager加载{type.Name}失败.这不是有效的主键!");
                        }
                    });
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }
            }

            Debug.Log($"DataManager加载资源完成!");
        }

        /// <summary>
        /// 得到数据表对象的类型
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="classType"></param>
        /// <returns></returns>
        private Type GetClassType(Assembly assembly, Type classType)
        {
            var className = GetClassName(classType);
            var type = assembly.GetType(Namespace + "." + className);
            return type;
        }

        /// <summary>
        /// 得到数据表的类名
        /// </summary>
        /// <param name="classType">传入类对象类型</param>
        /// <returns></returns>
        private string GetClassName(Type classType)
        {
            var fullName = classType.Name;
            var parts = fullName.Split('.');
            var lastPart = parts[^1];
            lastPart = lastPart.Substring(lastPart.IndexOf('_') + 1);
            var length = lastPart.IndexOf(Table, StringComparison.Ordinal);
            return string.IsNullOrEmpty(Table) ? lastPart : lastPart.Substring(0, length);
        }

        /// <summary>
        /// 获取当前程序集中的数据表对象类
        /// </summary>
        /// <returns></returns>
        private Assembly GetAssembly()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(DataTable)));
                var collectionTypes = types as Type[] ?? types.ToArray();
                if (collectionTypes.Any()) return assembly;
            }

            return typeof(DataTable).Assembly;
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
            return (T)data;
        }
        
        /// <summary>
        /// 通过数据管理器得到数据表
        /// </summary>
        /// <typeparam name="T">可以使用所有继承Data类型的对象</typeparam>
        /// <returns>返回泛型列表</returns>
        public List<T> GetTable<T>() where T : IData
        {
            IntDataDict.TryGetValue(typeof(T), out IntDataDict dictInt);
            if (dictInt != null)
            {
                List<T> list = new List<T>();
                dictInt.Values.ForEach(data => list.Add((T)data));
                return list;
            }

            StrDataDict.TryGetValue(typeof(T), out StrDataDict dictStr);
            if (dictStr != null)
            {
                List<T> list = new List<T>();
                dictStr.Values.ForEach(data => list.Add((T)data));
                return list;
            }

            return null;
        }

        /// <summary>
        /// 通过数据管理器得到数据表
        /// </summary>
        /// <param name="type">传入的类型</param>
        /// <returns>返回一个Data的列表</returns>
        public List<IData> GetTable(Type type)
        {
            IntDataDict.TryGetValue(type, out IntDataDict dictInt);
            if (dictInt != null) return dictInt.Values.ToList();
            StrDataDict.TryGetValue(type, out StrDataDict dictStr);
            if (dictStr != null) return dictStr.Values.ToList();
            return null;
        }

        /// <summary>
        /// 获取数据的主键
        /// </summary>
        /// <param name="type">传入的数据类型</param>
        /// <returns>返回字段的信息</returns>
        private static FieldInfo KeyValue(Type type)
        {
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var key = (from fieldInfo in fields
                let attrs = fieldInfo.GetCustomAttributes(typeof(KeyAttribute), false)
                where attrs.Length > 0
                select fieldInfo).FirstOrDefault();
            return key;
        }
        
        /// <summary>
        /// 获取数据的键值
        /// </summary>
        /// <returns>返回数据的主键</returns>
        public static object KeyValue(IData data)
        {
            var key = KeyValue(data.GetType());
            return key == null ? null : key.GetValue(data);
        }
    }
}