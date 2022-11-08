using System;
using System.Collections.Generic;

namespace JFramework
{
    public class DebugData: BaseData
    {
        private readonly Dictionary<string, Language> textDict = new Dictionary<string, Language>();
        public LanguageType Language = LanguageType.English;
        public List<Language> textList = new List<Language>();

        public override void InitData()
        {
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
    public class Language
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
