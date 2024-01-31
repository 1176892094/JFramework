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
using System.Linq;
using System.Security.Cryptography;
using Sirenix.OdinInspector;
using UnityEngine;

// ReSharper disable All

namespace JFramework.Core
{
    using JsonSetting = Variables<JsonData>;

    /// <summary>
    /// Json管理器
    /// </summary>
    public sealed class JsonManager : Controller<GlobalManager>
    {
        /// <summary>
        /// 存储加密密钥的字典
        /// </summary>
        [ShowInInspector] private Dictionary<string, JsonData> secrets = new Dictionary<string, JsonData>();

        /// <summary>
        /// 管理器初始化
        /// </summary>
        private void Awake()
        {
            secrets = Load<JsonSetting>(nameof(JsonManager)).value.ToDictionary(data => data.name);
        }

        /// <summary>
        /// 存储数据
        /// </summary>
        /// <param name="obj">保存的数据</param>
        /// <param name="name">保存的名称</param>
        public void Save<T>(T obj, string name) where T : new()
        {
            File.WriteAllText(FilePath(name), JsonUtility.ToJson(obj));
        }

        /// <summary>
        /// 存储加密数据
        /// </summary>
        /// <param name="obj">保存的数据</param>
        /// <param name="name">保存的名称</param>
        public void Encrypt<T>(T obj, string name) where T : new()
        {
            File.WriteAllBytes(FilePath(name), Encrypt(JsonUtility.ToJson(obj), name));
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="name">加载的数据名称</param>
        /// <typeparam name="T">可以使用任何类型</typeparam>
        /// <returns>返回加载的数据</returns>
        public T Load<T>(string name) where T : new()
        {
            var filePath = FilePath(name);
            if (!File.Exists(filePath))
            {
                Save(new T(), name);
                Debug.Log($"创建 {name.Orange()} 数据文件");
            }

            try
            {
                var saveJson = File.ReadAllText(filePath);
                return !saveJson.IsEmpty() ? JsonUtility.FromJson<T>(saveJson) : new T();
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
        public T Decrypt<T>(string name) where T : new()
        {
            var filePath = FilePath(name);
            if (!File.Exists(filePath))
            {
                Encrypt(new T(), name);
                Debug.Log($"创建 {name.Orange()} 数据文件");
            }

            if (!secrets.ContainsKey(name))
            {
                secrets[name] = new JsonData();
                Save(new JsonSetting(secrets.Values.ToList()), nameof(JsonManager));
                return new T();
            }

            try
            {
                var saveJson = Decrypt(File.ReadAllBytes(filePath), name);
                return !saveJson.IsEmpty() ? JsonUtility.FromJson<T>(saveJson) : new T();
            }
            catch (Exception)
            {
                secrets[name] = new JsonData();
                Save(new JsonSetting(secrets.Values.ToList()), nameof(JsonManager));
                return new T();
            }
        }

        /// <summary>
        /// 存储数据
        /// </summary>
        /// <param name="obj">保存的数据</param>
        public void Save(ScriptableObject obj)
        {
            if (obj.name.IsEmpty())
            {
                obj.name = obj.GetType().Name;
            }

            File.WriteAllText(FilePath(obj.name), JsonUtility.ToJson(obj));
        }

        /// <summary>
        /// 存储加密数据
        /// </summary>
        /// <param name="obj">保存的数据</param>
        public void Encrypt(ScriptableObject obj)
        {
            if (obj.name.IsEmpty())
            {
                obj.name = obj.GetType().Name;
            }

            File.WriteAllBytes(FilePath(obj.name), Encrypt(JsonUtility.ToJson(obj), name));
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="obj">加载的数据</param>
        public void Load(ScriptableObject obj)
        {
            if (obj.name.IsEmpty())
            {
                obj.name = obj.GetType().Name;
            }

            var filePath = FilePath(obj.name);
            if (!File.Exists(filePath))
            {
                Debug.Log($"创建 {obj.name.Orange()} 数据文件");
                Save(obj, obj.name);
            }

            try
            {
                var saveJson = File.ReadAllText(filePath);
                if (saveJson.IsEmpty()) return;
                JsonUtility.FromJsonOverwrite(saveJson, obj);
            }
            catch (Exception)
            {
                secrets[obj.name] = new JsonData();
                Save(new JsonSetting(secrets.Values.ToList()), nameof(JsonManager));
            }
        }

        /// <summary>
        /// 加载解密数据
        /// </summary>
        /// <param name="obj">加载的数据</param>
        public void Decrypt(ScriptableObject obj)
        {
            if (obj.name.IsEmpty())
            {
                obj.name = obj.GetType().Name;
            }

            var filePath = FilePath(obj.name);
            if (!File.Exists(filePath))
            {
                Debug.Log($"创建 {obj.name.Orange()} 数据文件");
                Encrypt(obj, obj.name);
            }

            if (!secrets.ContainsKey(obj.name))
            {
                secrets[name] = new JsonData();
                Save(new JsonSetting(secrets.Values.ToList()), nameof(JsonManager));
                return;
            }

            try
            {
                var saveJson = Decrypt(File.ReadAllBytes(filePath), obj.name);
                if (saveJson.IsEmpty()) return;
                JsonUtility.FromJsonOverwrite(saveJson, obj);
            }
            catch (Exception)
            {
                secrets[obj.name] = new JsonData();
                Save(new JsonSetting(secrets.Values.ToList()), nameof(JsonManager));
            }
        }

        /// <summary>
        /// Json管理器获取路径
        /// </summary>
        /// <param name="name">传入的路径名称</param>
        /// <returns>返回的到的路径</returns>
        private static string FilePath(string name)
        {
            var filePath = Path.Combine(Application.streamingAssetsPath, $"{name}.json");
            if (!File.Exists(filePath))
            {
                filePath = Path.Combine(Application.persistentDataPath, $"{name}.json");
            }

            return filePath;
        }

        /// <summary>
        /// 进行AES加密
        /// </summary>
        /// <param name="json">加密的字符串</param>
        /// <param name="name">加密的数据名称</param>
        /// <returns>返回加密的字节</returns>
        private byte[] Encrypt(string json, string name)
        {
            try
            {
                using var aes = Aes.Create();
                secrets[name] = new JsonData(name, aes.Key, aes.IV);
                using var cryptoTransform = aes.CreateEncryptor(aes.Key, aes.IV);
                using var memoryStream = new MemoryStream();
                using var cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write);
                using (var streamWriter = new StreamWriter(cryptoStream))
                {
                    streamWriter.Write(json);
                }

                return memoryStream.ToArray();
            }
            catch (Exception e)
            {
                Debug.LogWarning($"存档 {name.Red()} 丢失!\n{e}");
                secrets[name] = new JsonData();
                return null;
            }
            finally
            {
                Save(new JsonSetting(secrets.Values.ToList()), nameof(JsonManager));
            }
        }

        /// <summary>
        /// 进行AES解密
        /// </summary>
        /// <param name="json">解密的二进制数据</param>
        /// <param name="name">解密的数据名称</param>
        /// <returns>返回解密的字符串</returns>
        private string Decrypt(byte[] json, string name)
        {
            try
            {
                using var aes = Aes.Create();
                aes.Key = secrets[name].key;
                aes.IV = secrets[name].iv;
                using var cryptoTransform = aes.CreateDecryptor(aes.Key, aes.IV);
                using var memoryStream = new MemoryStream(json);
                using var cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Read);
                using var streamReader = new StreamReader(cryptoStream);
                return streamReader.ReadToEnd();
            }
            catch (Exception e)
            {
                Debug.LogWarning($"存档 {name.Red()} 丢失!\n{e}");
                secrets[name] = new JsonData();
                Save(new JsonSetting(secrets.Values.ToList()), nameof(JsonManager));
                return null;
            }
        }
    }
}