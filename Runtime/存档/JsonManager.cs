using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

// ReSharper disable All
namespace JFramework.Core
{
    /// <summary>
    /// Json管理器
    /// </summary>
    public static partial class JsonManager
    {
        private static Dictionary<string, JsonData> jsonDict = new Dictionary<string, JsonData>();

        /// <summary>
        /// 静态构造函数(第一次使用时加载密钥)
        /// </summary>
        static JsonManager() => jsonDict = Load<Dictionary<string, JsonData>>(nameof(JsonManager));

        /// <summary>
        /// 存储数据
        /// </summary>
        /// <param name="obj">保存的数据</param>
        /// <param name="name">保存的名称</param>
        public static void Save(object obj, string name)
        {
            var filePath = GetPath(name);
            var saveJson = obj is ScriptableObject ? JsonUtility.ToJson(obj) : JsonConvert.SerializeObject(obj);
            File.WriteAllText(filePath, saveJson);
            Log.Info(DebugOption.Json, $"保存 => {name.Orange()} 数据文件");
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="obj">加载的数据</param>
        public static void Load(ScriptableObject obj)
        {
            var filePath = GetPath(obj.name);
            if (!File.Exists(filePath))
            {
                Debug.LogWarning($"{nameof(JsonManager).Sky()} 创建 => {obj.name.Orange()} 数据文件");
                Save(obj, obj.name);
            }

            Log.Info(DebugOption.Json, $"读取 => {obj.name.Orange()} 数据文件");
            var saveJson = File.ReadAllText(filePath);
            if (saveJson.IsEmpty()) return;
            JsonUtility.FromJsonOverwrite(saveJson, obj);
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="name">加载的数据名称</param>
        /// <typeparam name="T">可以使用任何类型</typeparam>
        /// <returns>返回加载的数据</returns>
        public static T Load<T>(string name) where T : new()
        {
            var filePath = GetPath(name);
            if (!File.Exists(filePath))
            {
                Debug.LogWarning($"{nameof(JsonManager).Sky()} 创建 => {name.Orange()} 数据文件");
                Save(new T(), name);
            }

            Log.Info(DebugOption.Json, $"读取 => {name.Orange()} 数据文件");
            var saveJson = File.ReadAllText(filePath);
            return !saveJson.IsEmpty() ? JsonConvert.DeserializeObject<T>(saveJson) : default;
        }

        /// <summary>
        /// 清空管理器
        /// </summary>
        public static void Clear()
        {
            jsonDict.Clear();
            Save(jsonDict, nameof(JsonManager));
        }

        /// <summary>
        /// Json管理器设置加密数据
        /// </summary>
        /// <param name="id">加密数据的id</param>
        /// <param name="key">加密数据的键值</param>
        /// <param name="iv">加密数据的向量</param>
        private static void SetData(string id, byte[] key, byte[] iv)
        {
            if (!jsonDict.ContainsKey(id))
            {
                jsonDict.Add(id, new JsonData());
            }

            jsonDict[id].key = key;
            jsonDict[id].iv = iv;
            Save(jsonDict, nameof(JsonManager));
        }

        /// <summary>
        /// Json管理器得到加密数据
        /// </summary>
        /// <param name="id">加密数据的id</param>
        /// <returns>返回得到的加密数据</returns>
        private static JsonData GetData(string id)
        {
            if (!jsonDict.ContainsKey(id))
            {
                jsonDict.Add(id, new JsonData());
            }

            return jsonDict[id];
        }

        /// <summary>
        /// Json管理器获取路径
        /// </summary>
        /// <param name="name">传入的路径名称</param>
        /// <returns>返回的到的路径</returns>
        private static string GetPath(string name)
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, $"{name}.json");
            if (!File.Exists(filePath))
            {
                filePath = Path.Combine(Application.persistentDataPath, $"{name}.json");
            }

            return filePath;
        }
    }
}