using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using JFramework.Core;
using UnityEngine;

namespace JFramework.Interface
{
    /// <summary>
    /// Json加密数据
    /// </summary>
    [Serializable]
    internal class JsonData
    {
        /// <summary>
        /// 加密数据的键值
        /// </summary>
        public byte[] key;

        /// <summary>
        /// 加密数据的向量
        /// </summary>
        public byte[] iv;
    }

    internal static class JsonSetting
    {
        /// <summary>
        /// 进行AES加密
        /// </summary>
        /// <param name="targetStr">加密的字符串</param>
        /// <param name="key">加密的键值</param>
        /// <param name="iv">加密的向量</param>
        /// <returns>返回加密的字节</returns>
        public static byte[] Encrypt(string targetStr, byte[] key, byte[] iv)
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
                    streamWriter.Write(targetStr);
                }

                return memoryStream.ToArray();
            }
            catch (Exception)
            {
                Debug.LogWarning($"{nameof(JsonManager).Red()} 存档丢失 => 加密失败!");
                JsonManager.jsonDict = new Dictionary<string, JsonData>();
            }

            return null;
        }

        /// <summary>
        /// 进行AES解密
        /// </summary>
        /// <param name="targetByte">解密的二进制数据</param>
        /// <param name="key">解密的键值</param>
        /// <param name="iv">解密的向量</param>
        /// <returns>返回解密的字符串</returns>
        public static string Decrypt(byte[] targetByte, byte[] key, byte[] iv)
        {
            try
            {
                using Aes aes = Aes.Create();
                aes.Key = key;
                aes.IV = iv;
                ICryptoTransform cryptoTF = aes.CreateDecryptor(aes.Key, aes.IV);
                using MemoryStream memoryStream = new MemoryStream(targetByte);
                using CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTF, CryptoStreamMode.Read);
                using StreamReader streamReader = new StreamReader(cryptoStream);
                return streamReader.ReadToEnd();
            }
            catch (Exception)
            {
                Debug.LogWarning($"{nameof(JsonManager).Red()} 存档丢失 => 解密失败!");
                JsonManager.jsonDict = new Dictionary<string, JsonData>();
            }

            return null;
        }
    }
}