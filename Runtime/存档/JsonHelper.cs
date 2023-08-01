using System;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

namespace JFramework.Core
{
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
                using Aes aes = Aes.Create();
                secrets[name] = new JsonData(aes.Key, aes.IV);
                ICryptoTransform cryptoTF = aes.CreateEncryptor(aes.Key, aes.IV);
                using MemoryStream memoryStream = new MemoryStream();
                using CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTF, CryptoStreamMode.Write);
                using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                {
                    streamWriter.Write(json);
                }

                return memoryStream.ToArray();
            }
            catch (Exception e)
            {
                Debug.LogWarning($"{nameof(JsonManager).Red()} {name} 存档丢失 => 加密失败!\n" + e);
                secrets[name] = new JsonData();
                Save(secrets, nameof(JsonManager));
                return null;
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
                using Aes aes = Aes.Create();
                aes.Key = secrets[name].key;
                aes.IV = secrets[name].iv;
                ICryptoTransform cryptoTF = aes.CreateDecryptor(aes.Key, aes.IV);
                using MemoryStream memoryStream = new MemoryStream(json);
                using CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTF, CryptoStreamMode.Read);
                using StreamReader streamReader = new StreamReader(cryptoStream);
                return streamReader.ReadToEnd();
            }
            catch (Exception e)
            {
                Debug.LogWarning($"{nameof(JsonManager).Red()} {name} 存档丢失 => 解密失败!\n" + e);
                secrets[name] = new JsonData();
                Save(secrets, nameof(JsonManager));
                return null;
            }
        }
    }
}