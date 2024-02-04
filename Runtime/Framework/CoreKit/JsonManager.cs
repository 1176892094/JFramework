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
using Sirenix.OdinInspector;
using UnityEngine;

namespace JFramework.Core
{
    public sealed class JsonManager : Component<GlobalManager>
    {
        [ShowInInspector] private Dictionary<string, Json> secrets = new Dictionary<string, Json>();

        private void Awake() => secrets = Load<List<Json>>(nameof(JsonManager)).ToDictionary(json => json.name);

        public void Save<T>(T obj, string name) where T : new()
        {
            var filePath = FilePath(name);
            var saveJson = JsonUtility.ToJson(new Variable<T>(obj));
            File.WriteAllText(filePath, saveJson);
        }

        public T Load<T>(string name) where T : new()
        {
            var filePath = FilePath(name);
            if (!File.Exists(filePath))
            {
                Save(new T(), name);
            }

            var saveJson = File.ReadAllText(filePath);
            var jsonData = JsonUtility.FromJson<Variable<T>>(saveJson).value;
            return !string.IsNullOrEmpty(saveJson) ? jsonData : new T();
        }

        public void Encrypt<T>(T obj, string name) where T : new()
        {
            var filePath = FilePath(name);
            var saveJson = Encrypt(JsonUtility.ToJson(new Variable<T>(obj)), name);
            File.WriteAllBytes(filePath, saveJson);
        }

        public T Decrypt<T>(string name) where T : new()
        {
            var filePath = FilePath(name);
            if (!File.Exists(filePath))
            {
                Encrypt(new T(), name);
            }

            if (!secrets.ContainsKey(name))
            {
                secrets[name] = new Json(name);
                Save(secrets.Values.ToList(), nameof(JsonManager));
                return new T();
            }

            try
            {
                var saveJson = Decrypt(File.ReadAllBytes(filePath), name);
                var jsonData = JsonUtility.FromJson<Variable<T>>(saveJson).value;
                return !string.IsNullOrEmpty(saveJson) ? jsonData : new T();
            }
            catch (Exception)
            {
                secrets[name] = new Json(name);
                Save(secrets.Values.ToList(), nameof(JsonManager));
                return new T();
            }
        }

        public void Save(ScriptableObject obj)
        {
            var filePath = FilePath(obj);
            var saveJson = JsonUtility.ToJson(obj);
            File.WriteAllText(filePath, saveJson);
        }

        public void Load(ScriptableObject obj)
        {
            var filePath = FilePath(obj);
            if (!File.Exists(filePath))
            {
                Save(obj);
            }

            var saveJson = File.ReadAllText(filePath);
            if (string.IsNullOrEmpty(saveJson)) return;
            JsonUtility.FromJsonOverwrite(saveJson, obj);
        }

        public void Encrypt(ScriptableObject obj)
        {
            var filePath = FilePath(obj);
            var saveJson = Encrypt(JsonUtility.ToJson(obj), obj.name);
            File.WriteAllBytes(filePath, saveJson);
        }

        public void Decrypt(ScriptableObject obj)
        {
            var filePath = FilePath(obj);
            if (!File.Exists(filePath))
            {
                Encrypt(obj);
            }

            if (!secrets.ContainsKey(obj.name))
            {
                secrets[obj.name] = new Json(obj.name);
                Save(secrets.Values.ToList(), nameof(JsonManager));
                return;
            }

            try
            {
                var saveJson = Decrypt(File.ReadAllBytes(filePath), obj.name);
                if (string.IsNullOrEmpty(saveJson)) return;
                JsonUtility.FromJsonOverwrite(saveJson, obj);
            }
            catch (Exception)
            {
                secrets[obj.name] = new Json(obj.name);
                Save(secrets.Values.ToList(), nameof(JsonManager));
            }
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

        private static string FilePath(ScriptableObject obj)
        {
            if (string.IsNullOrEmpty(obj.name))
            {
                obj.name = obj.GetType().Name;
            }

            var filePath = Path.Combine(Application.streamingAssetsPath, $"{obj.name}.json");
            if (!File.Exists(filePath))
            {
                filePath = Path.Combine(Application.persistentDataPath, $"{obj.name}.json");
            }

            return filePath;
        }

        private byte[] Encrypt(string json, string name)
        {
            try
            {
                using var aes = Aes.Create();
                secrets[name] = new Json(name, aes.Key, aes.IV);
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
                secrets[name] = new Json(name);
                Debug.LogWarning($"存档 {name.Red()} 丢失!\n{e}");
                return null;
            }
            finally
            {
                Save(secrets.Values.ToList(), nameof(JsonManager));
            }
        }

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
                secrets[name] = new Json(name);
                Debug.LogWarning($"存档 {name.Red()} 丢失!\n{e}");
                Save(secrets.Values.ToList(), nameof(JsonManager));
                return null;
            }
        }
    }
}