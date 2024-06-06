// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-02-04  16:28
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JFramework.Core
{
    public static class JsonManager
    {
        private static Dictionary<string, JsonData> secrets = new();

        internal static void Register()
        {
            var copies = new List<JsonData>();
            Load(copies, nameof(JsonManager));
            secrets = copies.ToDictionary(json => json.name);
        }

        public static void Save<T>(T obj, string name)
        {
            var filePath = FilePath(name);
            object jsonData = obj is Object ? obj : new JsonMapper<T>(obj);
            var saveJson = JsonUtility.ToJson(jsonData);
            File.WriteAllText(filePath, saveJson);
        }

        public static void Load<T>(T obj, string name)
        {
            var filePath = FilePath(name);
            if (!File.Exists(filePath))
            {
                Save(obj, name);
            }

            var saveJson = File.ReadAllText(filePath);
            if (string.IsNullOrEmpty(saveJson)) return;
            object jsonData = obj is Object ? obj : new JsonMapper<T>(obj);
            JsonUtility.FromJsonOverwrite(saveJson, jsonData);
        }

        public static void Encrypt<T>(T obj, string name)
        {
            if (!GlobalManager.Instance) return;
            var filePath = FilePath(name);
            object jsonData = obj is Object ? obj : new JsonMapper<T>(obj);
            var saveJson = Encrypt(JsonUtility.ToJson(jsonData), name);
            File.WriteAllBytes(filePath, saveJson);
        }

        public static void Decrypt<T>(T obj, string name)
        {
            if (!GlobalManager.Instance) return;
            var filePath = FilePath(name);
            if (!File.Exists(filePath))
            {
                Encrypt(obj, name);
            }

            var saveJson = Decrypt(File.ReadAllBytes(filePath), name);
            if (string.IsNullOrEmpty(saveJson)) return;
            object jsonData = obj is Object ? obj : new JsonMapper<T>(obj);
            JsonUtility.FromJsonOverwrite(saveJson, jsonData);
        }

        private static string FilePath(string name)
        {
            var filePath = Path.Combine(Application.streamingAssetsPath, $"{name}.json");
            if (!File.Exists(filePath))
            {
                filePath = Path.Combine(Application.persistentDataPath, $"{name}.json");
            }

            return filePath;
        }

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
                secrets[name] = new JsonData(name);
                Debug.LogWarning($"存档 {name.Red()} 丢失!\n{e}");
                return null;
            }
            finally
            {
                Save(secrets.Values.ToList(), nameof(JsonManager));
            }
        }

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
                secrets[name] = new JsonData(name);
                Debug.LogWarning($"存档 {name.Red()} 丢失!\n{e}");
                Save(secrets.Values.ToList(), nameof(JsonManager));
                return null;
            }
        }

        public static T Reader<T>(string json)
        {
            return JsonUtility.FromJson<JsonMapper<T>>(json).value;
        }

        public static string Writer<T>(T obj, bool isPretty = false)
        {
            return JsonUtility.ToJson(new JsonMapper<T>(obj), isPretty);
        }

        [Serializable]
        private class JsonMapper<T>
        {
            public T value;

            public JsonMapper(T value)
            {
                this.value = value;
            }
        }

        [Serializable]
        private struct JsonData
        {
            public string name;
            public byte[] key;
            public byte[] iv;

            public JsonData(string name)
            {
                this.name = name;
                iv = Array.Empty<byte>();
                key = Array.Empty<byte>();
            }

            public JsonData(string name, byte[] key, byte[] iv)
            {
                this.iv = iv;
                this.key = key;
                this.name = name;
            }

            public static implicit operator bool(JsonData data)
            {
                return data.key != null && data.key.Length != 0 && data.iv != null && data.iv.Length != 0;
            }
        }
    }
}