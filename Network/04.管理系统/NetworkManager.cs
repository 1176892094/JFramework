// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-11-29 13:11:20
// # Recently: 2024-12-22 21:12:51
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Runtime.CompilerServices;
using JFramework.Common;
using UnityEngine;

namespace JFramework.Net
{
    public partial class NetworkManager : MonoBehaviour, IEvent<SceneComplete>
    {
        public static NetworkManager Instance;

        public Transport transport;

        public int sendRate = 30;

        public int connection = 100;

        private string sceneName;

        public static EntryMode Mode
        {
            get
            {
                if (!Application.isPlaying)
                {
                    return EntryMode.None;
                }

                if (Server.isActive)
                {
                    return Client.isActive ? EntryMode.Host : EntryMode.Server;
                }

                return Client.isActive ? EntryMode.Client : EntryMode.None;
            }
        }

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Application.runInBackground = true;
            Transport.Instance = transport;
        }

        private void OnEnable()
        {
            Service.Event.Listen(this);
        }

        private void OnDisable()
        {
            Service.Event.Remove(this);
        }

        private void OnApplicationQuit()
        {
            if (Client.isConnected)
            {
                StopClient();
            }

            if (Server.isActive)
            {
                StopServer();
            }
        }

        public void Execute(SceneComplete message)
        {
            switch (Mode)
            {
                case EntryMode.Host:
                    Server.LoadSceneComplete(sceneName);
                    Client.LoadSceneComplete(sceneName);
                    break;
                case EntryMode.Server:
                    Server.LoadSceneComplete(sceneName);
                    break;
                case EntryMode.Client:
                    Client.LoadSceneComplete(sceneName);
                    break;
            }
        }

        public static void StartServer()
        {
            if (Server.isActive)
            {
                Debug.LogWarning("服务器已经连接！");
                return;
            }

            Server.Start(EntryMode.Server);
        }

        public static void StopServer()
        {
            if (!Server.isActive)
            {
                Debug.LogWarning("服务器已经停止！");
                return;
            }

            Server.Stop();
        }

        public static void StartClient()
        {
            if (Client.isActive)
            {
                Debug.LogWarning("客户端已经连接！");
                return;
            }

            Client.Start(EntryMode.Client);
        }

        public static void StartClient(Uri uri)
        {
            if (Client.isActive)
            {
                Debug.LogWarning("客户端已经连接！");
                return;
            }

            Client.Start(uri);
        }

        public static void StopClient()
        {
            if (!Client.isActive)
            {
                Debug.LogWarning("客户端已经停止！");
                return;
            }

            if (Mode == EntryMode.Host)
            {
                Server.OnServerDisconnect(Server.hostId);
            }

            Client.Stop();
        }

        public static void StartHost(EntryMode mode = EntryMode.Host)
        {
            if (Server.isActive || Client.isActive)
            {
                Debug.LogWarning("客户端或服务器已经连接！");
                return;
            }

            Server.Start(mode);
            Client.Start(EntryMode.Host);
        }

        public static void StopHost()
        {
            StopClient();
            StopServer();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NetworkObject GetNetworkObject(uint objectId)
        {
            if (Server.isActive)
            {
                if (Server.spawns.TryGetValue(objectId, out var @object))
                {
                    return @object;
                }
            }

            if (Client.isActive)
            {
                if (Client.spawns.TryGetValue(objectId, out var @object))
                {
                    return @object;
                }
            }

            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool Tick(float sendRate, ref double sendTime)
        {
            var duration = 1.0 / sendRate;
            if (sendTime + duration <= Time.unscaledTimeAsDouble)
            {
                sendTime = (long)(Time.unscaledTimeAsDouble / duration) * duration;
                return true;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsSceneObject(NetworkObject @object)
        {
            if (@object.sceneId == 0)
            {
                return false;
            }

            if (@object.gameObject.hideFlags == HideFlags.NotEditable)
            {
                return false;
            }

            return @object.gameObject.hideFlags != HideFlags.HideAndDontSave;
        }

        private static void Reference()
        {
            var option = GUILayout.Height(30);
            if (!Client.isConnected && !Server.isActive)
            {
                if (!Client.isActive)
                {
                    if (GUILayout.Button("Host (Server + Client)", option))
                    {
                        StartHost();
                    }

                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Server", option))
                    {
                        StartServer();
                    }

                    if (GUILayout.Button("Client", option))
                    {
                        StartClient();
                    }

                    GUILayout.EndHorizontal();
                }
                else
                {
                    GUILayout.Label("<b>Connecting...</b>", "Box", option);

                    if (GUILayout.Button("Stop Client", option))
                    {
                        StopClient();
                    }
                }
            }
            else
            {
                if (Server.isActive || Client.isActive)
                {
                    var message = Service.Text.Format("<b>{0} : {1}</b>", Transport.Instance.address, Transport.Instance.port);
                    GUILayout.Label(message, "Box", option);
                }
            }

            if (Client.isConnected && !Client.isReady)
            {
                if (GUILayout.Button("Ready", option))
                {
                    Client.Ready();
                }
            }

            if (Server.isActive && Client.isConnected)
            {
                if (GUILayout.Button("Stop Host", option))
                {
                    StopHost();
                }
            }
            else if (Client.isConnected)
            {
                if (GUILayout.Button("Stop Client", option))
                {
                    StopClient();
                }
            }
            else if (Server.isActive)
            {
                if (GUILayout.Button("Stop Server", option))
                {
                    StopServer();
                }
            }
        }
    }
}