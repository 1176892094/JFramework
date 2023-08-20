using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
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
        private static async Task<byte[]> Encrypt(string json, string name)
        {
            try
            {
                using var aes = Aes.Create();
                secrets[name] = new JsonData(aes.Key, aes.IV);
                var cryptoTransform = aes.CreateEncryptor(aes.Key, aes.IV);
                using var memoryStream = new MemoryStream();
                await using var cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write);
                await using (var streamWriter = new StreamWriter(cryptoStream))
                {
                    await streamWriter.WriteAsync(json);
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
                await Save(secrets, nameof(JsonManager));
            }
        }

        /// <summary>
        /// 进行AES解密
        /// </summary>
        /// <param name="json">解密的二进制数据</param>
        /// <param name="name">解密的数据名称</param>
        /// <returns>返回解密的字符串</returns>
        private static async Task<string> Decrypt(byte[] json, string name)
        {
            try
            {
                using var aes = Aes.Create();
                aes.Key = secrets[name].key;
                aes.IV = secrets[name].iv;
                var cryptoTransform = aes.CreateDecryptor(aes.Key, aes.IV);
                using var memoryStream = new MemoryStream(json);
                await using var cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Read);
                using var streamReader = new StreamReader(cryptoStream);
                return await streamReader.ReadToEndAsync();
            }
            catch (Exception e)
            {
                Debug.LogWarning($"存档 {name.Red()} 丢失!\n{e}");
                secrets[name] = new JsonData();
                await Save(secrets, nameof(JsonManager));
                return null;
            }
        }
    }
}