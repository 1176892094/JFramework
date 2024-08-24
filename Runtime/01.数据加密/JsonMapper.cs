// *********************************************************************************
// # Project: Forest
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-08-25  04:08
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using UnityEngine;

namespace JFramework.Core
{
    public static partial class JsonManager
    {
        public static T Read<T>(string json)
        {
            return JsonUtility.FromJson<JsonMapper<T>>(json).value;
        }

        public static string Write<T>(T obj)
        {
            return JsonUtility.ToJson(new JsonMapper<T>(obj));
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