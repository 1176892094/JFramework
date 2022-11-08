using System;
using System.Collections.Generic;
using UnityEngine;

namespace JFramework
{
    public class JsonData : BaseData
    {
        private readonly Dictionary<string, AesData> aesDict = new Dictionary<string, AesData>();
        public List<AesData> aesList = new List<AesData>();

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

        public AesData GetData(string id)
        {
            if (aesDict.ContainsKey(id)) return aesDict[id];
            return null;
        }

        public void AddData(string id)
        {
            AesData data = new AesData() { ID = id };
            aesDict.Add(id,data);
            aesList.Add(data);
        }

        public void Clear()
        {
            aesDict.Clear();
            aesList.Clear();
            SaveData();
        }
    }
    
    [Serializable]
    public class AesData
    {
        public string ID;
        public byte[] key;
        public byte[] iv;
    }
}