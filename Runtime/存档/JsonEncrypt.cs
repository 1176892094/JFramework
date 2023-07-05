using System;
using System.IO;
using System.Security.Cryptography;
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
            Log.Info(DebugOption.Json,$"保存加密 => {name.Orange()} 数据文件");
            using Aes aes = Aes.Create();
            {
                SetData(name, aes.Key, aes.IV);
                var data = JsonSetting.Encrypt(saveJson, aes.Key, aes.IV);
                File.WriteAllBytes(filePath, data);
            }
        }

        /// <summary>
        /// 加载解密数据
        /// </summary>
        /// <param name="obj">加载的数据</param>
        public static void Decrypt(ScriptableObject obj)
        {
            var filePath = GetPath(obj.name);
            if (!File.Exists(filePath))
            {
                Debug.Log($"{nameof(JsonManager)} 创建 => {obj.name.Orange()} 数据文件");
                Encrypt(obj, obj.name);
            }
            
            Log.Info(DebugOption.Json,$"读取解密 => {obj.name.Orange()} 数据文件");
            var loadJson = File.ReadAllBytes(filePath);
            var data = GetData(obj.name);
            var key = data.key;
            var iv = data.iv;
            if (key is not { Length: > 0 } || iv is not { Length: > 0 }) return;
            var saveJson = JsonSetting.Decrypt(loadJson, key, iv);
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
            if (!File.Exists(filePath))
            {
                Debug.Log($"{nameof(JsonManager)} 创建 => {name.Orange()} 数据文件");
                Encrypt(new T(), name);
            }
            
            Log.Info(DebugOption.Json,$"读取解密 => {name.Orange()} 数据文件");
            var loadJson = File.ReadAllBytes(filePath);
            var json = GetData(name);
            var key = json.key;
            var iv = json.iv;
            if (key is not { Length: > 0 } || iv is not { Length: > 0 }) return new T();
            var saveJson = JsonSetting.Decrypt(loadJson, key, iv);
            return !saveJson.IsEmpty() ? JsonConvert.DeserializeObject<T>(saveJson) : default;
        }

        internal static class JsonSetting
        {
            /// <summary>
            /// 进行AES加密
            /// </summary>
            /// <param name="string">加密的字符串</param>
            /// <param name="key">加密的键值</param>
            /// <param name="iv">加密的向量</param>
            /// <returns>返回加密的字节</returns>
            public static byte[] Encrypt(string @string, byte[] key, byte[] iv)
            {
                try
                {
                    using Aes aes = Aes.Create();
                    aes.Key = key;
                    aes.IV = iv;
                    ICryptoTransform cryptoTF = aes.CreateEncryptor(aes.Key, aes.IV);
                    using MemoryStream memoryStream = new MemoryStream();
                    using CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTF, CryptoStreamMode.Write);
                    using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                    {
                        streamWriter.Write(@string);
                    }

                    return memoryStream.ToArray();
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"{nameof(JsonManager).Red()} 存档丢失 => 加密失败!\n" + e);
                    Clear();
                }

                return null;
            }

            /// <summary>
            /// 进行AES解密
            /// </summary>
            /// <param name="byte">解密的二进制数据</param>
            /// <param name="key">解密的键值</param>
            /// <param name="iv">解密的向量</param>
            /// <returns>返回解密的字符串</returns>
            public static string Decrypt(byte[] @byte, byte[] key, byte[] iv)
            {
                try
                {
                    using Aes aes = Aes.Create();
                    aes.Key = key;
                    aes.IV = iv;
                    ICryptoTransform cryptoTF = aes.CreateDecryptor(aes.Key, aes.IV);
                    using MemoryStream memoryStream = new MemoryStream(@byte);
                    using CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTF, CryptoStreamMode.Read);
                    using StreamReader streamReader = new StreamReader(cryptoStream);
                    return streamReader.ReadToEnd();
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"{nameof(JsonManager).Red()} 存档丢失 => 解密失败!\n" + e);
                    Clear();
                }

                return null;
            }
        }
    }
}