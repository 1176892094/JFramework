// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-09 19:12:10
// # Recently: 2024-12-22 20:12:33
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JFramework
{
    internal sealed partial class DefaultHelper : IJsonHelper
    {
        public string ToJson<T>(T data)
        {
            if (typeof(T).IsSubclassOf(typeof(Object)))
            {
                return JsonUtility.ToJson(data);
            }

            return JsonUtility.ToJson(new JsonMapper<T>(data));
        }

        public void FromJson<T>(string json, T data)
        {
            if (typeof(T).IsSubclassOf(typeof(Object)))
            {
                JsonUtility.FromJsonOverwrite(json, data);
                return;
            }

            JsonUtility.FromJsonOverwrite(json, new JsonMapper<T>(data));
        }

        public T FromJson<T>(string json)
        {
            if (typeof(T).IsSubclassOf(typeof(Object)))
            {
                return JsonUtility.FromJson<T>(json);
            }

            return JsonUtility.FromJson<JsonMapper<T>>(json).value;
        }

        [Serializable]
        private class JsonMapper<T>
        {
            public T value;

            public JsonMapper(T value)
            {
                this.value = value;
            }
        }
    }
}