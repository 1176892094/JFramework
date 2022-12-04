using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JFramework.Basic;
using JFramework.Excel;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace JFramework
{
    using IntDataDict = Dictionary<int, ExcelData>;
    using StrDataDict = Dictionary<string, ExcelData>;

    public class ExcelManager : SingletonMono<ExcelManager>
    {
        private readonly Dictionary<Type, IntDataDict> IntDataDict = new Dictionary<Type, IntDataDict>();
        private readonly Dictionary<Type, StrDataDict> StrDataDict = new Dictionary<Type, StrDataDict>();

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
            InitData();
        }

        private void InitData()
        {
#if UNITY_EDITOR
            if (!ExcelSetting.Instance.AssetPath.Contains("/Resources"))
            {
                EditorUtility.DisplayDialog("JFramework Tool", "SO文件必须在Resources目录下", "OK");
                return;
            }
#endif
            IntDataDict.Clear();
            StrDataDict.Clear();

            var assembly = ExcelUtility.GetSourceAssembly();
            var types = assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(ExcelContainer)));
            foreach (var containerType in types)
            {
                LoadData(assembly, containerType);
            }

            Debugger.Log($"ExcelManager加载{IntDataDict.Count + StrDataDict.Count}个数据");
        }

        private void LoadData(Assembly assembly, Type dataCollectionType)
        {
            try
            {
                var tableName = dataCollectionType.Name;
                var collection = LoadContainer(tableName);
                if (collection == null)
                {
                    Debugger.LogError($"ExcelManager加载数据失败:{tableName}!");
                    return;
                }

                collection.InitData();
                var rowDataType = GetClassType(assembly, dataCollectionType);
                var keyField = ExcelUtility.GetRowDataKeyField(rowDataType);
                if (keyField == null)
                {
                    Debugger.LogError($"ExcelManager没有找到主键:{tableName}!");
                    return;
                }

                var keyType = keyField.FieldType;
                if (keyType == typeof(int))
                {
                    var dataDict = new IntDataDict();
                    for (var i = 0; i < collection.GetCount(); ++i)
                    {
                        var data = collection.GetData(i);
                        int key = (int)keyField.GetValue(data);
                        dataDict.Add(key, data);
                    }

                    IntDataDict.Add(rowDataType, dataDict);
                }
                else if (keyType == typeof(string))
                {
                    var dataDict = new StrDataDict();
                    for (var i = 0; i < collection.GetCount(); ++i)
                    {
                        var data = collection.GetData(i);
                        string key = (string)keyField.GetValue(data);
                        dataDict.Add(key, data);
                    }

                    StrDataDict.Add(rowDataType, dataDict);
                }
                else
                {
                    Debugger.LogError($"ExcelManager加载{dataCollectionType.Name}失败.这不是有效的主键!");
                }
            }
            catch (Exception e)
            {
                Debugger.LogError(e.ToString());
            }
        }

        public T Get<T>(int key) where T : ExcelData
        {
            return (T)Get(key, typeof(T));
        }

        public T Get<T>(string key) where T : ExcelData
        {
            return (T)Get(key, typeof(T));
        }

        public List<T> GetList<T>() where T : ExcelData
        {
            IntDataDict.TryGetValue(typeof(T), out IntDataDict dictInt);
            if (dictInt != null)
            {
                List<T> list = new List<T>();
                foreach (var data in dictInt)
                {
                    list.Add((T)data.Value);
                }

                return list;
            }

            StrDataDict.TryGetValue(typeof(T), out StrDataDict dictStr);
            if (dictStr != null)
            {
                List<T> list = new List<T>();
                foreach (var data in dictStr)
                {
                    list.Add((T)data.Value);
                }

                return list;
            }

            return null;
        }

        public List<ExcelData> GetList(Type type)
        {
            IntDataDict.TryGetValue(type, out IntDataDict dictInt);
            if (dictInt != null) return dictInt.Values.ToList();
            StrDataDict.TryGetValue(type, out StrDataDict dictStr);
            if (dictStr != null) return dictStr.Values.ToList();
            return null;
        }
        
        private ExcelData Get(int key, Type type)
        {
            IntDataDict.TryGetValue(type, out IntDataDict soDic);
            if (soDic == null) return null;
            soDic.TryGetValue(key, out ExcelData data);
            return data;
        }

        private ExcelData Get(string key, Type type)
        {
            StrDataDict.TryGetValue(type, out StrDataDict soDic);
            if (soDic == null) return null;
            soDic.TryGetValue(key, out ExcelData data);
            return data;
        }
        
        private Type GetClassType(Assembly assembly, Type sheetClassType)
        {
            var sheetName = GetSheetName(sheetClassType);
            var type = assembly.GetType(ExcelSetting.Instance.GetClassName(sheetName, true));
            return type;
        }

        private string GetSheetName(Type sheetClassType)
        {
            return ExcelSetting.Instance.GetSheetName(sheetClassType);
        }

        private ExcelContainer LoadContainer(string name)
        {
            int index = ExcelSetting.Instance.AssetPath.IndexOf("Resources/", StringComparison.Ordinal);
            string filePath = ExcelSetting.Instance.AssetPath.Substring(index + "Resources/".Length) + name;
            ExcelContainer table = ResourceManager.Load<ExcelContainer>(filePath);
            return table;
        }
    }
}