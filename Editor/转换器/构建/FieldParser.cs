using System;
using UnityEngine;

namespace JFramework
{
    internal static class FieldParser
    {
        public static DataField Parse(int id, string name, string type)
        {
            try
            {
                type = type.Trim();
                if (IsSystemType(type))
                    return new ExcelSystemType(id, name, type);
                if (IsSystemArray(type))
                    return new ExcelSystemArray(id, name, type);
                if (IsSystemDictionary(type))
                    return new ExcelSystemDict(id, name, type);
                if (IsCustomType(type))
                    return new CustomData(id, name, type);
                if (IsCustomArray(type))
                    return new CustomDataArray(id, name, type);
                if (IsCustomDictionary(type))
                    return new CustomDataDictionary(id, name, type);
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(type))
                    Debug.LogError($"不能转换列: \"{name}\"   类型为: \"{type}\".");
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }

            return null;
        }

        public static bool IsSupportedType(string type)
        {
            if (string.IsNullOrEmpty(type)) return false;
            var realType = type.ToLower().Trim();
            if (IsSystemType(realType)) return true;
            if (IsSystemArray(realType)) return true;
            if (IsSystemDictionary(realType)) return true;
            if (IsCustomType(realType)) return true;
            if (IsCustomArray(realType)) return true;
            if (IsCustomDictionary(realType)) return true;
            return false;
        }

        public static bool GetKeyValue(string name, string type)
        {
            string keyName = name.ToLower().Trim();
            if (!keyName.EndsWith(ExcelType.KeyValue)) return false;
            if (type.Equals("int") || type.Equals("string")) return true;
            Debug.LogError($"主键只支持int和string两种类型!");
            return false;
        }

        public static bool IsSystemType(string data)
        {
            foreach (var type in ExcelType.TypeArray)
            {
                if (data.Equals(type)) return true;
            }

            return false;
        }

        private static bool IsSystemArray(string data)
        {
            if (!data.EndsWith("[]")) return false;
            int index = data.IndexOf('[');
            string name = data.Substring(0, index);
            if (string.IsNullOrEmpty(name)) return false;
            foreach (var type in ExcelType.TypeArray)
            {
                if (name.Equals(type)) return true;
            }

            return false;
        }

        private static bool IsSystemDictionary(string data)
        {
            if (!data.StartsWith("<") || !data.EndsWith(">")) return false;
            if (data.Contains("{") || data.Contains("}")) return false;
            int startIndex = data.IndexOf('<');
            int sepIndex = data.IndexOf(',');
            int endIndex = data.IndexOf('>');
            return startIndex == 0 && sepIndex > 0 && endIndex > sepIndex;
        }

        private static bool IsCustomType(string data)
        {
            return data.StartsWith("{") && data.EndsWith("}");
        }

        private static bool IsCustomArray(string data)
        {
            return data.StartsWith("{") && data.EndsWith("}[]");
        }

        private static bool IsCustomDictionary(string data)
        {
            if (!data.StartsWith("<") || !data.EndsWith("}>")) return false;
            return data.Contains(",{") || data.Contains(", {");
        }
    }
}