using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using JFramework.Basic;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using Logger = JFramework.Basic.Logger;

namespace JFramework
{
    public static class JsonManager
    {
        public static void InitData()
        {
            JsonSetting.Instance.Clear();
        }

        public static void LoadData()
        {
            JsonSetting.Instance.LoadData();
            JsonSetting.Instance.InitData();
        }

        public static void Save(object obj, string fileName, bool AES = false)
        {
            string filePath = Application.persistentDataPath + "/" + fileName + ".json";
            string saveJson = obj is ScriptableObject ? JsonUtility.ToJson(obj) : JsonConvert.SerializeObject(obj);
            if (AES)
            {
                using Aes aes = Aes.Create();
                JsonSetting.Instance.SetData(fileName, aes.Key, aes.IV);
                byte[] data = Encrypt(saveJson, aes.Key, aes.IV);
                File.WriteAllBytes(filePath, data);
            }
            else
            {
                File.WriteAllText(filePath, saveJson);
            }
        }

        public static void Load(ScriptableObject obj, bool AES = false)
        {
            string filePath = Application.streamingAssetsPath + "/" + obj.name + ".json";
            if (!File.Exists(filePath))
            {
                filePath = Application.persistentDataPath + "/" + obj.name + ".json";
            }

            if (!File.Exists(filePath))
            {
                Logger.Log($"创建存储文件: {obj.name}");
                Save(obj, obj.name);
                Load(obj);
                return;
            }

            if (AES)
            {
                byte[] loadJson = File.ReadAllBytes(filePath);
                JsonSetting.AesData data = JsonSetting.Instance.GetData(obj.name);
                byte[] key = data.key;
                byte[] iv = data.iv;
                if (key == null || key.Length <= 0 || iv == null || iv.Length <= 0) return;
                string saveJson = Decrypt(loadJson, key, iv);
                JsonUtility.FromJsonOverwrite(saveJson, obj);
            }
            else
            {
                string saveJson = File.ReadAllText(filePath);
                JsonUtility.FromJsonOverwrite(saveJson, obj);
            }
        }

        public static T Load<T>(string fileName, bool AES = false) where T : new()
        {
            string filePath = Application.streamingAssetsPath + "/" + fileName + ".json";
            if (!File.Exists(filePath))
            {
                filePath = Application.persistentDataPath + "/" + fileName + ".json";
            }

            if (!File.Exists(filePath)) return new T();
            if (AES)
            {
                byte[] loadJson = File.ReadAllBytes(filePath);
                JsonSetting.AesData aes = JsonSetting.Instance.GetData(fileName);
                byte[] key = aes.key;
                byte[] iv = aes.iv;
                if (key == null || key.Length <= 0 || iv == null || iv.Length <= 0)
                {
                    return new T();
                }

                string saveJson = Decrypt(loadJson, key, iv);
                T data = JsonConvert.DeserializeObject<T>(saveJson);
                return data;
            }
            else
            {
                string saveJson = File.ReadAllText(filePath);
                T data = JsonConvert.DeserializeObject<T>(saveJson);
                return data;
            }
        }

        private static byte[] Encrypt(string targetStr, byte[] key, byte[] iv)
        {
            if (targetStr.Length == 0)
            {
                Logger.LogWarning("加密数据为空！");
                return null;
            }

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

        private static string Decrypt(byte[] targetByte, byte[] key, byte[] iv)
        {
            if (targetByte.Length == 0)
            {
                Logger.LogWarning("解密数据为空！");
                return null;
            }

            using Aes aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;
            ICryptoTransform cryptoTF = aes.CreateDecryptor(aes.Key, aes.IV);
            using MemoryStream memoryStream = new MemoryStream(targetByte);
            using CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTF, CryptoStreamMode.Read);
            using StreamReader streamReader = new StreamReader(cryptoStream);
            return streamReader.ReadToEnd();
        }
    }
}