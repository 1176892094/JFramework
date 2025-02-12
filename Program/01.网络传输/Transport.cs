// *********************************************************************************
// # Project: JFramework.Lobby
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-08-28 20:08:49
// # Recently: 2024-12-23 00:12:10
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using JFramework.Udp;

namespace JFramework.Net
{
    internal sealed class Transport
    {
        public string address = "localhost";
        public ushort port = 20974;
        public int maxUnit = 1200;
        public uint timeout = 10000;
        public uint interval = 10;
        public uint deadLink = 40;
        public uint fastResend = 2;
        public uint sendWindow = 1024 * 4;
        public uint receiveWindow = 1024 * 4;

        private Client client;
        private Server server;

        public Action OnClientConnect { get; set; }
        public Action OnClientDisconnect { get; set; }
        public Action<int, string> OnClientError { get; set; }
        public Action<ArraySegment<byte>, int> OnClientReceive { get; set; }
        public Action<int> OnServerConnect { get; set; }
        public Action<int> OnServerDisconnect { get; set; }
        public Action<int, int, string> OnServerError { get; set; }
        public Action<int, ArraySegment<byte>, int> OnServerReceive { get; set; }

        public Transport()
        {
            var setting = new Udp.Setting(maxUnit, timeout, interval, deadLink, fastResend, sendWindow, receiveWindow);
            client = new Client(setting, ClientConnect, ClientDisconnect, ClientError, ClientReceive);
            server = new Server(setting, ServerConnect, ServerDisconnect, ServerError, ServerReceive);
            return;

            void ClientConnect()
            {
                OnClientConnect.Invoke();
            }

            void ClientDisconnect()
            {
                OnClientDisconnect.Invoke();
            }

            void ClientError(int error, string message)
            {
                OnClientError?.Invoke(error, message);
            }

            void ClientReceive(ArraySegment<byte> message, int channel)
            {
                OnClientReceive.Invoke(message, channel);
            }

            void ServerConnect(int clientId)
            {
                OnServerConnect.Invoke(clientId);
            }

            void ServerDisconnect(int clientId)
            {
                OnServerDisconnect.Invoke(clientId);
            }

            void ServerError(int clientId, int error, string message)
            {
                OnServerError?.Invoke(clientId, error, message);
            }

            void ServerReceive(int clientId, ArraySegment<byte> message, int channel)
            {
                OnServerReceive.Invoke(clientId, message, channel);
            }
        }

        public void Update()
        {
            server.EarlyUpdate();
            server.AfterUpdate();
        }

        public int MessageSize(int channel)
        {
            return channel == Channel.Reliable ? Agent.ReliableSize(maxUnit, receiveWindow) : Agent.UnreliableSize(maxUnit);
        }

        public void StartServer()
        {
            server.Connect(port);
        }

        public void StopServer()
        {
            server.StopServer();
        }

        public void StopClient(int clientId)
        {
            server.Disconnect(clientId);
        }

        public void SendToClient(int clientId, ArraySegment<byte> segment, int channel = Channel.Reliable)
        {
            server.Send(clientId, segment, channel);
        }

        public void StartClient()
        {
            client.Connect(address, port);
        }

        public void StartClient(Uri uri)
        {
            client.Connect(uri.Host, (ushort)(uri.IsDefaultPort ? port : uri.Port));
        }

        public void StopClient()
        {
            client.Disconnect();
        }

        public void SendToServer(ArraySegment<byte> segment, int channel = Channel.Reliable)
        {
            client.Send(segment, channel);
        }

        public void ClientEarlyUpdate()
        {
            client.EarlyUpdate();
        }

        public void ClientAfterUpdate()
        {
            client.AfterUpdate();
        }

        public void ServerEarlyUpdate()
        {
            server.EarlyUpdate();
        }

        public void ServerAfterUpdate()
        {
            server.AfterUpdate();
        }
    }
}