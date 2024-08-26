// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-02-04  16:28
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

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

            var loadJson = File.ReadAllText(filePath);
            object jsonData = obj is Object ? obj : new JsonMapper<T>(obj);
            JsonUtility.FromJsonOverwrite(loadJson, jsonData);
        }

        public static void Encrypt<T>(T obj, string name)
        {
            var filePath = FilePath(name);
            object jsonData = obj is Object ? obj : new JsonMapper<T>(obj);
            var saveBytes = Encoding.UTF8.GetBytes(JsonUtility.ToJson(jsonData));
            File.WriteAllBytes(filePath, Obfuscator.Encrypt(saveBytes, AES_KEY));
        }

        public static void Decrypt<T>(T obj, string name)
        {
            var filePath = FilePath(name);
            if (!File.Exists(filePath))
            {
                Encrypt(obj, name);
            }

            var loadBytes = Obfuscator.Decrypt(File.ReadAllBytes(filePath), AES_KEY);
            object jsonData = obj is Object ? obj : new JsonMapper<T>(obj);
            JsonUtility.FromJsonOverwrite(Encoding.UTF8.GetString(loadBytes), jsonData);
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