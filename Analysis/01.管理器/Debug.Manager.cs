// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-16 01:12:26
// # Recently: 2024-12-22 20:12:26
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using JFramework.Common;
using JFramework.Net;
using UnityEngine;

namespace JFramework
{
    [DefaultExecutionOrder(-20)]
    public partial class DebugManager : MonoBehaviour, IEvent<PingUpdate>
    {
        private float frameData;
        private double frameTime;
        private double framePing;
        private Rect minRect;
        private Status status;
        private Window window;
        private Color windowColor = Color.white;

        private Vector2 consoleView;
        private Vector2 messageView;

        private Func<Reference[]> ServicePoolRef;
        private Func<Reference[]> ServiceEventRef;
        private Func<Reference[]> PoolManagerRef;

        private void Awake()
        {
            Application.logMessageReceived += LogMessageReceived;
        }

        private void Start()
        {
            var message = typeof(Service.Pool).GetMethod("Reference", Service.Find.Static);
            if (message != null)
            {
                ServicePoolRef = (Func<Reference[]>)Delegate.CreateDelegate(typeof(Func<Reference[]>), message);
            }

            message = typeof(Service.Event).GetMethod("Reference", Service.Find.Static);
            if (message != null)
            {
                ServiceEventRef = (Func<Reference[]>)Delegate.CreateDelegate(typeof(Func<Reference[]>), message);
            }

            message = typeof(PoolManager).GetMethod("Reference", Service.Find.Static);
            if (message != null)
            {
                PoolManagerRef = (Func<Reference[]>)Delegate.CreateDelegate(typeof(Func<Reference[]>), message);
            }
            
            status |= Status.Ping;
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var sceneTypes = assembly.GetTypes();
                foreach (var sceneType in sceneTypes)
                {
                    if (!sceneType.IsSubclassOf(typeof(Component)))
                    {
                        continue;
                    }

                    if (sceneType.IsAbstract)
                    {
                        continue;
                    }

                    if (sceneType.IsGenericType)
                    {
                        continue;
                    }

                    cachedTypes.Add(sceneType);
                }
            }
        }

        private void Update()
        {
            if ((status & Status.Freeze) != 0)
            {
                return;
            }

            if (frameTime > Time.realtimeSinceStartup)
            {
                return;
            }

            frameData = (int)(1.0 / Time.deltaTime);
            frameTime = Time.realtimeSinceStartup + 1;
        }

        private void OnEnable()
        {
            Service.Event.Listen(this);
        }

        private void OnDisable()
        {
            Service.Event.Remove(this);
        }

        private void OnDestroy()
        {
            Application.logMessageReceived -= LogMessageReceived;
        }

        private void OnGUI()
        {
            if ((status & Status.Freeze) != 0)
            {
                return;
            }

            var matrix = GUI.matrix;
            var labelAlignment = GUI.skin.label.alignment;
            var fieldAlignment = GUI.skin.textField.alignment;

            GUI.matrix = Matrix4x4.Scale(Scale);
            GUI.skin.label.alignment = TextAnchor.MiddleLeft;
            GUI.skin.textField.alignment = TextAnchor.MiddleLeft;

            if (status.HasFlag(Status.Expand))
            {
                var maxRect = new Rect(0, 0, Width, Height);
                GUI.Window(0, maxRect, MaxWindow, "调试器");
            }
            else if (status.HasFlag(Status.Window))
            {
                minRect.size = new Vector2(WindowRect.size.x + 83, WindowRect.size.y + 66);
                minRect = GUI.Window(0, minRect, MinWindow, "调试器");
            }
            else if (status.HasFlag(Status.Ping))
            {
                minRect.size = new Vector2(WindowRect.size.x + 83, WindowRect.size.y);
                minRect = GUI.Window(0, minRect, MinWindow, "调试器");
            }
            else if (status.HasFlag(Status.Common))
            {
                minRect.size = WindowRect.size;
                minRect = GUI.Window(0, minRect, MinWindow, "调试器");
            }

            GUI.matrix = matrix;
            GUI.skin.label.alignment = labelAlignment;
            GUI.skin.textField.alignment = fieldAlignment;
        }

        void IEvent<PingUpdate>.Execute(PingUpdate message)
        {
            framePing = message.pingTime;
        }

