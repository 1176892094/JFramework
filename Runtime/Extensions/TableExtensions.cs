// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: jinyijie
// # Version: 1.0.0
// # History: 2024-4-4  18:2
// # Copyright: 2024, jinyijie
// # Description: This is an automatically generated comment.
// *********************************************************************************

#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

// ReSharper disable All

namespace JFramework
{
    public static partial class Extensions
    {
        public static void Input(this string reason, out SecretInt result)
        {
            result = int.Parse(reason);
        }

        public static void Input(this string reason, out SecretBool result)
        {
            result = bool.Parse(reason);
        }

        public static void Input(this string reason, out SecretFloat result)
        {
            result = float.Parse(reason);
        }

        public static void Input(this string reason, out SecretString result)
        {
            result = reason ?? string.Empty;
        }

        public static void Input(this string reason, out string result)
        {
            result = reason ?? string.Empty;
        }

        public static void Input(this string reason, out Vector2 result)
        {
            var points = reason.Split(',');
            points[0].Input(out float x);
            points[1].Input(out float y);
            result = new Vector2(x, y);
        }

        public static void Input(this string reason, out Vector3 result)
        {
            var points = reason.Split(',');
            points[0].Input(out float x);
            points[1].Input(out float y);
            points[2].Input(out float z);
            result = new Vector3(x, y, z);
        }

        public static void Input(this string reason, out Vector2Int result)
        {
            var points = reason.Split(',');
            points[0].Input(out int x);
            points[1].Input(out int y);
            result = new Vector2Int(x, y);
        }

        public static void Input(this string reason, out Vector3Int result)
        {
            var points = reason.Split(',');
            points[0].Input(out int x);
            points[1].Input(out int y);
            points[2].Input(out int z);
            result = new Vector3Int(x, y, z);
        }

        public static void Input(this string reason, out Sprite result)
        {
            result = AssetDatabase.LoadAssetAtPath<Sprite>(reason);
        }

        public static void Input<T>(this string reason, out T result) where T : struct
        {
            result = default;
            if (!string.IsNullOrEmpty(reason))
            {
                if (typeof(T).IsEnum)
                {
                    Enum.TryParse(reason, out result);
                }
                else if (typeof(T).IsPrimitive)
                {
                    result = (T)reason.Parse(typeof(T));
                }
                else
                {
                    object obj = new T();
                    var fields = typeof(T).GetFields();
                    var members = reason.Split(',');
                    for (int i = 0; i < members.Length; ++i)
                    {
                        if (i < fields.Length)
                        {
                            var value = members[i].Parse(fields[i].FieldType);
                            fields[i].SetValue(obj, value);
                        }
                    }

                    result = (T)obj;
                }
            }
        }
    }

    public static partial class Extensions
    {
        public static void Input(this string reason, out SecretInt[] result)
        {
            var content = reason.Content(out result);
            for (int i = 0; i < content.Length; ++i)
            {
                content[i].Input(out result[i]);
            }
        }

        public static void Input(this string reason, out SecretBool[] result)
        {
            var content = reason.Content(out result);
            for (int i = 0; i < content.Length; ++i)
            {
                content[i].Input(out result[i]);
            }
        }

        public static void Input(this string reason, out SecretFloat[] result)
        {
            var content = reason.Content(out result);
            for (int i = 0; i < content.Length; ++i)
            {
                content[i].Input(out result[i]);
            }
        }

        public static void Input(this string reason, out SecretString[] result)
        {
            var content = reason.Content(out result);
            for (int i = 0; i < content.Length; ++i)
            {
                content[i].Input(out result[i]);
            }
        }
        
        public static void Input(this string reason, out string[] result)
        {
            var content = reason.Content(out result);
            for (int i = 0; i < content.Length; ++i)
            {
                content[i].Input(out result[i]);
            }
        }

        public static void Input(this string reason, out Vector2[] result)
        {
            var content = reason.Content(out result);
            for (int i = 0; i < content.Length; ++i)
            {
                content[i].Input(out result[i]);
            }
        }

        public static void Input(this string reason, out Vector3[] result)
        {
            var content = reason.Content(out result);
            for (int i = 0; i < content.Length; ++i)
            {
                content[i].Input(out result[i]);
            }
        }

        public static void Input(this string reason, out Vector2Int[] result)
        {
            var content = reason.Content(out result);
            for (int i = 0; i < content.Length; ++i)
            {
                content[i].Input(out result[i]);
            }
        }

        public static void Input(this string reason, out Vector3Int[] result)
        {
            var content = reason.Content(out result);
            for (int i = 0; i < content.Length; ++i)
            {
                content[i].Input(out result[i]);
            }
        }

        public static void Input(this string reason, out Sprite[] result)
        {
            var content = reason.Content(out result);
            for (int i = 0; i < content.Length; ++i)
            {
                content[i].Input(out result[i]);
            }
        }

        public static void Input<T>(this string reason, out T[] result) where T : struct
        {
            var content = reason.Content(out result);
            for (int i = 0; i < content.Length; ++i)
            {
                content[i].Input(out result[i]);
            }
        }
    }

    public static partial class Extensions
    {
        private static string[] Content<T>(this string reason, out T[] result)
        {
            result = default;
            if (!string.IsNullOrEmpty(reason))
            {
                if (reason.EndsWith(';'))
                {
                    reason = reason[..^1];
                }

                var splits = reason.Split(';');
                result = new T[splits.Length];
                return splits;
            }

            return Array.Empty<string>();
        }

        private static object Parse(this string reason, Type type)
        {
            try
            {
                if (type.IsEnum)
                {
                    return Enum.Parse(type, reason);
                }
                else
                {
                    return Convert.ChangeType(reason, type);
                }
            }
            catch
            {
                Debug.LogWarning("类型: " + type.Name + " 无效的转换: " + reason);
                return default;
            }
        }
    }
}
#endif