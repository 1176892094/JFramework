// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-10-25  00:01
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

namespace JFramework.Core
{
    using JsonSetting = Variables<JsonData>;

    /// <summary>
    /// Json管理器
    /// </summary>
    public static partial class JsonManager
    {
        /// <summary>
        /// Json管理器获取路径
        /// </summary>
        /// <param name="name">传入的路径名称</param>
        /// <returns>返回的到的路径</returns>
        private static string GetPath(string name)
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
        private static byte[] Encrypt(string json, string name)
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
        private static string Decrypt(byte[] json, string name)
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