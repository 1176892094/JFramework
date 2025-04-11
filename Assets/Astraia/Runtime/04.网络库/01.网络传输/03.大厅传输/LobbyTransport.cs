// *********************************************************************************
// # Project: Astraia
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-21 23:12:50
// # Recently: 2024-12-22 20:12:21
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using Astraia.Common;
using UnityEngine;
using UnityEngine.Networking;

namespace Astraia.Net
{
    public sealed class LobbyTransport : Transport
    {
        public Transport transport;
        public RoomMode roomMode;
        public string roomName;
        public string roomData;
        public string serverId;
        public string serverKey;

        private readonly Dictionary<int, int> clients = new Dictionary<int, int>();
        private readonly Dictionary<int, int> players = new Dictionary<int, int>();
        private bool isClient;
        private bool isServer;
        private int objectId;
        private State state = State.Disconnect;

        private void Awake()
        {
            transport.OnClientConnect -= OnClientConnect;
            transport.OnClientDisconnect -= OnClientDisconnect;
            transport.OnClientReceive -= OnClientReceive;
            transport.OnClientConnect += OnClientConnect;
            transport.OnClientDisconnect += OnClientDisconnect;
            transport.OnClientReceive += OnClientReceive;

            void OnClientConnect()
            {
                state = State.Connect;
            }

            void OnClientDisconnect()
            {
                StopLobby();
            }

            void OnClientReceive(ArraySegment<byte> segment, int channel)
            {
                try
                {
                    OnMessageReceive(segment, channel);
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e.ToString());
                }
            }
        }

        private void OnDestroy()
        {
            StopLobby();
        }

        public void StartLobby()
        {
            if (state != State.Disconnect)
            {
                Debug.LogWarning("大厅服务器已经连接！");
                return;
            }

            transport.port = port;
            transport.address = address;
            transport.StartClient();
        }

        public void StopLobby()
        {
            if (state != State.Disconnect)
            {
                Debug.Log("停止大厅服务器。");
                state = State.Disconnect;
                clients.Clear();
                players.Clear();
                isServer = false;
                isClient = false;
                EventManager.Invoke(new LobbyDisconnect());
                transport.StopClient();
            }
        }

        public async void UpdateLobby()
        {
            if (state != State.Connected)
            {
                Debug.Log("您必须连接到大厅以请求房间列表!");
                return;
            }

            var uri = Service.Text.Format("http://{0}:{1}/api/compressed/servers", address, port);
            using var request = UnityWebRequest.Get(uri);
            await request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogWarning(Service.Text.Format("无法获取服务器列表: {0}:{1}", address, port));
                return;
            }

