// *********************************************************************************
// # Project: SQLServer
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-09-01 21:09:14
// # Recently: 2025-02-11 00:02:13
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
        /// 连接地址
        /// </summary>
        public string Address = "localhost";
        
        /// <summary>
        /// 数据库
        /// </summary>
        public string Database = "Forest";
        
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username = "root";
        
        /// <summary>
        /// 密码
        /// </summary>
        public string Password = "jinyijie";

        /// <summary>
        /// Rest服务器端口
        /// </summary>
        public ushort RestPort = 20974;
        
        /// <summary>
        /// 是否启用Rest服务
        /// </summary>
        public bool UseEndPoint = true;
        
        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <param name="username">验证用户名</param>
        /// <param name="password">验证密码</param>
        /// <returns></returns>
        public string GetConnection(string username, string password)
        {
            if (username != Username || password != Password)
            {
                return null;
            }

            return $"Server={Address};Database={Database};User ID={username};Password={password};";
        }
    }
}