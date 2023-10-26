// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-10-24  23:52
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;

// ReSharper disable All

namespace JFramework
{
    /// <summary>
    /// 资源数据
    /// </summary>
    internal struct AssetData
    {
        /// <summary>
        /// 资源编码
        /// </summary>
        public string code;

        /// <summary>
        /// 资源名称
        /// </summary>
        public string name;

        /// <summary>
        /// 资源大小
        /// </summary>
        public long size;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <param name="size"></param>
        public AssetData(string code, string name, long size)
        {
            this.code = code;
            this.name = name;
            this.size = size;
        }

        /// <summary>
        /// 重载运算符 ==
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(AssetData a, AssetData b)
        {
            return a.code == b.code;
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
            return size == other.size && code == other.code && name == other.name;
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
            return HashCode.Combine(size, code, name);
        }
    }
}