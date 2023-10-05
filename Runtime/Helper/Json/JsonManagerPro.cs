using System;
using System.Collections.Generic;
using System.IO;
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
        public static void Encrypt(object obj, string name)
        {
            var filePath = GetPath(name);
            var saveJson = obj is ScriptableObject ? JsonUtility.ToJson(obj) : JsonConvert.SerializeObject(obj);
            File.WriteAllBytesAsync(filePath, Encrypt(saveJson, name));
        }

        /// <summary>
        /// 加载解密数据
        /// </summary>
        /// <param name="obj">加载的数据</param>
        public static void Decrypt(ScriptableObject obj)
        {
            var filePath = GetPath(obj.name);
            secrets ??= Load<Dictionary<string, JsonData>>(nameof(JsonManager));
            if (!File.Exists(filePath))
            {
                Debug.Log($"创建 {obj.name.Orange()} 数据文件");
                Encrypt(obj, obj.name);
            }

            secrets.TryAdd(obj.name, new JsonData());
            if (!secrets[obj.name]) return;
            var saveJson = Decrypt(File.ReadAllBytes(filePath), obj.name);
            if (saveJson.IsEmpty()) return;
            JsonUtility.FromJsonOverwrite(saveJson, obj);
        }


        /// <summary>
        /// 加载解密数据
        /// </summary>
        /// <param name="name">加载的数据名称</param>
        /// <typeparam name="T">可以使用任何类型</typeparam>
        /// <returns>返回解密的数据</returns>
        public static T Decrypt<T>(string name) where T : new()
        {
            var filePath = GetPath(name);
            secrets ??= Load<Dictionary<string, JsonData>>(nameof(JsonManager));
            if (!File.Exists(filePath))
            {
                Debug.Log($"创建 {name.Orange()} 数据文件");
                Encrypt(new T(), name);
            }

            try
            {
                secrets.TryAdd(name, new JsonData());
                if (!secrets[name]) return new T();
                var saveJson = Decrypt(File.ReadAllBytes(filePath), name);
                return !saveJson.IsEmpty() ? JsonConvert.DeserializeObject<T>(saveJson) : default;
            }
            catch (Exception)
            {
                secrets[name] = new JsonData();
                Save(secrets, nameof(JsonManager));
                return new T();
            }
        }
    }
}