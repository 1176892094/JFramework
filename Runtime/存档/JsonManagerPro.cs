using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace JFramework.Core
{
    /// <summary>
    /// Json管理器
    /// </summary>
    public static partial class JsonManager
    {
        /// <summary>
        /// 存储加密数据
        /// </summary>
        /// <param name="obj">保存的数据</param>
        /// <param name="name">保存的名称</param>
        public static async Task Encrypt(object obj, string name)
        {
            var filePath = GetPath(name);
            var saveJson = obj is ScriptableObject ? JsonUtility.ToJson(obj) : JsonConvert.SerializeObject(obj);
            await File.WriteAllBytesAsync(filePath, await Encrypt(saveJson, name));
            Log.Info(Option.Json, $"保存加密 => {name.Orange()} 数据文件");
        }

        /// <summary>
        /// 加载解密数据
        /// </summary>
        /// <param name="obj">加载的数据</param>
        public static async Task Decrypt(ScriptableObject obj)
        {
            var filePath = GetPath(obj.name);
            secrets ??= await Load<Dictionary<string, JsonData>>(nameof(JsonManager));
            if (!File.Exists(filePath))
            {
                Debug.Log($"{nameof(JsonManager)} 创建 => {obj.name.Orange()} 数据文件");
                await Encrypt(obj, obj.name);
            }

            Log.Info(Option.Json, $"读取解密 => {obj.name.Orange()} 数据文件");
            secrets.TryAdd(obj.name, new JsonData());
            if (!secrets[obj.name]) return;
            var saveJson = await Decrypt(await File.ReadAllBytesAsync(filePath), obj.name);
            if (saveJson.IsEmpty()) return;
            JsonUtility.FromJsonOverwrite(saveJson, obj);
        }


        /// <summary>
        /// 加载解密数据
        /// </summary>
        /// <param name="name">加载的数据名称</param>
        /// <typeparam name="T">可以使用任何类型</typeparam>
        /// <returns>返回解密的数据</returns>
        public static async Task<T> Decrypt<T>(string name) where T : new()
        {
            var filePath = GetPath(name);
            secrets ??= await Load<Dictionary<string, JsonData>>(nameof(JsonManager));
            if (!File.Exists(filePath))
            {
                Debug.Log($"{nameof(JsonManager)} 创建 => {name.Orange()} 数据文件");
                await Encrypt(new T(), name);
            }

            Log.Info(Option.Json, $"读取解密 => {name.Orange()} 数据文件");
            secrets.TryAdd(name, new JsonData());
            if (!secrets[name]) return default;
            var saveJson = await Decrypt(await File.ReadAllBytesAsync(filePath), name);
            try
            {
                return !saveJson.IsEmpty() ? JsonConvert.DeserializeObject<T>(saveJson) : default;
            }
            catch (Exception)
            {
                secrets[name] = new JsonData();
                await Save(secrets, nameof(JsonManager));
                return new T();
            }
        }
    }
}