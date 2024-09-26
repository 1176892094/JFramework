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

namespace JFramework
{
    public static partial class JsonManager
    {
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
            object jsonData = obj is Object ? obj : new JsonMapper<T>(obj);
            string readJson;
            if (!File.Exists(filePath))
            {
                readJson = JsonUtility.ToJson(jsonData);
                File.WriteAllText(filePath, readJson);
            }

            readJson = File.ReadAllText(filePath);
            JsonUtility.FromJsonOverwrite(readJson, jsonData);
        }

        public static void Encrypt<T>(T obj, string name, string key = Obfuscator.AES_KEY)
        {
            var filePath = FilePath(name);
            object jsonData = obj is Object ? obj : new JsonMapper<T>(obj);
            var saveJson = JsonUtility.ToJson(jsonData);
            saveJson = Compression.Compress(saveJson);
            var saveBytes = Encoding.UTF8.GetBytes(saveJson);
            saveBytes = Obfuscator.Encrypt(saveBytes, key);
            File.WriteAllBytes(filePath, saveBytes);
        }

        public static void Decrypt<T>(T obj, string name, string key = Obfuscator.AES_KEY)
        {
            var filePath = FilePath(name);
            object jsonData = obj is Object ? obj : new JsonMapper<T>(obj);
            byte[] readBytes;
            string saveJson;
            if (!File.Exists(filePath))
            {
                saveJson = JsonUtility.ToJson(jsonData);
                saveJson = Compression.Compress(saveJson);
                readBytes = Encoding.UTF8.GetBytes(saveJson);
                readBytes = Obfuscator.Encrypt(readBytes, key);
                File.WriteAllBytes(filePath, readBytes);
            }

            readBytes = File.ReadAllBytes(filePath);
            readBytes = Obfuscator.Decrypt(readBytes, key);
            saveJson = Encoding.UTF8.GetString(readBytes);
            saveJson = Compression.Decompress(saveJson);
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
    }
}