using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
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
        /// <summary>
        /// 存储加密密钥的字典
        /// </summary>
        private static Dictionary<string, JsonData> secrets;

        /// <summary>
        /// 存储数据
        /// </summary>
        /// <param name="obj">保存的数据</param>
        /// <param name="name">保存的名称</param>
        public static async Task Save(object obj, string name)
        {
            var filePath = GetPath(name);
            var saveJson = obj is ScriptableObject ? JsonUtility.ToJson(obj) : JsonConvert.SerializeObject(obj);
            await File.WriteAllTextAsync(filePath, saveJson);
            Log.Info(DebugOption.Json, $"保存 => {name.Orange()} 数据文件");
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="obj">加载的数据</param>
        public static async Task Load(ScriptableObject obj)
        {
            var filePath = GetPath(obj.name);
            if (!File.Exists(filePath))
            {
                Debug.Log($"{nameof(JsonManager).Sky()} 创建 => {obj.name.Orange()} 数据文件");
                await Save(obj, obj.name);
            }

            Log.Info(DebugOption.Json, $"读取 => {obj.name.Orange()} 数据文件");
            var saveJson = await File.ReadAllTextAsync(filePath);
            if (saveJson.IsEmpty()) return;
            JsonUtility.FromJsonOverwrite(saveJson, obj);
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="name">加载的数据名称</param>
        /// <typeparam name="T">可以使用任何类型</typeparam>
        /// <returns>返回加载的数据</returns>
        public static async Task<T> Load<T>(string name) where T : new()
        {
            var filePath = GetPath(name);
            if (!File.Exists(filePath))
            {
                Debug.Log($"{nameof(JsonManager).Sky()} 创建 => {name.Orange()} 数据文件");
                await Save(new T(), name);
            }

            Log.Info(DebugOption.Json, $"读取 => {name.Orange()} 数据文件");
            var saveJson = await File.ReadAllTextAsync(filePath);
            try
            {
                return !saveJson.IsEmpty() ? JsonConvert.DeserializeObject<T>(saveJson) : default;
            }
            catch (Exception)
            {
                return new T();
            }
        }

        /// <summary>
        /// 清空管理器
        /// </summary>
        public static async void Clear()
        {
            secrets.Clear();
            await Save(secrets, nameof(JsonManager));
        }
    }
}