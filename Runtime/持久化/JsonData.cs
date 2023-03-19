using System;

namespace JFramework
{
    /// <summary>
    /// Json加密数据
    /// </summary>
    [Serializable]
    internal class JsonData
    {
        /// <summary>
        /// 加密数据的键值
        /// </summary>
        public byte[] key;

        /// <summary>
        /// 加密数据的向量
        /// </summary>
        public byte[] iv;
    }
}