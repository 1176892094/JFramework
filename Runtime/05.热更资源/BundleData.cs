// *********************************************************************************
// # Project: Forest
// # Unity: 2022.3.5f1c1
// # Author: jinyijie
// # Version: 1.0.0
// # History: 2024-08-25  01:08
// # Copyright: 2024, jinyijie
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;

namespace JFramework
{
    [Serializable]
    internal struct BundleData
    {
        public string code;
        public string name;
        public string size;

        public BundleData(string code, string name, string size)
        {
            this.code = code;
            this.name = name;
            this.size = size;
        }

        public static bool operator ==(BundleData a, BundleData b) => a.code == b.code;

        public static bool operator !=(BundleData a, BundleData b) => a.code != b.code;

        private bool Equals(BundleData other) => size == other.size && code == other.code && name == other.name;

        public override bool Equals(object obj) => obj is BundleData other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(size, code, name);
    }
}