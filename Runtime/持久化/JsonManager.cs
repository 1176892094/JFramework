using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JFramework.Core
{
    /// <summary>
    /// Json管理器
    /// </summary>
    public class JsonManager : Singleton<JsonManager>
    {
        /// <summary>
        /// 存储Json的字典
        /// </summary>
        [ShowInInspector, ReadOnly, LabelText("游戏加密密钥"), FoldoutGroup("游戏存档视图")]
        private Dictionary<string, JsonData> jsonDict;

        /// <summary>
        /// 构造函数初始化字典
        /// </summary>
        protected override void OnInit(params object[] args)
        {
            jsonDict = Load<Dictionary<string, JsonData>>(Global.JsonManager);
        }

        /// <summary>
        /// Json管理器存储数据
        /// </summary>
        /// <param name="obj">传入的对象数据</param>
        /// <param name="fileName">存储的文件名称</param>
        /// <param name="AES">设置是否加密</param>
        public void Save(object obj, string fileName, bool AES = false)
        {
            var filePath = GetPath(fileName);
            var saveJson = obj is ScriptableObject ? JsonUtility.ToJson(obj) : JsonConvert.SerializeObject(obj);
            if (AES)
            {
                using Aes aes = Aes.Create();
                SetData(fileName, aes.Key, aes.IV);
                var data = Encrypt(saveJson, aes.Key, aes.IV);
                File.WriteAllBytes(filePath, data);
            }
            else
            {
                File.WriteAllText(filePath, saveJson);
            }
        }

        /// <summary>
        /// Json管理器加载数据
        /// </summary>
        /// <param name="obj">传入的对象数据</param>
        /// <param name="AES">设置是否解密</param>
        public void Load(ScriptableObject obj, bool AES = false)
        {
            var filePath = GetPath(obj.name);
            if (!File.Exists(filePath))
            {
                Debug.Log($"创建存储文件: {obj.name}");
                Save(obj, obj.name, AES);
            }

            if (AES)
            {
                var loadJson = File.ReadAllBytes(filePath);
                var data = GetData(obj.name);
                var key = data.key;
                var iv = data.iv;
                if (key == null || key.Length <= 0 || iv == null || iv.Length <= 0) return;
                var saveJson = Decrypt(loadJson, key, iv);
                JsonUtility.FromJsonOverwrite(saveJson, obj);
                Debug.Log(saveJson);
            }
            else
            {
                var saveJson = File.ReadAllText(filePath);
                JsonUtility.FromJsonOverwrite(saveJson, obj);
                Debug.Log(saveJson);
            }
        }

        /// <summary>
        /// Json管理器加载数据
        /// </summary>
        /// <param name="fileName">存储的文件名称</param>
        /// <param name="AES">设置是否解密</param>
        /// <typeparam name="T">可以使用所有可以被新建的类</typeparam>
        /// <returns>返回存储的游戏数据</returns>
        public T Load<T>(string fileName, bool AES = false) where T : new()
        {
            var filePath = GetPath(fileName);
            if (!File.Exists(filePath))
            {
                Debug.Log($"创建存储文件: {fileName}");
                Save(new T(), fileName, AES);
            }

            if (AES)
            {
                var loadJson = File.ReadAllBytes(filePath);
                var json = GetData(fileName);
                var key = json.key;
                var iv = json.iv;
                if (key == null || key.Length <= 0 || iv == null || iv.Length <= 0) return new T();
                var saveJson = Decrypt(loadJson, key, iv);
                var data = JsonConvert.DeserializeObject<T>(saveJson);
                return data;
            }
            else
            {
                var saveJson = File.ReadAllText(filePath);
                var data = JsonConvert.DeserializeObject<T>(saveJson);
                return data;
            }
        }

        /// <summary>
        /// Json管理器清除存档所有数据
        /// </summary>
        public void Clear()
        {
            jsonDict.Clear();
            Save(jsonDict, Global.JsonManager);
        }

        /// <summary>
        /// Json管理器设置加密数据
        /// </summary>
        /// <param name="id">加密数据的id</param>
        /// <param name="key">加密数据的键值</param>
        /// <param name="iv">加密数据的向量</param>
        private void SetData(string id, byte[] key, byte[] iv)
        {
            if (!jsonDict.ContainsKey(id))
            {
                jsonDict.Add(id, new JsonData());
            }

            jsonDict[id].key = key;
            jsonDict[id].iv = iv;
            Save(jsonDict, Global.JsonManager);
        }

        /// <summary>
        /// Json管理器得到加密数据
        /// </summary>
        /// <param name="id">加密数据的id</param>
        /// <returns>返回得到的加密数据</returns>
        private JsonData GetData(string id)
        {
            if (!jsonDict.ContainsKey(id))
            {
                jsonDict.Add(id, new JsonData());
            }

            return jsonDict[id];
        }

        /// <summary>
        /// Json管理器获取路径
        /// </summary>
        /// <param name="name">传入的路径名称</param>
        /// <returns>返回的到的路径</returns>
        private string GetPath(string name)
        {
            var filePath = Application.streamingAssetsPath + "/" + name + ".json";
            if (!File.Exists(filePath))
            {
                filePath = Application.persistentDataPath + "/" + name + ".json";
            }

            return filePath;
        }

        /// <summary>
        /// 进行AES加密
        /// </summary>
        /// <param name="targetStr">加密的字符串</param>
        /// <param name="key">加密的键值</param>
        /// <param name="iv">加密的向量</param>
        /// <returns></returns>
        private byte[] Encrypt(string targetStr, byte[] key, byte[] iv)
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

        /// <summary>
        /// 进行AES解密
        /// </summary>
        /// <param name="targetByte">解密的二进制数据</param>
        /// <param name="key">解密的键值</param>
        /// <param name="iv">解密的向量</param>
        /// <returns></returns>
        private string Decrypt(byte[] targetByte, byte[] key, byte[] iv)
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

        /// <summary>
        /// Json加密数据
        /// </summary>
        [Serializable]
        private class JsonData
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
    }
}