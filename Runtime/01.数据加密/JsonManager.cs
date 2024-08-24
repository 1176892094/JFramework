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
using System.IO;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JFramework.Core
{
    public static partial class JsonManager
    {
        private const string AES_KEY = "0123456789ABCDEF";

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
            var filePath = FilePath(name);
            object jsonData = obj is Object ? obj : new JsonMapper<T>(obj);
            var saveJson = JsonUtility.ToJson(jsonData);
            var saveBytes = Obfuscator.Encrypt(Encoding.UTF8.GetBytes(saveJson), AES_KEY);
            File.WriteAllBytes(filePath, saveBytes);
        }

        public static void Decrypt<T>(T obj, string name)
        {
            var filePath = FilePath(name);
            if (!File.Exists(filePath))
            {
                Encrypt(obj, name);
            }

            var loadBytes = Obfuscator.Decrypt(File.ReadAllBytes(filePath), AES_KEY);
            var loadJson = Encoding.UTF8.GetString(loadBytes);
            if (string.IsNullOrEmpty(loadJson)) return;
            object jsonData = obj is Object ? obj : new JsonMapper<T>(obj);
            JsonUtility.FromJsonOverwrite(loadJson, jsonData);
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
    }
}