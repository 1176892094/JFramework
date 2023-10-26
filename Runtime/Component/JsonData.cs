// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-10-25  00:01
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;

namespace JFramework
{
    /// <summary>
    /// Json加密数据
    /// </summary>
    [Serializable]
    internal struct JsonData
    {
        /// <summary>
        /// 加密数据的键值
        /// </summary>
        public byte[] key;

        /// <summary>
        /// 加密数据的向量
        /// </summary>
        public byte[] iv;

        /// <summary>
        /// 构造密钥
        /// </summary>
        /// <param name="key">密钥</param>
        /// <param name="iv">向量</param>
        public JsonData(byte[] key, byte[] iv)
        {
            this.key = key;
            this.iv = iv;
        }

        /// <summary>
        /// 隐式转换成 bool
        /// </summary>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        public static implicit operator bool(JsonData jsonData)
        {
            if (jsonData.key == null) return false;
            if (jsonData.key.Length == 0) return false;
            if (jsonData.iv == null) return false;
            return jsonData.iv.Length != 0;
        }
    }
}