            var rooms = Service.Zip.Decompress(request.downloadHandler.text);
            var jsons = JsonManager.FromJson<RoomData[]>("{" + "\"value\":" + rooms + "}");
            EventManager.Invoke(new LobbyUpdate(jsons));
            Debug.Log("房间信息：" + rooms);
        }

        public void UpdateRoom()
        {
            if (isServer)
            {
                using var setter = MemorySetter.Pop();
                setter.SetByte((byte)OpCodes.UpdateRoom);
                setter.SetString(roomName);
                setter.SetString(roomData);
                setter.SetByte((byte)roomMode);
                setter.SetInt(NetworkManager.Instance.connection);
                transport.SendToServer(setter);
            }
        }

        private void OnMessageReceive(ArraySegment<byte> segment, int channel)
        {
            using var getter = MemoryGetter.Pop(segment);
            var opcode = (OpCodes)getter.GetByte();
            if (opcode == OpCodes.Connect)
            {
                using var setter = MemorySetter.Pop();
                setter.SetByte((byte)OpCodes.Connected);
                setter.SetString(serverKey);
                transport.SendToServer(setter);
            }
            else if (opcode == OpCodes.Connected)
            {
                state = State.Connected;
                UpdateLobby();
            }
            else if (opcode == OpCodes.CreateRoom)
            {
                serverId = getter.GetString();
            }
            else if (opcode == OpCodes.JoinRoom)
            {
                if (isServer)
                {
                    objectId++;
                    var clientId = getter.GetInt();
                    clients.Add(clientId, objectId);
                    players.Add(objectId, clientId);
                    OnServerConnect?.Invoke(objectId);
                }

                if (isClient)
                {
                    OnClientConnect?.Invoke();
                }
            }
            else if (opcode == OpCodes.LeaveRoom)
            {
                if (isClient)
                {
                    isClient = false;
                    OnClientDisconnect?.Invoke();
                }
            }
            else if (opcode == OpCodes.UpdateData)
            {
                var message = getter.GetArraySegment();
                if (isServer)
                {
                    var clientId = getter.GetInt();
                    if (clients.TryGetValue(clientId, out var playerId))
                    {
                        OnServerReceive?.Invoke(playerId, message, channel);
                    }
                }

                if (isClient)
                {
                    OnClientReceive?.Invoke(message, channel);
                }
            }
            else if (opcode == OpCodes.KickRoom)
            {
                if (isServer)
                {
                    var clientId = getter.GetInt();
                    if (clients.TryGetValue(clientId, out var playerId))
                    {
                        OnServerDisconnect?.Invoke(playerId);
                        clients.Remove(clientId);
                        players.Remove(playerId);
                    }
                }
            }
        }

        public override int SendLength(int channel)
        {
            return transport.SendLength(channel);
        }

        public override void SendToClient(int clientId, ArraySegment<byte> segment, int channel = Channel.Reliable)
        {
            if (players.TryGetValue(clientId, out var playerId))
            {
                using var setter = MemorySetter.Pop();
                setter.SetByte((byte)OpCodes.UpdateData);
                setter.SetArraySegment(segment);
                setter.SetInt(playerId);
                transport.SendToServer(setter);
            }
        }

        public override void SendToServer(ArraySegment<byte> segment, int channel = Channel.Reliable)
        {
            using var setter = MemorySetter.Pop();
            setter.SetByte((byte)OpCodes.UpdateData);
            setter.SetArraySegment(segment);
            setter.SetInt(0);
            transport.SendToServer(setter);
        }

        public override void StartServer()
        {
            if (state != State.Connected)
            {
                Debug.Log("没有连接到大厅!");
                return;
            }

            if (isClient || isServer)
            {
                Debug.Log("客户端或服务器已经连接!");
                return;
            }

            objectId = 0;
            clients.Clear();
            players.Clear();
            isServer = true;

            using var setter = MemorySetter.Pop();
            setter.SetByte((byte)OpCodes.CreateRoom);
            setter.SetString(roomName);
            setter.SetString(roomData);
            setter.SetInt(NetworkManager.Instance.connection);
            setter.SetByte((byte)roomMode);
            transport.SendToServer(setter);
        }

        public override void StopServer()
        {
            if (isServer)
            {
                isServer = false;
                using var setter = MemorySetter.Pop();
                setter.SetByte((byte)OpCodes.LeaveRoom);
                transport.SendToServer(setter);
            }
        }

        public override void StopClient(int clientId)
        {
            if (players.TryGetValue(clientId, out var playerId))
            {
                using var setter = MemorySetter.Pop();
                setter.SetByte((byte)OpCodes.KickRoom);
                setter.SetInt(playerId);
                transport.SendToServer(setter);
            }
        }

        public override void StartClient()
        {
            if (state != State.Connected)
            {
                Debug.Log("没有连接到大厅！");
                OnClientDisconnect?.Invoke();
                return;
            }

            if (isClient || isServer)
            {
                Debug.Log("客户端或服务器已经连接!");
                return;
            }

            isClient = true;
            using var setter = MemorySetter.Pop();
            setter.SetByte((byte)OpCodes.JoinRoom);
            setter.SetString(transport.address);
            transport.SendToServer(setter);
        }

        public override void StartClient(Uri uri)
        {
            if (uri != null)
            {
                address = uri.Host;
            }

            StartClient();
        }

        public override void StopClient()
        {
            if (state != State.Disconnect)
            {
                isClient = false;
                using var setter = MemorySetter.Pop();
                setter.SetByte((byte)OpCodes.LeaveRoom);
                transport.SendToServer(setter);
            }
        }

        public override void ClientEarlyUpdate()
        {
            transport.ClientEarlyUpdate();
        }

        public override void ClientAfterUpdate()
        {
            transport.ClientAfterUpdate();
        }

        public override void ServerEarlyUpdate()
        {
        }

        public override void ServerAfterUpdate()
        {
        }
    }

    [Serializable]
    public struct RoomData
    {
        /// <summary>
        /// 房间拥有者
        /// </summary>
        public int clientId;

        /// <summary>
        /// 是否显示
        /// </summary>
        public RoomMode roomMode;

        /// <summary>
        /// 房间最大人数
        /// </summary>
        public int maxCount;

        /// <summary>
        /// 额外房间数据
        /// </summary>
        public string roomData;

        /// <summary>
        /// 房间Id
        /// </summary>
        public string roomId;

        /// <summary>
        /// 房间名称
        /// </summary>
        public string roomName;

        /// <summary>
        /// 客户端数量
        /// </summary>
        public int[] clients;
    }
}