// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-10 21:01:21
// # Recently: 2025-01-10 21:01:31
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;

namespace JFramework.Net
{
    internal class Process
    {
        private readonly Dictionary<string, Room> rooms = new Dictionary<string, Room>();
        private readonly Dictionary<int, Room> clients = new Dictionary<int, Room>();
        private readonly HashSet<int> connections = new HashSet<int>();
        private readonly Random random = new Random();
        private readonly Transport transport;

        public Process(Transport transport)
        {
            this.transport = transport;
        }

        public List<Room> roomInfo => rooms.Values.ToList();

        public void ServerError(int clientId, int error, string message)
        {
            string reason;
            switch (error)
            {
                case 1:
                    return;
                case 2:
                    return;
                case 3:
                    reason = "Congestion";
                    break;
                case 4:
                    reason = "InvalidReceive";
                    break;
                case 5:
                    reason = "InvalidSend";
                    break;
                case 6:
                    reason = "ConnectionClosed";
                    break;
                default:
                    reason = "Unexpected";
                    break;
            }

            Log.Warn(Service.Text.Format("客户端: {0}  错误代码: {1}\n{2}", clientId, reason, message));
        }

        public void ServerConnect(int clientId)
        {
            connections.Add(clientId);
            using var writer = MemoryWriter.Pop();
            writer.WriteByte((byte)OpCodes.Connect);
            transport.SendToClient(clientId, writer);
        }

        public void ServerDisconnect(int clientId)
        {
            var copies = rooms.Values.ToList();
            foreach (var room in copies)
            {
                if (room.clientId == clientId) // 主机断开
                {
                    using var writer = MemoryWriter.Pop();
                    writer.WriteByte((byte)OpCodes.LeaveRoom);
                    foreach (var client in room.clients)
                    {
                        transport.SendToClient(client, writer);
                        clients.Remove(client);
                    }

                    room.clients.Clear();
                    rooms.Remove(room.roomId);
                    clients.Remove(clientId);
                    return;
                }

                if (room.clients.Remove(clientId)) // 客户端断开
                {
                    using var writer = MemoryWriter.Pop();
                    writer.WriteByte((byte)OpCodes.KickRoom);
                    writer.WriteInt(clientId);
                    transport.SendToClient(room.clientId, writer);
                    clients.Remove(clientId);
                    break;
                }
            }
        }