        private void MaxWindow(int id)
        {
            GUILayout.BeginArea(MaxBox, "", "Box");
            GUILayout.BeginHorizontal();
            GUI.contentColor = windowColor;
            if (GUILayout.Button(Service.Text.Format("FPS: {0}", frameData), Height30, Width80))
            {
                status &= ~Status.Expand;
            }

            GUI.contentColor = window == Window.Console ? Color.white : Color.gray;
            if (GUILayout.Button(Window.Console.ToString(), Height30))
            {
                window = Window.Console;
            }

            GUI.contentColor = window == Window.Scene ? Color.white : Color.gray;
            if (GUILayout.Button(Window.Scene.ToString(), Height30))
            {
                UpdateGameObject();
                UpdateComponent();
                window = Window.Scene;
            }

            GUI.contentColor = window == Window.Reference ? Color.white : Color.gray;
            if (GUILayout.Button(Window.Reference.ToString(), Height30))
            {
                window = Window.Reference;
            }

            GUI.contentColor = window == Window.Project ? Color.white : Color.gray;
            if (GUILayout.Button(Window.Project.ToString(), Height30))
            {
                window = Window.Project;
            }

            GUILayout.EndHorizontal();

            if (window != Window.Console && window != Window.Scene && window != Window.Reference)
            {
                GUILayout.BeginHorizontal();
                GUI.contentColor = window == Window.Memory ? Color.white : Color.gray;
                if (GUILayout.Button(Window.Memory.ToString(), Height30))
                {
                    window = Window.Memory;
                }

                GUI.contentColor = window == Window.System ? Color.white : Color.gray;
                if (GUILayout.Button(Window.System.ToString(), Height30))
                {
                    window = Window.System;
                }

                GUI.contentColor = window == Window.Screen ? Color.white : Color.gray;
                if (GUILayout.Button(Window.Screen.ToString(), Height30))
                {
                    window = Window.Screen;
                }

                GUI.contentColor = window == Window.Time ? Color.white : Color.gray;
                if (GUILayout.Button(Window.Time.ToString(), Height30))
                {
                    window = Window.Time;
                }

                GUILayout.EndHorizontal();
            }

            GUI.contentColor = Color.white;
            switch (window)
            {
                case Window.Console:
                    ConsoleWindow();
                    break;
                case Window.Scene:
                    SceneWindow();
                    break;
                case Window.Reference:
                    ReferenceWindow();
                    break;
                case Window.System:
                    SystemWindow();
                    break;
                case Window.Project:
                    ProjectWindow();
                    break;
                case Window.Memory:
                    MemoryWindow();
                    break;
                case Window.Screen:
                    ScreenWindow();
                    break;
                case Window.Time:
                    TimeWindow();
                    break;
            }

            GUILayout.EndArea();
        }

        private void MinWindow(int id)
        {
            GUI.DragWindow(new Rect(0, 0, WindowRect.width, 20f));

            var newRect = MinBox;
            if ((status & Status.Ping) != 0)
            {
                newRect.width += 83;
            }

            if ((status & Status.Window) != 0)
            {
                newRect.height += 66;
            }

            GUI.contentColor = windowColor;
            GUILayout.BeginArea(newRect, "", "Box");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(Service.Text.Format("FPS: {0}", frameData), Height30, Width80))
            {
                status |= Status.Expand;
            }

            if (GUILayout.Button(Service.Text.Format("Ping: {0}", (int)(framePing * 1000)), Height30))
            {
                if ((status & Status.Window) != 0)
                {
                    status &= ~Status.Window;
                }
                else
                {
                    status |= Status.Window;
                }
            }

            GUI.contentColor = Color.white;
            GUILayout.EndHorizontal();
            if (status.HasFlag(Status.Window))
            {
                var boxAlignment = GUI.skin.box.alignment;
                GUI.skin.box.alignment = TextAnchor.MiddleCenter;
                Reference();
                GUI.skin.box.alignment = boxAlignment;
            }

            GUILayout.EndArea();
        }

        private static void Reference()
        {
            var option = GUILayout.Height(30f);
            if (!NetworkManager.Client.isConnected && !NetworkManager.Server.isActive)
            {
                if (!NetworkManager.Client.isActive)
                {
                    if (GUILayout.Button("Host (Server + Client)", option))
                    {
                        NetworkManager.StartHost();
                    }

                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Server", option))
                    {
                        NetworkManager.StartServer();
                    }

                    if (GUILayout.Button("Client", option))
                    {
                        NetworkManager.StartClient();
                    }

                    GUILayout.EndHorizontal();
                }
                else
                {
                    GUILayout.Label("Connecting...".Bold(), "Box", option);

                    if (GUILayout.Button("Stop Client", option))
                    {
                        NetworkManager.StopClient();
                    }
                }
            }
            else
            {
                if (NetworkManager.Server.isActive || NetworkManager.Client.isActive)
                {
                    var message = Service.Text.Format("{0} : {1}".Bold(), Transport.Instance.address, Transport.Instance.port);
                    GUILayout.Label(message, "Box", option);
                }
            }

            if (NetworkManager.Client.isConnected && !NetworkManager.Client.isReady)
            {
                if (GUILayout.Button("Ready", option))
                {
                    NetworkManager.Client.Ready();
                }
            }

            if (NetworkManager.Server.isActive && NetworkManager.Client.isConnected)
            {
                if (GUILayout.Button("Stop Host", option))
                {
                    NetworkManager.StopHost();
                }
            }
            else if (NetworkManager.Client.isConnected)
            {
                if (GUILayout.Button("Stop Client", option))
                {
                    NetworkManager.StopClient();
                }
            }
            else if (NetworkManager.Server.isActive)
            {
                if (GUILayout.Button("Stop Server", option))
                {
                    NetworkManager.StopServer();
                }
            }
        }
    }
}