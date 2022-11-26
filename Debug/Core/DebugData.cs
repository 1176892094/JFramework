using System;
using System.Collections.Generic;
using UnityEngine.Device;

namespace JFramework.Logger
{
    internal class DebugData: BaseData
    {
        private readonly Dictionary<string, Language> textDict = new Dictionary<string, Language>();
        public List<Language> textList = new List<Language>();
        public bool isDebug;
        public float MaxWidth;
        public float MaxHeight;
        public LanguageType Language = LanguageType.English;

        public override void InitData()
        {
            isDebug = true;
            MaxWidth = Screen.width;
            MaxHeight = Screen.height;
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

    public enum LanguageType
    {
        Chinese,
        English
    }
}
