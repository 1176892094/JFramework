// *********************************************************************************
// # Project: SQLServer
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-11-25 19:11:18
// # Recently: 2025-02-11 00:02:40
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;

namespace JFramework.Net
{
    [Serializable]
    public struct LoginRequest
    {
        public string username;
        public string password;
        public SqlSettingManager settingManager;
        public SqlPlayerData playerData1;
        public SqlPlayerData playerData2;
        public SqlPlayerData playerData3;
        public SqlPlayerData playerData4;
    }

    [Serializable]
    public struct LoginResponse
    {
        public string userId;
        public int userData;
        public int errorCode;
    }
}