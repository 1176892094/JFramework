// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2025-01-08 17:01:37
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;

namespace JFramework
{
    [Serializable]
    internal struct PackData : IEquatable<PackData>
    {
        public string code;
        public string name;
        public int size;

        public PackData(string code, string name, int size)
        {
            this.code = code;
            this.name = name;
            this.size = size;
        }

        public static bool operator ==(PackData a, PackData b) => a.code == b.code;

        public static bool operator !=(PackData a, PackData b) => a.code != b.code;

        public bool Equals(PackData other)
        {
            return size == other.size && code == other.code && name == other.name;
        }

        public override bool Equals(object obj)
        {
            return obj is PackData other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (code != null ? code.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (name != null ? name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ size;
                return hashCode;
            }
        }
    }
}