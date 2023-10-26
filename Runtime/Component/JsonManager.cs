// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-10-25  00:02
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
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
        /// <summary>
        /// 存储加密密钥的字典
        /// </summary>
        private static Dictionary<string, JsonData> secrets;

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
        }

        /// <summary>
        /// 存储加密数据
        /// </summary>
        /// <param name="obj">保存的数据</param>
        /// <param name="name">保存的名称</param>
        public static void Encrypt(object obj, string name)
        {
            var filePath = GetPath(name);
            var saveJson = obj is ScriptableObject ? JsonUtility.ToJson(obj) : JsonConvert.SerializeObject(obj);
            File.WriteAllBytes(filePath, Encrypt(saveJson, name));
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
                Debug.Log($"创建 {name.Orange()} 数据文件");
                Save(new T(), name);
            }

            try
            {
                var saveJson = File.ReadAllText(filePath);
                return !saveJson.IsEmpty() ? JsonConvert.DeserializeObject<T>(saveJson) : new T();
            }
            catch (Exception)
            {
                return new T();
            }
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
                return !saveJson.IsEmpty() ? JsonConvert.DeserializeObject<T>(saveJson) : new T();
            }
            catch (Exception)
            {
                secrets[name] = new JsonData();
                Save(secrets, nameof(JsonManager));
                return new T();
            }
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="obj">加载的数据</param>
        internal static void Load(ScriptableObject obj)
        {
            var filePath = GetPath(obj.name);
            if (!File.Exists(filePath))
            {
                Debug.Log($"创建 {obj.name.Orange()} 数据文件");
                Save(obj, obj.name);
            }

            var saveJson = File.ReadAllText(filePath);
            if (saveJson.IsEmpty()) return;
            JsonUtility.FromJsonOverwrite(saveJson, obj);
        }

        /// <summary>
        /// 加载解密数据
        /// </summary>
        /// <param name="obj">加载的数据</param>
        internal static void Decrypt(ScriptableObject obj)
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
        /// 清空管理器
        /// </summary>
        internal static void Clear()
        {
            secrets = null;
        }
    }
}