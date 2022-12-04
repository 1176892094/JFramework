using System;
using System.Collections.Generic;
using JFramework.Basic;
using UnityEngine;

namespace JFramework
{
    internal class DebugData: ScriptableObject
    {
        public bool isDebug;
        public float MaxWidth;
        public float MaxHeight;
        public string screenPath = "Assets/Screen/";
        public LanguageType Language = LanguageType.English;
        public List<Language> textList = new List<Language>();
        private Dictionary<string, Language> textDict;

        public void InitData()
        {
            textDict = new Dictionary<string, Language>();
            foreach (var data in textList)
            {
                if (!textDict.ContainsKey(data.ID))
                {
                    textDict.Add(data.ID, data);
                }
            }
        }

        public string GetData(string key)
        {
            return textDict.ContainsKey(key) ? textDict[key].SetData(Language) : "<None>";
        }
    }

    [Serializable]
    internal class Language
    {
        public string ID;
        public string english;
        public string chinese;

        public string SetData(LanguageType type)
        {
            return type == LanguageType.Chinese ? chinese : english;
        }
    }
}
