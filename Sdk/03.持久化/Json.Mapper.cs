// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-08 22:01:43
// # Recently: 2025-01-08 22:01:43
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JFramework
{
    public static partial class JsonManager
    {
        [Serializable]
        private class JsonMapper<T>
        {
            public T value;

            public JsonMapper(T value)
            {
                this.value = value;
            }
        }

        public static string ToJson<T>(T data)
        {
            if (typeof(T).IsSubclassOf(typeof(Object)))
            {
                return JsonUtility.ToJson(data);
            }

            return JsonUtility.ToJson(new JsonMapper<T>(data));
        }

        public static void FromJson<T>(string json, T data)
        {
            if (typeof(T).IsSubclassOf(typeof(Object)))
            {
                JsonUtility.FromJsonOverwrite(json, data);
                return;
            }

            JsonUtility.FromJsonOverwrite(json, new JsonMapper<T>(data));
        }

        public static T FromJson<T>(string json)
        {
            if (typeof(T).IsSubclassOf(typeof(Object)))
            {
                return JsonUtility.FromJson<T>(json);
            }

            return JsonUtility.FromJson<JsonMapper<T>>(json).value;
        }
    }
}