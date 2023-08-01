using System;
using Newtonsoft.Json;


namespace JFramework
{
    /// <summary>
    /// 资源数据
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal readonly struct AssetData
    {
        /// <summary>
        /// 资源名称
        /// </summary>
        [JsonProperty] public readonly string name;

        /// <summary>
        /// 资源md5码
        /// </summary>
        [JsonProperty] private readonly string md5;

        /// <summary>
        /// 资源大小
        /// </summary>
        [JsonProperty] private readonly long size;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="name"></param>
        /// <param name="size"></param>
        /// <param name="md5"></param>
        public AssetData(string name, string size, string md5)
        {
            this.md5 = md5;
            this.name = name;
            this.size = long.Parse(size);
        }

        /// <summary>
        /// 重载运算符 ==
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(AssetData a, AssetData b)
        {
            return a.md5 == b.md5;
        }

        /// <summary>
        /// 重载运算符 !=
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(AssetData a, AssetData b)
        {
            return !(a == b);
        }

        /// <summary>
        /// 同类型比较
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        private bool Equals(AssetData other)
        {
            return size == other.size && md5 == other.md5 && name == other.name;
        }

        /// <summary>
        /// 不同类型比较
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj is AssetData other && Equals(other);
        }

        /// <summary>
        /// 哈希码
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(size, md5, name);
        }
    }
}