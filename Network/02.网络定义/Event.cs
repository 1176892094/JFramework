// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-21 23:12:15
// # Recently: 2024-12-22 20:12:19
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Net;
using JFramework.Net;

namespace JFramework.Common
{
    public struct ServerConnectEvent : IEvent
    {
        public NetworkClient client { get; private set; }

        public ServerConnectEvent(NetworkClient client)
        {
            this.client = client;
        }
    }

    public struct ServerDisconnectEvent : IEvent
    {
        public NetworkClient client { get; private set; }

        public ServerDisconnectEvent(NetworkClient client)
        {
            this.client = client;
        }
    }

    public struct ServerReadyEvent : IEvent
    {
        public NetworkClient client { get; private set; }

        public ServerReadyEvent(NetworkClient client)
        {
            this.client = client;
        }
    }

    public struct ServerLoadSceneEvent : IEvent
    {
        public string sceneName { get; private set; }

        public ServerLoadSceneEvent(string sceneName)
        {
            this.sceneName = sceneName;
        }
    }

    public struct ServerLoadCompleteEvent : IEvent
    {
        public string sceneName { get; private set; }

        public ServerLoadCompleteEvent(string sceneName)
        {
            this.sceneName = sceneName;
        }
    }

    public struct ClientConnectEvent : IEvent
    {
    }

    public struct ClientDisconnectEvent : IEvent
    {
    }

    public struct ClientNotReadyEvent : IEvent
    {
    }

    public struct ClientLoadSceneEvent : IEvent
    {
        public string sceneName { get; private set; }

        public ClientLoadSceneEvent(string sceneName)
        {
            this.sceneName = sceneName;
        }
    }

    public struct ClientLoadCompleteEvent : IEvent
    {
        public string sceneName { get; private set; }

        public ClientLoadCompleteEvent(string sceneName)
        {
            this.sceneName = sceneName;
        }
    }

    public struct ServerResponseEvent : IEvent
    {
        public Uri uri { get; private set; }
        public IPEndPoint endPoint { get; private set; }

        public ServerResponseEvent(Uri uri, IPEndPoint endPoint)
        {
            this.uri = uri;
            this.endPoint = endPoint;
        }
    }

    public struct LobbyUpdateEvent : IEvent
    {
        public RoomData[] rooms { get; private set; }

        public LobbyUpdateEvent(RoomData[] rooms)
        {
            this.rooms = rooms;
        }
    }

    public struct LobbyDisconnectEvent : IEvent
    {
    }
    
    public struct PingUpdateEvent : IEvent
    {
        public double pingTime { get; private set; }

        public PingUpdateEvent(double pingTime)
        {
            this.pingTime = pingTime;
        }
    }
}