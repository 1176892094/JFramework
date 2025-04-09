// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2025-01-08 17:01:43
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace JFramework.Common
{
    public static class DataManager
    {
        [Serializable]
        private struct Name
        {
            public string name;
        }
        
        public static async void LoadDataTable()
        {
            if (!GlobalManager.Instance) return;
            var fileName = Resources.LoadAll<TextAsset>(nameof(GlobalSetting))[0].text;
            var assembly = Service.Find.Assembly(JsonUtility.FromJson<Name>(fileName).name);
            if (assembly == null)
            {
                return;
            }

            var assetNames = new List<string>();
            foreach (var assetType in assembly.GetTypes())
            {
                if (typeof(IDataTable).IsAssignableFrom(assetType))
                {
                    assetNames.Add(assetType.FullName);
                }
            }

            if (assetNames.Count == 0)
            {
                return;
            }

            EventManager.Invoke(new DataAwake(assetNames.ToArray()));
            foreach (var assetName in assetNames)
            {
                var shortName = assetName.Substring(assetName.LastIndexOf('.') + 1);
                try
                {
                    var assetData = (IDataTable)await AssetManager.Load<ScriptableObject>(GlobalSetting.GetTablePath(shortName));
                    var assetType = assembly.GetType(assetName.Substring(0, assetName.Length - 5));
                    var properties = assetType.GetProperties(Service.Find.Entity);
                    foreach (var property in properties)
                    {
                        if (property.GetCustomAttribute(typeof(PrimaryAttribute)) == null)
                        {
                            continue;
                        }

                        if (property.PropertyType == typeof(int))
                        {
                            GlobalManager.itemTable.Add(assetType, LoadData<int>());
                        }
                        else if (property.PropertyType == typeof(string))
                        {
                            GlobalManager.nameTable.Add(assetType, LoadData<string>());
                        }
                        else if (property.PropertyType.IsEnum)
                        {
                            GlobalManager.enumTable.Add(assetType, LoadData<Enum>());
                        }

                        continue;

                        Dictionary<T, IData> LoadData<T>()
                        {
                            var items = new Dictionary<T, IData>();
                            for (var i = 0; i < assetData.Count; i++)
                            {
                                var data = assetData.GetData(i);
                                var item = (T)property.GetValue(data);
                                if (!items.ContainsKey(item))
                                {
                                    items.Add(item, data);
                                    continue;
                                }

                                Debug.LogWarning(Service.Text.Format("加载数据 {0} 失败。键值重复: {1}", shortName, item));
                            }

                            return items;
                        }
                    }

                    EventManager.Invoke(new DataUpdate(shortName));
                }
                catch (Exception e)
                {
                    Debug.LogError(Service.Text.Format("加载 {0} 数据失败!\n{1}", shortName, e));
                }
            }

            EventManager.Invoke(new DataComplete());
        }

        public static T Get<T>(int key) where T : IData
        {
            if (!GlobalManager.Instance) return default;
            if (!GlobalManager.itemTable.TryGetValue(typeof(T), out var dataTable))
            {
                return default;
            }

            if (!dataTable.TryGetValue(key, out var data))
            {
                return default;
            }

            return (T)data;
        }

        public static T Get<T>(string key) where T : IData
        {
            if (!GlobalManager.Instance) return default;
            if (!GlobalManager.nameTable.TryGetValue(typeof(T), out var dataTable))
            {
                return default;
            }

            if (!dataTable.TryGetValue(key, out var data))
            {
                return default;
            }

            return (T)data;
        }

        public static T Get<T>(Enum key) where T : IData
        {
            if (!GlobalManager.Instance) return default;
            if (!GlobalManager.enumTable.TryGetValue(typeof(T), out var dataTable))
            {
                return default;
            }

            if (!dataTable.TryGetValue(key, out var data))
            {
                return default;
            }

            return (T)data;
        }

        public static List<T> GetTable<T>() where T : IData
        {
            if (!GlobalManager.Instance) return null;
            if (GlobalManager.itemTable.TryGetValue(typeof(T), out var itemTable))
            {
                var caches = new List<T>();
                foreach (T data in itemTable.Values)
                {
                    caches.Add(data);
                }

                return caches;
            }

            if (GlobalManager.nameTable.TryGetValue(typeof(T), out var nameTable))
            {
                var caches = new List<T>();
                foreach (T data in nameTable.Values)
                {
                    caches.Add(data);
                }

                return caches;
            }

            if (GlobalManager.enumTable.TryGetValue(typeof(T), out var enumTable))
            {
                var caches = new List<T>();
                foreach (T data in enumTable.Values)
                {
                    caches.Add(data);
                }

                return caches;
            }

            Debug.LogError(Service.Text.Format("获取 {0} 失败!", typeof(T).Name));
            return null;
        }

        internal static void Dispose()
        {
            var itemTable = new List<Type>(GlobalManager.itemTable.Keys);
            foreach (var data in itemTable)
            {
                if (GlobalManager.itemTable.TryGetValue(data, out var pool))
                {
                    pool.Clear();
                    GlobalManager.itemTable.Remove(data);
                }
            }

            GlobalManager.itemTable.Clear();

            var enumTable = new List<Type>(GlobalManager.enumTable.Keys);
            foreach (var data in enumTable)
            {
                if (GlobalManager.enumTable.TryGetValue(data, out var pool))
                {
                    pool.Clear();
                    GlobalManager.enumTable.Remove(data);
                }
            }

            GlobalManager.enumTable.Clear();

            var nameTable = new List<Type>(GlobalManager.nameTable.Keys);
            foreach (var data in nameTable)
            {
                if (GlobalManager.nameTable.TryGetValue(data, out var pool))
                {
                    pool.Clear();
                    GlobalManager.nameTable.Remove(data);
                }
            }

            GlobalManager.nameTable.Clear();
        }
    }
}