// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-4-4  18:2
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JFramework
{
    public static partial class Extensions
    {
        private static readonly Dictionary<Type, Delegate> parsers = new Dictionary<Type, Delegate>()
        {
            { typeof(string), new Func<string, string>(InputString) },
            { typeof(Vector2), new Func<string, Vector2>(InputVector2) },
            { typeof(Vector3), new Func<string, Vector3>(InputVector3) },
            { typeof(Vector2Int), new Func<string, Vector2Int>(InputVector2Int) },
            { typeof(Vector3Int), new Func<string, Vector3Int>(InputVector3Int) },
            { typeof(string[]), new Func<string, string[]>(InputStringArray) },
            { typeof(Vector2[]), new Func<string, Vector2[]>(InputVector2Array) },
            { typeof(Vector3[]), new Func<string, Vector3[]>(InputVector3Array) },
            { typeof(Vector2Int[]), new Func<string, Vector2Int[]>(InputVector2IntArray) },
            { typeof(Vector3Int[]), new Func<string, Vector3Int[]>(InputVector3IntArray) },
        };

        public static T Parse<T>(this Variable<string> reason)
        {
            if (parsers.TryGetValue(typeof(T), out var func))
            {
                return ((Func<string, T>)func).Invoke(reason.Value);
            }

            return reason.Value.InputGeneric<T>();
        }

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

        private static List<string> InputArray(this string reason)
        {
            var result = new List<string>();
            if (!string.IsNullOrEmpty(reason))
            {
                if (reason.EndsWith(';'))
                {
                    reason = reason[..^1];
                }

                result.AddRange(reason.Split(';'));
            }

            return result;
        }

        public static string[] InputStringArray(this string reason)
        {
            return reason.InputArray().Select(InputString).ToArray();
        }

        public static Vector2[] InputVector2Array(this string reason)
        {
            return reason.InputArray().Select(InputVector2).ToArray();
        }

        public static Vector3[] InputVector3Array(this string reason)
        {
            return reason.InputArray().Select(InputVector3).ToArray();
        }

        public static Vector2Int[] InputVector2IntArray(this string reason)
        {
            return reason.InputArray().Select(InputVector2Int).ToArray();
        }

        public static Vector3Int[] InputVector3IntArray(this string reason)
        {
            return reason.InputArray().Select(InputVector3Int).ToArray();
        }

        public static T InputGeneric<T>(this string reason)
        {
            if (!typeof(T).IsArray)
            {
                var result = InputGeneric(reason, typeof(T));
                if (result != null)
                {
                    return (T)result;
                }

                return default;
            }

            if (string.IsNullOrEmpty(reason))
            {
                return default;
            }

            if (reason.EndsWith(';'))
            {
                reason = reason[..^1];
            }

            var members = reason.Split(';');
            var element = typeof(T).GetElementType();
            if (element != null)
            {
                var instance = Array.CreateInstance(element, members.Length);
                for (int i = 0; i < members.Length; ++i)
                {
                    var result = InputGeneric(members[i], element);
                    instance.SetValue(result, i);
                }

                return (T)(object)instance;
            }

            return default;
        }

        public static object InputGeneric(this string reason, Type type)
        {
            if (!string.IsNullOrEmpty(reason))
            {
                if (type.IsEnum)
                {
                    return Enum.Parse(type, reason);
                }

                if (type.IsPrimitive)
                {
                    return Convert.ChangeType(reason, type);
                }

                var member = reason.Split(',');
                var result = Activator.CreateInstance(type);
                var fields = type.GetFields(Reflection.Instance);
                for (int i = 0; i < fields.Length; i++)
                {
                    fields[i].SetValue(result, new Variable<string>(member[i]));
                }

                return result;
            }

            return default;
        }
    }
}