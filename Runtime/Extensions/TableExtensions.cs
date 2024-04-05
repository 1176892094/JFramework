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
using System.Text;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace JFramework
{
    public static partial class Extensions
    {
        private const byte DATA_OFFSET = 1 << 7;

        public static void Serialize(this string reason, out byte[] offset, out byte[] result)
        {
            offset = default;
            result = default;
            if (!string.IsNullOrEmpty(reason))
            {
                result = Encoding.UTF8.GetBytes(reason);
                offset = new byte[result.Length];
                Buffer.BlockCopy(result, 0, offset, 0, result.Length);
                for (int i = 0; i < offset.Length; i++)
                {
                    offset[i] += DATA_OFFSET;
                }
            }
        }

        public static string Deserialize(this byte[] reason, byte[] result)
        {
            if (reason == null || result == null || reason.Length != result.Length)
            {
                return "";
            }

            var offset = new byte[result.Length];
            Buffer.BlockCopy(result, 0, offset, 0, result.Length);
            for (int i = 0; i < offset.Length; i++)
            {
                offset[i] -= DATA_OFFSET;
            }

            var target = Encoding.UTF8.GetString(offset);
            if (target != Encoding.UTF8.GetString(reason))
            {
                Secret.AntiCheat();
            }

            return target;
        }

#if UNITY_EDITOR
        public static bool Input(this string reason, out string result)
        {
            result = reason ?? string.Empty;
            return true;
        }

        public static bool Input(this string reason, out Vector2 result)
        {
            var points = reason.Split(',');
            points[0].Input(out float x);
            points[1].Input(out float y);
            result = new Vector2(x, y);
            return true;
        }

        public static bool Input(this string reason, out Vector3 result)
        {
            var points = reason.Split(',');
            points[0].Input(out float x);
            points[1].Input(out float y);
            points[2].Input(out float z);
            result = new Vector3(x, y, z);
            return true;
        }

        public static bool Input(this string reason, out Vector2Int result)
        {
            var points = reason.Split(',');
            points[0].Input(out int x);
            points[1].Input(out int y);
            result = new Vector2Int(x, y);
            return true;
        }

        public static bool Input(this string reason, out Vector3Int result)
        {
            var points = reason.Split(',');
            points[0].Input(out int x);
            points[1].Input(out int y);
            points[2].Input(out int z);
            result = new Vector3Int(x, y, z);
            return true;
        }

        public static bool Input(this string reason, out Sprite result)
        {
            result = AssetDatabase.LoadAssetAtPath<Sprite>(reason);
            return true;
        }

        public static bool Input<T>(this string reason, out T result) where T : struct
        {
            result = default;
            if (!string.IsNullOrEmpty(reason))
            {
                if (typeof(T).IsEnum)
                {
                    result = Enum.Parse<T>(reason);
                    return true;
                }

                if (typeof(T).IsPrimitive)
                {
                    result = (T)Convert.ChangeType(reason, typeof(T));
                    return true;
                }

                object obj = new T();
                var fields = typeof(T).GetFields(Reflection.Instance);
                var members = reason.Split(',');
                for (int i = 0; i < fields.Length; i += 2)
                {
                    var target = Encoding.UTF8.GetBytes(members[i / 2]);
                    var offset = new byte[target.Length];
                    Buffer.BlockCopy(target, 0, offset, 0, target.Length);
                    for (int j = 0; j < offset.Length; j++)
                    {
                        offset[j] += DATA_OFFSET;
                    }

                    fields[i].SetValue(obj, target);
                    fields[i + 1].SetValue(obj, offset);
                }

                result = (T)obj;
                return true;
            }

            return false;
        }

        public static bool Input(this string reason, out string[] result)
        {
            var content = reason.Content(out result);
            for (int i = 0; i < content.Length; ++i)
            {
                content[i].Input(out result[i]);
            }

            return true;
        }

        public static bool Input(this string reason, out Vector2[] result)
        {
            var content = reason.Content(out result);
            for (int i = 0; i < content.Length; ++i)
            {
                content[i].Input(out result[i]);
            }

            return true;
        }

        public static bool Input(this string reason, out Vector3[] result)
        {
            var content = reason.Content(out result);
            for (int i = 0; i < content.Length; ++i)
            {
                content[i].Input(out result[i]);
            }

            return true;
        }

        public static bool Input(this string reason, out Vector2Int[] result)
        {
            var content = reason.Content(out result);
            for (int i = 0; i < content.Length; ++i)
            {
                content[i].Input(out result[i]);
            }

            return true;
        }

        public static bool Input(this string reason, out Vector3Int[] result)
        {
            var content = reason.Content(out result);
            for (int i = 0; i < content.Length; ++i)
            {
                content[i].Input(out result[i]);
            }

            return true;
        }

        public static bool Input(this string reason, out Sprite[] result)
        {
            var content = reason.Content(out result);
            for (int i = 0; i < content.Length; ++i)
            {
                content[i].Input(out result[i]);
            }

            return true;
        }

        public static bool Input<T>(this string reason, out T[] result) where T : struct
        {
            var content = reason.Content(out result);
            for (int i = 0; i < content.Length; ++i)
            {
                content[i].Input(out result[i]);
            }

            return true;
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
#endif
    }
}