        public void ServerReceive(int clientId, ArraySegment<byte> segment, int channel)
        {
            try
            {
                using var reader = MemoryReader.Pop(segment);
                var opcode = (OpCodes)reader.ReadByte();
                if (opcode == OpCodes.Connected)
                {
                    if (connections.Contains(clientId))
                    {
                        var serverKey = reader.ReadString();
                        if (serverKey == Program.Setting.ServerKey)
                        {
                            using var writer = MemoryWriter.Pop();
                            writer.WriteByte((byte)OpCodes.Connected);
                            transport.SendToClient(clientId, writer);
                        }

                        connections.Remove(clientId);
                    }
                }
                else if (opcode == OpCodes.CreateRoom)
                {
                    ServerDisconnect(clientId);
                    string id;
                    do
                    {
                        id = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ", 5).Select(s => s[random.Next(s.Length)]).ToArray());
                    } while (rooms.ContainsKey(id));

                    var room = new Room
                    {
                        roomId = id,
                        clientId = clientId,
                        roomName = reader.ReadString(),
                        roomData = reader.ReadString(),
                        maxCount = reader.ReadInt(),
                        roomMode = reader.ReadByte(),
                        clients = new HashSet<int>(),
                    };

                    rooms.Add(id, room);
                    clients.Add(clientId, room);
                    Log.Info(Service.Text.Format("客户端 {0} 创建房间。 房间名称: {1} 房间数: {2} 连接数: {3}", clientId, room.roomName, rooms.Count,
                        clients.Count));

                    using var writer = MemoryWriter.Pop();
                    writer.WriteByte((byte)OpCodes.CreateRoom);
                    writer.WriteString(room.roomId);
                    transport.SendToClient(clientId, writer);
                }
                else if (opcode == OpCodes.JoinRoom)
                {
                    ServerDisconnect(clientId);
                    var roomId = reader.ReadString();
                    if (rooms.TryGetValue(roomId, out var room) && room.clients.Count + 1 < room.maxCount)
                    {
                        room.clients.Add(clientId);
                        clients.Add(clientId, room);
                        Log.Info(Service.Text.Format("客户端 {0} 加入房间。 房间名称: {1} 房间数: {2} 连接数: {3}", clientId, room.roomName, rooms.Count,
                            clients.Count));

                        using var writer = MemoryWriter.Pop();
                        writer.WriteByte((byte)OpCodes.JoinRoom);
                        writer.WriteInt(clientId);
                        transport.SendToClient(clientId, writer);
                        transport.SendToClient(room.clientId, writer);
                    }
                    else
                    {
                        using var writer = MemoryWriter.Pop();
                        writer.WriteByte((byte)OpCodes.LeaveRoom);
                        transport.SendToClient(clientId, writer);
                    }
                }
                else if (opcode == OpCodes.UpdateRoom)
                {
                    if (clients.TryGetValue(clientId, out var room))
                    {
                        room.roomName = reader.ReadString();
                        room.roomData = reader.ReadString();
                        room.roomMode = reader.ReadByte();
                        room.maxCount = reader.ReadInt();
                    }
                }
                else if (opcode == OpCodes.LeaveRoom)
                {
                    ServerDisconnect(clientId);
                }
                else if (opcode == OpCodes.UpdateData)
                {
                    var message = reader.ReadArraySegment();
                    var targetId = reader.ReadInt();
                    if (clients.TryGetValue(clientId, out var room) && room != null)
                    {
                        if (message.Count > transport.MessageSize(channel))
                        {
                            Log.Warn(Service.Text.Format("接收消息大小过大！消息大小: {0}", message.Count));
                            ServerDisconnect(clientId);
                            return;
                        }

                        if (room.clientId == clientId)
                        {
                            if (room.clients.Contains(targetId))
                            {
                                using var writer = MemoryWriter.Pop();
                                writer.WriteByte((byte)OpCodes.UpdateData);
                                writer.WriteArraySegment(message);
                                transport.SendToClient(targetId, writer, channel);
                            }
                        }
                        else
                        {
                            using var writer = MemoryWriter.Pop();
                            writer.WriteByte((byte)OpCodes.UpdateData);
                            writer.WriteArraySegment(message);
                            writer.WriteInt(clientId);
                            transport.SendToClient(room.clientId, writer, channel);
                        }
                    }
                }
                else if (opcode == OpCodes.KickRoom)
                {
                    var targetId = reader.ReadInt();
                    var copies = rooms.Values.ToList();
                    foreach (var room in copies)
                    {
                        if (room.clientId == targetId) // 踢掉的是主机
                        {
                            using var writer = MemoryWriter.Pop();
                            writer.WriteByte((byte)OpCodes.LeaveRoom);
                            foreach (var client in room.clients)
                            {
                                transport.SendToClient(client, writer);
                                clients.Remove(client);
                            }

                            room.clients.Clear();
                            rooms.Remove(room.roomId);
                            clients.Remove(targetId);
                            return;
                        }

                        if (room.clientId == clientId) // 踢掉的是客户端
                        {
                            if (room.clients.Remove(targetId))
                            {
                                using var writer = MemoryWriter.Pop();
                                writer.WriteByte((byte)OpCodes.KickRoom);
                                writer.WriteInt(targetId);
                                transport.SendToClient(room.clientId, writer);
                                clients.Remove(targetId);
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                transport.StopClient(clientId);
            }
        }
    }
}