// *********************************************************************************
// # Project: Forest
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-12-03  13:53
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;

namespace JFramework
{
    /// <summary>
    /// 资源信息
    /// </summary>
    [Serializable]
    internal struct Asset
    {
        /// <summary>
        /// 资源名称
        /// </summary>
        public string asset;

        /// <summary>
        /// 资源标签
        /// </summary>
        public string bundle;

        /// <summary>
        /// 分割包名和资源名
        /// </summary>
        /// <param name="path"></param>
        public Asset(string path)
        {
            asset = path.Split('/')[1];
            bundle = path.Split('/')[0].ToLower();
        }
    }
}