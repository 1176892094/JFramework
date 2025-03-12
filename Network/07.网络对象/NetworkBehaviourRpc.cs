// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-21 23:12:50
// # Recently: 2024-12-22 22:12:02
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************


using System.Linq;
using JFramework.Common;
using UnityEngine;

namespace JFramework.Net
{
    public abstract partial class NetworkBehaviour
    {
        protected void SendServerRpcInternal(string methodName, int methodHash, MemoryWriter writer, int channel)
        {
            if (!NetworkManager.Client.isActive)
            {
                Debug.LogError(Service.Text.Format("调用 {0} 但是客户端不是活跃的。", methodName), gameObject);
                return;
            }

            if (!NetworkManager.Client.isReady)
            {
                Debug.LogWarning(Service.Text.Format("调用 {0} 但是客户端没有准备就绪的。对象名称：{1}", methodName, name), gameObject);
                return;
            }

            if ((channel & Channel.NonOwner) == 0 && !isOwner)
            {
                Debug.LogWarning(Service.Text.Format("调用 {0} 但是客户端没有对象权限。对象名称：{1}", methodName, name), gameObject);
                return;
            }

            if (NetworkManager.Client.connection == null)
            {
                Debug.LogError(Service.Text.Format("调用 {0} 但是客户端的连接为空。对象名称：{1}", methodName, name), gameObject);
                return;
            }

            var message = new ServerRpcMessage
            {
                objectId = objectId,
                componentId = componentId,
                methodHash = (ushort)methodHash,
                segment = writer,
            };

            NetworkManager.Client.connection.Send(message, (channel & Channel.Reliable) != 0 ? Channel.Reliable : Channel.Unreliable);
        }

        protected void SendClientRpcInternal(string methodName, int methodHash, MemoryWriter writer, int channel)
        {
            if (!NetworkManager.Server.isActive)
            {
                Debug.LogError(Service.Text.Format("调用 {0} 但是服务器不是活跃的。", methodName), gameObject);
                return;
            }

            if (!isServer)
            {
                Debug.LogWarning(Service.Text.Format("调用 {0} 但是对象未初始化。对象名称：{1}", methodName, name), gameObject);
                return;
            }

            var message = new ClientRpcMessage
            {
                objectId = objectId,
                componentId = componentId,
                methodHash = (ushort)methodHash,
                segment = writer
            };

            using var current = MemoryWriter.Pop();
            current.Invoke(message);

            foreach (var client in NetworkManager.Server.clients.Values.Where(client => client.isReady))
            {
                if ((channel & Channel.NonOwner) == 0 || client != connection)
                {
                    client.Send(message, (channel & Channel.Reliable) != 0 ? Channel.Reliable : Channel.Unreliable);
                }
            }
        }

        protected void SendTargetRpcInternal(NetworkClient client, string methodName, int methodHash, MemoryWriter writer, int channel)
        {
            if (!NetworkManager.Server.isActive)
            {
                Debug.LogError(Service.Text.Format("调用 {0} 但是服务器不是活跃的。", methodName), gameObject);
                return;
            }

            if (!isServer)
            {
                Debug.LogWarning(Service.Text.Format("调用 {0} 但是对象未初始化。对象名称：{1}", methodName, name), gameObject);
                return;
            }

            if (client == null)
            {
                client = connection;
            }

            if (client == null)
            {
                Debug.LogError(Service.Text.Format("调用 {0} 但是对象的连接为空。对象名称：{1}", methodName, name), gameObject);
                return;
            }

            var message = new ClientRpcMessage
            {
                objectId = objectId,
                componentId = componentId,
                methodHash = (ushort)methodHash,
                segment = writer
            };

            client.Send(message, channel);
        }
    }
}