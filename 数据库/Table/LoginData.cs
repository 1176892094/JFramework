// *********************************************************************************
// # Project: SQLServer
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-09-03 14:09:26
// # Recently: 2025-02-11 00:02:40
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;

namespace Runtime
{
    [Serializable]
    public struct SqlSettingManager
    {
        public string deviceId; // 设备标识
        public int archive; // 选择存档
        public int process; // 后处理
        public int quality; // 画质
        public int joystick; // 摇杆设置
        public int frameRate; // 帧数
        public int screenRate; // 分辨率
        public string version; // 当前版本
        public string platform; // 操作系统
        public List<int> handbook; // 图鉴物品
        public List<string> addresses; // 存储地址
        public int modifyData; // 修改数据
        public int coinCache; // 金币广告
        public int dustCache; // 粉尘广告
        public int userData; // 用户数据
        public string userId; // 用户Id
        public string createTime; // 创建时间
        public string loginTime; // 最新的登陆时间
        public string targetTime; // 登陆时间下一天
    }

    [Serializable]
    public struct SqlPlayerData
    {
        public string deviceId;
        public string playerId;
        public int modifyData;
        public int playerType;
        public int playerLevel;
        public int coinCache;
        public int coinCount;
        public int woodCache;
        public int woodCount;
        public int bossCount;
        public int enemyCount;
        public int chestCount;
        public int deathCount;
        public int towerCount;
        public List<SqlItem> bagItems;
        public List<SqlItem> equipItems;
        public List<SqlItem> storeItems;
        public List<SqlItem> skillItems;
    }
    
    [Serializable]
    public struct SqlItem
    {
        public long a;
        public long b;
        public long c;
    }
}