using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace JFramework.Basic
{
    internal class JsonSetting : BaseData
    {
        private readonly Dictionary<string, AesData> aesDict = new Dictionary<string, AesData>();
        public List<AesData> aesList = new List<AesData>();

        private static JsonSetting instance;

        public static JsonSetting Instance
        {
            get
            {
                if (instance != null) return instance;
                instance = ResourceManager.Load<JsonSetting>("Settings/JsonSetting");
                if (instance != null) return instance;
                instance = CreateInstance<JsonSetting>();
                instance.Clear();
#if UNITY_EDITOR
                string path = Application.dataPath + "/Resources/Settings";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    AssetDatabase.Refresh();
                }

                AssetDatabase.CreateAsset(instance, "Assets/Resources/Settings/JsonSetting.asset");
#endif
                return instance;
            }
        }

        public override void InitData()
        {
            foreach (var data in aesList)
            {
                if (!aesDict.ContainsKey(data.ID))
                {
                    aesDict.Add(data.ID, data);
                }
            }
        }

        public override void SaveData() => JsonManager.Save(this, name);

        public override void LoadData() => JsonManager.Load(this);


        public void SetData(string id, byte[] key, byte[] iv)
        {
            if (!aesDict.ContainsKey(id))
            {
                AesData data = new AesData() { ID = id };
                aesDict.Add(id, data);
                aesList.Add(data);
            }
            else
            {
                aesDict[id].key = key;
                aesDict[id].iv = iv;
            }

            SaveData();
        }

        public AesData GetData(string id)
        {
            LoadData();
            if (!aesDict.ContainsKey(id))
            {
                AesData data = new AesData() { ID = id };
                aesDict.Add(id, data);
                aesList.Add(data);
                return data;
            }

            return aesDict[id];
        }

        public void Clear()
        {
            aesDict.Clear();
            aesList.Clear();
            SaveData();
        }

        [Serializable]
        public class AesData
        {
            public string ID;
            public byte[] key;
            public byte[] iv;
        }
    }
}