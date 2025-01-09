using System;
using System.Net;
using System.Net.Sockets;
using JFramework.Udp;
using UnityEngine;

namespace JFramework.Net
{
    public partial class NetworkDiscovery : MonoBehaviour
    {
        [SerializeField] private string address = "";

        [SerializeField] private ushort port = 47777;

        [SerializeField] private int duration = 1;

        [SerializeField] private string version;

        private UdpClient udpClient;

        private UdpClient udpServer;

        public void StartDiscovery()
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                Debug.LogError("网络发现不支持WebGL");
                return;
            }

            StopDiscovery();
            switch (NetworkManager.Mode)
            {
                case EntryMode.Server or EntryMode.Host:
                    udpServer = new UdpClient(port)
                    {
                        EnableBroadcast = true,
                        MulticastLoopback = false
                    };
#if UNITY_ANDROID
                    BeginMulticastLock();
#endif
                    ServerReceive();
                    break;
                case EntryMode.Client:
                    udpClient = new UdpClient(0)
                    {
                        EnableBroadcast = true,
                        MulticastLoopback = false
                    };
                    ClientReceive();
                    InvokeRepeating(nameof(ClientSend), 0, duration);
                    break;
            }
        }

        public void StopDiscovery()
        {
#if UNITY_ANDROID
            EndMulticastLock();
#endif
            udpServer?.Close();
            udpClient?.Close();
            udpServer = null;
            udpClient = null;
            CancelInvoke();
        }

        private void OnDestroy()
        {
            StopDiscovery();
        }

        private void ClientSend()
        {
            try
            {
                if (NetworkManager.Client.isConnected)
                {
                    StopDiscovery();
                    return;
                }

                var endPoint = new IPEndPoint(IPAddress.Broadcast, port);
                if (!string.IsNullOrWhiteSpace(address))
                {
                    endPoint = new IPEndPoint(IPAddress.Parse(address), port);
                }

                using var writer = MemoryWriter.Pop();
                writer.WriteString(version);
                writer.Invoke(new RequestMessage());
                ArraySegment<byte> segment = writer;
                udpClient.Send(segment.Array, segment.Count, endPoint);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private async void ServerReceive()
        {
            while (udpServer != null)
            {
                try
                {
                    var result = await udpServer.ReceiveAsync();
                    using var reader = MemoryReader.Pop(result.Buffer);
                    if (version != reader.ReadString())
                    {
                        Debug.LogError("接收到的消息版本不同！");
                        return;
                    }

                    reader.Invoke<RequestMessage>();
                    ServerSend(result.RemoteEndPoint);
                }
                catch (ObjectDisposedException)
                {
                    return;
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        private void ServerSend(IPEndPoint endPoint)
        {
            try
            {
                using var writer = MemoryWriter.Pop();
                writer.WriteString(version);
                writer.Invoke(new ResponseMessage(new UriBuilder
                {
                    Scheme = "https",
                    Host = Dns.GetHostName(),
                    Port = ((IAddress)NetworkManager.Transport).port
                }.Uri));
                ArraySegment<byte> segment = writer;
                udpServer.Send(segment.Array, segment.Count, endPoint);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private async void ClientReceive()
        {
            while (udpClient != null)
            {
                try
                {
                    var result = await udpClient.ReceiveAsync();
                    using var reader = MemoryReader.Pop(result.Buffer);
                    if (version != reader.ReadString())
                    {
                        Debug.LogError("接收到的消息版本不同息！");
                        return;
                    }

                    var endPoint = result.RemoteEndPoint;
                    var response = reader.Invoke<ResponseMessage>();
                    Service.Event.Invoke(new ServerResponseEvent(new UriBuilder(response.uri)
                    {
                        Host = endPoint.Address.ToString()
                    }.Uri, endPoint));
                }
                catch (ObjectDisposedException)
                {
                    return;
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }
    }

    public partial class NetworkDiscovery
    {
#if UNITY_ANDROID
        /// <summary>
        /// 是否启用多播
        /// </summary>
        private bool multicast;

        /// <summary>
        /// 多播
        /// </summary>
        private AndroidJavaObject multicastLock;

        /// <summary>
        /// 开启多播锁
        /// </summary>
        private void BeginMulticastLock()
        {
            if (multicast) return;
            if (Application.platform == RuntimePlatform.Android)
            {
                using var activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                using var wifiManager = activity.Call<AndroidJavaObject>("getSystemService", "wifi");
                multicastLock = wifiManager.Call<AndroidJavaObject>("createMulticastLock", "lock");
                multicastLock.Call("acquire");
                multicast = true;
            }
        }

        /// <summary>
        /// 结束多播锁
        /// </summary>
        private void EndMulticastLock()
        {
            if (!multicast) return;
            multicastLock?.Call("release");
            multicast = false;
        }
#endif
    }
}