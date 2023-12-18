// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-10-25  00:06
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Threading.Tasks;
using JFramework.Interface;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace JFramework.Core
{
    /// <summary>
    /// 数据管理器
    /// </summary>
    public static class DataManager
    {
        /// <summary>
        /// 加载数据表
        /// </summary>
        public static async Task LoadDataTable()
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
                    var obj = await AssetManager.Load<ScriptableObject>(GlobalSetting.GetTablePath(type.Name));
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
                        Data<Enum>.Add(data, field, table);
                    }
                    else if (field.FieldType == typeof(int))
                    {
                        Data<int>.Add(data, field, table);
                    }
                    else if (field.FieldType == typeof(string))
                    {
                        Data<string>.Add(data, field, table);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"加载 {type.Name.Red()} 数据失败\n{e}");
                }
            }
        }

        /// <summary>
        /// 获取主键为int的数据
        /// </summary>
        /// <param name="key">传入的int主键</param>
        /// <typeparam name="T">可以使用所有继承IData类型的对象</typeparam>
        /// <returns>返回一个数据对象</returns>
        public static T Get<T>(int key) where T : IData => Data<int>.Get<T>(key);

        /// <summary>
        /// 获取主键为Enum的数据
        /// </summary>
        /// <param name="key">传入的string主键</param>
        /// <typeparam name="T">要获取数据的类型,必须继承自JFramework.Data</typeparam>
        /// <returns>返回一个数据对象</returns>
        public static T Get<T>(Enum key) where T : IData => Data<Enum>.Get<T>(key);

        /// <summary>
        /// 获取主键为string的数据
        /// </summary>
        /// <param name="key">传入的string主键</param>
        /// <typeparam name="T">要获取数据的类型,必须继承自JFramework.Data</typeparam>
        /// <returns>返回一个数据对象</returns>
        public static T Get<T>(string key) where T : IData => Data<string>.Get<T>(key);

        /// <summary>
        /// 通过数据管理器得到数据表
        /// </summary>
        /// <returns>返回一个Data的列表</returns>
        public static T[] GetTable<T>() where T : IData
        {
            var data = Data<int>.GetTable<T>();
            if (data != null) return data;
            data = Data<Enum>.GetTable<T>();
            if (data != null) return data;
            data = Data<string>.GetTable<T>();
            if (data != null) return data;
            Debug.LogWarning($"获取 {typeof(T).Name.Red()} 失败！");
            return default;
        }

        /// <summary>
        /// 清除数据管理器
        /// </summary>
        internal static void Dispose()
        {
            Data<int>.Clear();
            Data<Enum>.Clear();
            Data<string>.Clear();
        }
    }
}