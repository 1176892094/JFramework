// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: jinyijie
// # Version: 1.0.0
// # History: 2024-4-4  18:2
// # Copyright: 2024, jinyijie
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using UnityEngine;

namespace JFramework
{
    public static partial class Extensions
    {
        private static readonly Dictionary<Type, Delegate> parse = new Dictionary<Type, Delegate>()
        {
            { typeof(string), new Func<string, string>(InputString) },
            { typeof(Vector2), new Func<string, Vector2>(InputVector2) },
            { typeof(Vector3), new Func<string, Vector3>(InputVector3) },
            { typeof(Vector2Int), new Func<string, Vector2Int>(InputVector2Int) },
            { typeof(Vector3Int), new Func<string, Vector3Int>(InputVector3Int) },
        };

        private static readonly Dictionary<Type, Delegate> parseArray = new Dictionary<Type, Delegate>()
        {
            { typeof(string[]), new Func<string, string[]>(InputStringArray) },
            { typeof(Vector2[]), new Func<string, Vector2[]>(InputVector2Array) },
            { typeof(Vector3[]), new Func<string, Vector3[]>(InputVector3Array) },
            { typeof(Vector2Int[]), new Func<string, Vector2Int[]>(InputVector2IntArray) },
            { typeof(Vector3Int[]), new Func<string, Vector3Int[]>(InputVector3IntArray) },
        };

        public static T Parse<T>(this SecretString reason)
        {
            if (parse.TryGetValue(typeof(T), out var func))
            {
                return (T)func.DynamicInvoke(reason.Value);
            }

            return reason.Value.InputGeneric<T>();
        }

        public static T[] Array<T>(this SecretString reason)
        {
            if (parseArray.TryGetValue(typeof(T[]), out var func))
            {
                return (T[])func.DynamicInvoke(reason.Value);
            }

            return reason.Value.InputGenericArray<T>();
        }

        public static void Input(this string reason, out SecretString result)
        {
            result = reason ?? string.Empty;
        }

#if UNITY_EDITOR
        public static void Input(this string reason, out Sprite result)
        {
            result = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(reason);
        }

        public static void Input(this string reason, out Sprite[] result)
        {
            var content = reason.Content(out result);
            for (int i = 0; i < content.Length; ++i)
            {
                content[i].Input(out result[i]);
            }
        }
#endif

        public static string InputString(this string reason)
        {
            return reason ?? string.Empty;
        }

        public static Vector2 InputVector2(this string reason)
        {
            var points = reason.Split(',');
            var x = float.Parse(points[0]);
            var y = float.Parse(points[1]);
            return new Vector2(x, y);
        }

        public static Vector3 InputVector3(this string reason)
        {
            var points = reason.Split(',');
            var x = float.Parse(points[0]);
            var y = float.Parse(points[1]);
            var z = float.Parse(points[2]);
            return new Vector3(x, y, z);
        }

        public static Vector2Int InputVector2Int(this string reason)
        {
            var points = reason.Split(',');
            var x = int.Parse(points[0]);
            var y = int.Parse(points[1]);
            return new Vector2Int(x, y);
        }

        public static Vector3Int InputVector3Int(this string reason)
        {
            var points = reason.Split(',');
            var x = int.Parse(points[0]);
            var y = int.Parse(points[1]);
            var z = int.Parse(points[2]);
            return new Vector3Int(x, y, z);
        }

        public static T InputGeneric<T>(this string reason)
        {
            if (!string.IsNullOrEmpty(reason))
            {
                var type = typeof(T);
                if (type.IsEnum)
                {
                    return (T)Enum.Parse(type, reason);
                }

                if (type.IsPrimitive)
                {
                    return (T)Convert.ChangeType(reason, type);
                }

                var obj = Activator.CreateInstance(type);
                var fields = type.GetFields(Reflection.Instance);
                var members = reason.Split(',');
                for (int i = 0; i < fields.Length; i++)
                {
                    fields[i].SetValue(obj, new SecretString(members[i]));
                }

                return (T)obj;
            }

            return default;
        }

        public static string[] InputStringArray(this string reason)
        {
            var content = reason.Content(out string[] result);
            for (int i = 0; i < content.Length; ++i)
            {
                result[i] = content[i].InputString();
            }

            return result;
        }

        public static Vector2[] InputVector2Array(this string reason)
        {
            var content = reason.Content(out Vector2[] result);
            for (int i = 0; i < content.Length; ++i)
            {
                result[i] = content[i].InputVector2();
            }

            return result;
        }

        public static Vector3[] InputVector3Array(this string reason)
        {
            var content = reason.Content(out Vector3[] result);
            for (int i = 0; i < content.Length; ++i)
            {
                result[i] = content[i].InputVector3();
            }

            return result;
        }

        public static Vector2Int[] InputVector2IntArray(this string reason)
        {
            var content = reason.Content(out Vector2Int[] result);
            for (int i = 0; i < content.Length; ++i)
            {
                result[i] = content[i].InputVector2Int();
            }

            return result;
        }

        public static Vector3Int[] InputVector3IntArray(this string reason)
        {
            var content = reason.Content(out Vector3Int[] result);
            for (int i = 0; i < content.Length; ++i)
            {
                result[i] = content[i].InputVector3Int();
            }

            return result;
        }
        
        public static T[] InputGenericArray<T>(this string reason)
        {
            var content = reason.Content(out T[] result);
            for (int i = 0; i < content.Length; ++i)
            {
                result[i] = content[i].InputGeneric<T>();
            }

            return result;
        }

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

            return new string[0];
        }
    }
}