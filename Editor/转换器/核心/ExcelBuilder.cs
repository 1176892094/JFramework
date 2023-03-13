using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace JFramework
{
    internal static class ExcelBuilder
    {
        private static readonly List<StringBuilder> stringList = new List<StringBuilder>();

        /// <summary>
        /// 指定容器大小
        /// </summary>
        public static StringBuilder Borrow()
        {
            if (stringList.Count == 0)
            {
                return new StringBuilder(1024);
            }

            var first = stringList[0];
            stringList.RemoveAt(0);
            return first;
        }

        /// <summary>
        /// 返回容器字符串
        /// </summary>
        public static string Return(StringBuilder builder)
        {
            if (builder == null) return null;
            if (!stringList.Contains(builder))
            {
                stringList.Add(builder);
            }

            var str = builder.ToString();
            builder.Clear();
            return str;
        }

        /// <summary>
        /// 重制StringBuilder
        /// </summary>
        public static void Clear() => stringList.Clear();

        /// <summary>
        /// 转换数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IParser Parse(string name, string type)
        {
            try
            {
                type = type.Trim();
                if (IsNormalData(type))
                    return new ParseNormal(name, type);
                if (IsNormalArray(type))
                    return new ParseNormalArray(name, type);
                if (IsCustomData(type))
                    return new ParseCustom(name, type);
                if (IsCustomArray(type))
                    return new ParseCustomArray(name, type);
                if (!name.IsEmpty() && !type.IsEmpty())
                    Debug.LogError($"不能转换列: \"{name}\"   类型为: \"{type}\".");
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }

            return null;
        }

        /// <summary>
        /// 是否支持传入类型
        /// </summary>
        public static bool IsSupportedType(string type)
        {
            if (type.IsEmpty()) return false;
            var fixType = type.ToLower().Trim();
            if (IsNormalData(fixType)) return true;
            if (IsNormalArray(fixType)) return true;
            if (IsCustomData(fixType)) return true;
            return IsCustomArray(fixType);
        }

        /// <summary>
        /// 是否为主键
        /// </summary>
        public static bool GetKeyValue(string name, string type)
        {
            var key = name.ToLower().Trim();
            if (!key.EndsWith(EditorConst.Key)) return false;
            if (type.Equals("int") || type.Equals("string")) return true;
            Debug.LogError($"主键只支持int和string两种类型!");
            return false;
        }

        /// <summary>
        /// 判断数组中是否有元素和data相等
        /// </summary>
        public static bool IsNormalData(string data) => EditorConst.Array.Any(data.Equals);

        /// <summary>
        /// 判断是标准数组
        /// </summary>
        private static bool IsNormalArray(string data)
        {
            if (!data.EndsWith("[]")) return false;
            var index = data.IndexOf('[');
            var name = data.Substring(0, index);
            return !name.IsEmpty() && EditorConst.Array.Any(type => name.Equals(type));
        }

        /// <summary>
        /// 判断是自定义数据
        /// </summary>
        private static bool IsCustomData(string data) => data.StartsWith("{") && data.EndsWith("}");

        /// <summary>
        /// 判断是自定义数组
        /// </summary>
        private static bool IsCustomArray(string data) => data.StartsWith("{") && data.EndsWith("}[]");
    }
}