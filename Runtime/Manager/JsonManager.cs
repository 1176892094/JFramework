using System.IO;
using System.Security.Cryptography;
using Newtonsoft.Json;
using UnityEngine;

namespace JFramework
{
    public static class JsonManager
    {
        private static JsonData jsonData;
        public static JsonData JsonData
        {
            get
            {
                if (jsonData == null)
                {
                    jsonData = ResourceManager.Load<JsonData>("Settings/JsonData");
                }

                return jsonData;
            }
        }

        public static void Save(object obj, string fileName, bool AES = false)
        {
            string filePath = Application.persistentDataPath + "/" + fileName + ".json";
            string saveJson = obj is ScriptableObject ? JsonUtility.ToJson(obj) : JsonConvert.SerializeObject(obj);
            if (AES)
            {
                using Aes aes = Aes.Create();
                if (JsonData.GetData(fileName) == null)
                {
                    JsonData.AddData(fileName);
                }
                JsonData.GetData(fileName).key = aes.Key;
                JsonData.GetData(fileName).iv = aes.IV;
                JsonData.SaveData();
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
                Save(obj, obj.name);
                return;
            }

            if (AES)
            {
                byte[] loadJson = File.ReadAllBytes(filePath);
                JsonData.LoadData();
                if (JsonData.GetData(obj.name) == null)
                {
                    JsonData.AddData(obj.name);
                }
                byte[] key = JsonData.GetData(obj.name).key;
                byte[] iv = JsonData.GetData(obj.name).iv;
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

        public static T Load<T>(string fileName) where T : new()
        {
            string filePath = Application.streamingAssetsPath + "/" + fileName + ".json";
            if (!File.Exists(filePath))
            {
                filePath = Application.persistentDataPath + "/" + fileName + ".json";
            }

            if (!File.Exists(filePath)) return new T();
            string saveJson = File.ReadAllText(filePath);
            T data = JsonConvert.DeserializeObject<T>(saveJson);
            return data;
        }

        private static byte[] Encrypt(string targetStr, byte[] key, byte[] iv)
        {
            if (targetStr.Length == 0)
            {
                Debug.LogWarning("加密数据为空！");
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
                Debug.LogWarning("解密数据为空！");
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