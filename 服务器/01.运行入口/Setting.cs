// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-10 21:01:21
// # Recently: 2025-01-10 21:01:33
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;

namespace JFramework.Net
{
    [Serializable]
    internal class Setting
    {
        /// <summary>
        /// 服务器密钥
        /// </summary>
        public string ServerKey = Guid.NewGuid().ToString();

        /// <summary>
        /// Rest服务器端口
        /// </summary>
        public ushort RestPort = 8080;

        /// <summary>
        /// 主线程循环时间
        /// </summary>
        public int UpdateTime = 10;

        /// <summary>
        /// 是否请求服务器列表
        /// </summary>
        public bool RequestRoom = true;

        /// <summary>
        /// 是否启用Rest服务
        /// </summary>
        public bool UseEndPoint = true;
    }
}