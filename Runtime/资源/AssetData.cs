using System;


namespace JFramework
{
    internal readonly struct AssetData
    {
        private readonly long size;
        private readonly string md5;
        private readonly string name;

        public AssetData(string name, string size, string md5)
        {
            this.md5 = md5;
            this.name = name;
            this.size = long.Parse(size);
        }
        
        public static bool operator ==(AssetData a, AssetData b)
        {
            return a.md5 == b.md5;
        }
        
        public static bool operator !=(AssetData a, AssetData b)
        {
            return !(a == b);
        }

        private bool Equals(AssetData other)
        {
            return size == other.size && md5 == other.md5 && name == other.name;
        }

        public override bool Equals(object obj)
        {
            return obj is AssetData other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(size, md5, name);
        }
    }
}