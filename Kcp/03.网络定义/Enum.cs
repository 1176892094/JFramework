// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-08 19:01:30
// # Recently: 2025-01-08 20:01:58
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

namespace JFramework
{
    internal enum Error : byte
    {
        DnsResolve = 1,       // 无法解析主机地址
        Timeout = 2,          // Ping链接超时
        Congestion = 3,       // 传输网络无法处理更多的消息
        InvalidReceive = 4,   // 接收到无效数据包（可能是故意攻击）
        InvalidSend = 5,      // 用户试图发送无效数据
        ConnectionClosed = 6, // 连接自动关闭或非自愿丢失
        Unexpected = 7        // 意外错误异常，需要修复
    }

    internal enum State : byte
    {
        Connect = 0,
        Connected = 1,
        Disconnect = 2
    }

    internal enum Reliable : byte
    {
        Connect = 1,
        Ping = 2,
        Data = 3,
    }

    internal enum Unreliable : byte
    {
        Data = 4,
        Disconnect = 5,
    }
    
    internal enum OpCodes : byte
    {
        Connect = 1,
        Connected = 2,
        JoinRoom = 3,
        CreateRoom = 4,
        UpdateRoom = 5,
        LeaveRoom = 6,
        UpdateData = 7,
        KickRoom = 8,
    }
}