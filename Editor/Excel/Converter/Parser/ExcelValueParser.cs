using System;
using JFramework.Basic;

namespace JFramework.Excel
{
    internal static class ExcelValueParser
    {
        private const string KeyValue = ":key";

        public static ExcelBaseType Parse(int id, string name, string type)
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
                    return new ExcelCustomType(id, name, type);
                if (IsCustomArray(type))
                    return new ExcelCustomArray(id, name, type);
                if (IsCustomDictionary(type))
                    return new ExcelCustomDict(id, name, type);
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(type))
                    Logger.LogError($"Failed to parse column \"{name}\" with type \"{type}\".");
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
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

        public static bool IsKeyValue(string name, string type)
        {
            string keyName = name.ToLower().Trim();
            if (!keyName.EndsWith(KeyValue)) return false;
            if (type.Equals("int") || type.Equals("string")) return true;
            Logger.LogError($"Only columns with type int or string can be key column, but {name}'s type is {type}.");
            return false;
        }

        public static bool IsSystemType(string lowerRawType)
        {
            foreach (var type in ExcelType.TypeArray)
            {
                if (lowerRawType.Equals(type))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsSystemArray(string lowerRawType)
        {
            if (!lowerRawType.EndsWith("[]"))
            {
                return false;
            }

            int startIndex = lowerRawType.IndexOf('[');
            string typeName = lowerRawType.Substring(0, startIndex);
            if (string.IsNullOrEmpty(typeName))
            {
                return false;
            }

            foreach (var type in ExcelType.TypeArray)
            {
                if (typeName.Equals(type)) return true;
            }

            return false;
        }

        private static bool IsSystemDictionary(string lowerRawType)
        {
            if (!lowerRawType.StartsWith("<") || !lowerRawType.EndsWith(">") || lowerRawType.Contains("{") || lowerRawType.Contains("}"))
            {
                return false;
            }

            int startIndex = lowerRawType.IndexOf('<');
            int sepIndex = lowerRawType.IndexOf(',');
            int endIndex = lowerRawType.IndexOf('>');
            return startIndex == 0 && sepIndex > 0 && endIndex > sepIndex;
        }

        private static bool IsCustomType(string lowerRawType)
        {
            return lowerRawType.StartsWith("{") && lowerRawType.EndsWith("}");
        }

        private static bool IsCustomArray(string lowerRawType)
        {
            return lowerRawType.StartsWith("{") && lowerRawType.EndsWith("}[]");
        }

        private static bool IsCustomDictionary(string lowerRawType)
        {
            if (!lowerRawType.StartsWith("<") || !lowerRawType.EndsWith("}>"))
            {
                return false;
            }

            return lowerRawType.Contains(",{") || lowerRawType.Contains(", {");
        }
    }
    
    internal struct ExcelType
    {
        public const string Int = "int";
        public const string Bool = "bool";
        public const string Long = "long";
        public const string Float = "float";
        public const string String = "string";
        public const string Double = "double";
        public static readonly string[] TypeArray = { Int, Bool, Long, Float, Double, String };
    }
}