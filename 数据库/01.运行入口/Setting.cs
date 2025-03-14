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

namespace JFramework.Net
{
    public static class Setting
    {
        public const string ADDRESS = "localhost";
        public const string DATABASE = "Forest";
        public const string USERNAME = "root";
        public const string PASSWORD = "jinyijie";

        public static string GetConnection(string username, string password)
        {
            if (username != USERNAME || password != PASSWORD)
            {
                return null;
            }

            return $"Server={ADDRESS};Database={DATABASE};User ID={username};Password={password};";
        }
    }
}