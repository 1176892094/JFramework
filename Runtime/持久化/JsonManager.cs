using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using JFramework.Interface;
using Newtonsoft.Json;
using UnityEngine;

namespace JFramework.Core
{
    /// <summary>
    /// Json管理器
    /// </summary>
    public sealed class JsonManager : Singleton<JsonManager>
    {
        /// <summary>
        /// 存储Json的字典
        /// </summary>
        internal Dictionary<string, JsonData> jsonDict;

        internal override void Awake()
        {
            base.Awake();
            jsonDict = Load<Dictionary<string, JsonData>>(Name);
        }

        /// <summary>
        /// 存储数据
        /// </summary>
        /// <param name="obj">保存的数据</param>
        /// <param name="name">保存的名称</param>
        /// <param name="isAes">加密</param>
        public void Save(object obj, string name, bool isAes = false)
        {
            var filePath = GetPath(name);
            var saveJson = obj is ScriptableObject ? JsonUtility.ToJson(obj) : JsonConvert.SerializeObject(obj);
            if (isAes)
            {
                if (jsonDict == null)
                {
                    Debug.Log($"{Name.Red()} 没有初始化!");
                    return;
                }

                if (DebugManager.IsDebugJson)
                {
                    Debug.Log($"{Name.Sky()} <= Encrypt => {name.Orange()}");
                }

                using Aes aes = Aes.Create();
                SetData(name, aes.Key, aes.IV);
                var data = JsonSetting.Encrypt(saveJson, aes.Key, aes.IV);
                File.WriteAllBytes(filePath, data);
            }
            else
            {
                if (DebugManager.IsDebugJson)
                {
                    Debug.Log($"{Name.Sky()} <= Save => {name.Orange()}");
                }

                File.WriteAllText(filePath, saveJson);
            }
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="obj">加载的数据</param>
        /// <param name="isAes">解密</param>
        public void Load(ScriptableObject obj, bool isAes = false)
        {
            var filePath = GetPath(obj.name);
            if (!File.Exists(filePath))
            {
                Debug.Log($"{Name.Sky()} <= Create => {obj.name.Orange()}");
                Save(obj, obj.name, isAes);
            }

            if (isAes)
            {
                if (jsonDict == null)
                {
                    Debug.Log($"{Name.Red()} 没有初始化!");
                    return;
                }

                if (DebugManager.IsDebugJson)
                {
                    Debug.Log($"{Name.Sky()} <= Decrypt => {obj.name.Orange()}");
                }

                var loadJson = File.ReadAllBytes(filePath);
                var data = GetData(obj.name);
                var key = data.key;
                var iv = data.iv;
                if (key == null || key.Length <= 0 || iv == null || iv.Length <= 0) return;
                var saveJson = JsonSetting.Decrypt(loadJson, key, iv);
                JsonUtility.FromJsonOverwrite(saveJson, obj);
            }
            else
            {
                if (DebugManager.IsDebugJson)
                {
                    Debug.Log($"{Name.Sky()} <= Load => {Name.Orange()}");
                }
                
                var saveJson = File.ReadAllText(filePath);
                JsonUtility.FromJsonOverwrite(saveJson, obj);
            }
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="name">加载的数据名称</param>
        /// <param name="isAes">解密</param>
        /// <typeparam name="T">可以使用任何类型</typeparam>
        /// <returns>返回解密的数据</returns>
        public T Load<T>(string name, bool isAes = false) where T : class, new()
        {
            var filePath = GetPath(name);
            if (!File.Exists(filePath))
            {
                Debug.Log($"{Name.Sky()} <= Create => {name.Orange()}");
                Save(new T(), name, isAes);
            }

            if (isAes)
            {
                if (jsonDict == null)
                {
                    Debug.Log($"{Name.Red()} 没有初始化!");
                    return null;
                }
                
                if (DebugManager.IsDebugJson)
                {
                    Debug.Log($"{Name.Sky()} <= Decrypt => {name.Orange()}");
                }
                
                var loadJson = File.ReadAllBytes(filePath);
                var json = GetData(name);
                var key = json.key;
                var iv = json.iv;
                if (key == null || key.Length <= 0 || iv == null || iv.Length <= 0) return new T();
                var saveJson = JsonSetting.Decrypt(loadJson, key, iv);
                var data = JsonConvert.DeserializeObject<T>(saveJson);
                return data;
            }
            else
            {
                if (DebugManager.IsDebugJson)
                {
                    Debug.Log($"{Name.Sky()} <= Load => {name.Orange()}");
                }
                
                var saveJson = File.ReadAllText(filePath);
                var data = JsonConvert.DeserializeObject<T>(saveJson);
                return data;
            }
        }

        /// <summary>
        /// 清空管理器
        /// </summary>
        public void Clear()
        {
            if (DebugManager.IsDebugJson)
            {
                Debug.Log($"{Name.Sky()} <= Clear => {Name.Orange()}");
            }

            jsonDict.Clear();
            Save(jsonDict, Name);
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
            Save(jsonDict, Name);
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
        /// Json管理器清除存档所有数据
        /// </summary>
        internal override void Destroy()
        {
            base.Destroy();
            jsonDict = null;
        }
    }
}