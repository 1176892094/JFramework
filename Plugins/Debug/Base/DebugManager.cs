using System;
using JFramework.Basic;
using UnityEngine;

namespace JFramework
{
    internal class DebugManager : MonoBehaviour
    {
        private DebugData debugData;
        private int FPS;
        private bool isExtend;
        private float gameTimer;
        private Rect windowRect;
        private DebugType debugType;
        private DebugConsole console;
        private DebugScene scene;
        private DebugMemory memory;
        private DebugDrawCall drawCall;
        private DebugSystem system;
        private DebugScreen screen;
        private DebugTime time;
        private DebugEnvironment environment;
        public LogLevel LogLevel;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            EventManager.AddListener(nameof(DebugManager), OnAwake);
        } 

        private void OnAwake()
        {
            Debugger.LogLevel = LogLevel;
            windowRect = new Rect(0, 0, 200, 120);
            debugData = ResourceManager.Load<DebugData>("DebugData");
            debugData.InitData();
            debugData.isDebug = true;
            debugData.MaxWidth = Screen.width;
            debugData.MaxHeight = Screen.height;
            console = new DebugConsole(debugData);
            scene = new DebugScene(debugData);
            memory = new DebugMemory(debugData);
            drawCall = new DebugDrawCall(debugData);
            system = new DebugSystem(debugData);
            screen = new DebugScreen(debugData);
            time = new DebugTime(debugData);
            environment = new DebugEnvironment(debugData);
            Application.logMessageReceived += console.LogMessageReceived;
            scene.Start();
            MonoManager.Instance.AddListener(OnUpdate);
        }

        private void OnUpdate()
        {
            if (!debugData.isDebug) return;
            float timer = Time.realtimeSinceStartup - gameTimer;
            if (timer >= 1)
            {
                FPS = (int)(1.0f / Time.deltaTime);
                gameTimer = Time.realtimeSinceStartup;
            }
        }

        private void OnGUI()
        {
            if (debugData == null || !debugData.isDebug) return;
            DebugStyle.Enable();
            if (isExtend)
            {
                Rect localRect = new Rect(0, 0, debugData.MaxWidth, debugData.MaxHeight);
                GUI.Window(0, localRect, MaxWindowGUI, debugData.GetData("Debugger"), DebugStyle.Window);
            }
            else
            {
                windowRect = GUI.Window(0, windowRect, MinGUIWindow, debugData.GetData("Debugger"), DebugStyle.Window);
            }
        }

        private void MinGUIWindow(int windowId)
        {
            GUI.DragWindow(new Rect(0, 0, 10000, 40));
            GUI.contentColor = console.colorFPS;
            GUILayout.Space(20);
            if (GUILayout.Button(debugData.GetData("FPS") + ": " + FPS, DebugStyle.Button, DebugStyle.MinGUIWindow))
            {
                isExtend = true;
            }

            GUI.contentColor = Color.white;
        }

        private void MaxWindowGUI(int windowId)
        {
            GUILayout.Space(20);
            ExtendTitleGUI();
            switch (debugType)
            {
                case DebugType.Console:
                    console.ExtendConsoleGUI();
                    break;
                case DebugType.Scene:
                    scene.ExtendSceneGUI();
                    break;
                case DebugType.Memory:
                    memory.ExtendMemoryGUI();
                    break;
                case DebugType.DrawCall:
                    drawCall.ExtendDrawCallGUI();
                    break;
                case DebugType.System:
                    system.ExtendSystemGUI();
                    break;
                case DebugType.Screen:
                    screen.ExtendScreenGUI();
                    break;
                case DebugType.Time:
                    time.ExtendTimeGUI();
                    break;
                case DebugType.Environment:
                    environment.ExtendEnvironmentGUI();
                    break;
            }
        }

        private void ExtendTitleGUI()
        {
            GUILayout.BeginHorizontal();
            GUI.contentColor = console.colorFPS;
            if (GUILayout.Button(debugData.GetData("FPS") + ": " + FPS,DebugStyle.Button, DebugStyle.MinGUIWindow))
            {
                isExtend = false;
            }

            GUI.contentColor = debugType == DebugType.Console ? Color.white : Color.gray;
            if (GUILayout.Button(debugData.GetData("Console"),DebugStyle.Button,  DebugStyle.MinHeight))
            {
                debugType = DebugType.Console;
            }

            GUI.contentColor = debugType == DebugType.Scene ? Color.white : Color.gray;
            if (GUILayout.Button(debugData.GetData("Scene"), DebugStyle.Button,  DebugStyle.MinHeight))
            {
                debugType = DebugType.Scene;
                scene.RefreshGameObject();
                scene.RefreshComponent();
            }

            GUI.contentColor = debugType == DebugType.Memory ? Color.white : Color.gray;
            if (GUILayout.Button(debugData.GetData("Memory"), DebugStyle.Button,  DebugStyle.MinHeight))
            {
                debugType = DebugType.Memory;
            }

            GUI.contentColor = debugType == DebugType.DrawCall ? Color.white : Color.gray;
            if (GUILayout.Button(debugData.GetData("DrawCall"), DebugStyle.Button,  DebugStyle.MinHeight))
            {
                debugType = DebugType.DrawCall;
            }

            GUI.contentColor = debugType == DebugType.System ? Color.white : Color.gray;
            if (GUILayout.Button(debugData.GetData("System"), DebugStyle.Button,  DebugStyle.MinHeight))
            {
                debugType = DebugType.System;
            }

            GUI.contentColor = debugType == DebugType.Screen ? Color.white : Color.gray;
            if (GUILayout.Button(debugData.GetData("Screen"), DebugStyle.Button,  DebugStyle.MinHeight))
            {
                debugType = DebugType.Screen;
            }

            GUI.contentColor = debugType == DebugType.Time ? Color.white : Color.gray;
            if (GUILayout.Button(debugData.GetData("Time"), DebugStyle.Button,  DebugStyle.MinHeight))
            {
                debugType = DebugType.Time;
            }

            GUI.contentColor = debugType == DebugType.Environment ? Color.white : Color.gray;
            if (GUILayout.Button(debugData.GetData("Environment"), DebugStyle.Button, DebugStyle.MinHeight))
            {
                debugType = DebugType.Environment;
            }

            GUI.contentColor = Color.white;
            GUILayout.EndHorizontal();
        }

        private void OnDestroy()
        {
            Application.logMessageReceived -= console.LogMessageReceived;
            MonoManager.Instance.RemoveListener(OnUpdate);
        }
    }

    internal struct LogData
    {
        public string type;
        public string dateTime;
        public string message;
        public string stackTrace;
        public string showName;
    }

    internal enum DebugType
    {
        Console = 0,
        Scene = 1,
        Memory = 2,
        DrawCall = 3,
        System = 4,
        Screen = 5,
        Time = 6,
        Environment = 7
    }
}