using JFramework.Basic;
using UnityEngine;
using Logger = JFramework.Basic.Logger;

namespace JFramework
{
    internal class DebugManager : BaseManager<DebugManager>
    {
        private int FPS;
        private bool isExtend;
        private float gameTimer;
        private Rect windowRect;
        private DebugType debugType;
        private DebugSetting setting;
        private DebugConsole console;
        private DebugScene scene;
        private DebugMemory memory;
        private DebugDrawCall drawCall;
        private DebugSystem system;
        private DebugScreen screen;
        private DebugTime time;
        private DebugEnvironment environment;
        public LogLevel LogLevel;

        protected override void Awake()
        {
            base.Awake();
            Logger.LogLevel = LogLevel;
            windowRect = new Rect(0, 0, 200, 120);
            setting = ResourceManager.Load<DebugSetting>("DebugSetting");
            setting.InitData();
            time = new DebugTime(setting);
            scene = new DebugScene(setting);
            memory = new DebugMemory(setting);
            system = new DebugSystem(setting);
            screen = new DebugScreen(setting);
            console = new DebugConsole(setting);
            drawCall = new DebugDrawCall(setting);
            environment = new DebugEnvironment(setting);
            Application.logMessageReceived += console.LogMessageReceived;
            scene.InitComponent();
        }

        protected override void OnUpdate()
        {
            if (!setting.IsDebug) return;
            float timer = Time.realtimeSinceStartup - gameTimer;
            if (timer >= 1)
            {
                FPS = (int)(1.0f / Time.deltaTime);
                gameTimer = Time.realtimeSinceStartup;
            }
        }

        private void OnGUI()
        {
            if (setting == null || !setting.IsDebug) return;
            DebugStyle.Enable();
            if (isExtend)
            {
                Rect localRect = new Rect(0, 0, setting.MaxWidth, setting.MaxHeight);
                GUI.Window(0, localRect, MaxWindowGUI, setting.GetData("Debugger"), DebugStyle.Window);
            }
            else
            {
                windowRect = GUI.Window(0, windowRect, MinGUIWindow, setting.GetData("Debugger"), DebugStyle.Window);
            }
        }

        private void MinGUIWindow(int windowId)
        {
            GUI.DragWindow(new Rect(0, 0, 10000, 40));
            GUI.contentColor = console.colorFPS;
            GUILayout.Space(20);
            if (GUILayout.Button(setting.GetData("FPS") + ": " + FPS, DebugStyle.Button, DebugStyle.MinGUIWindow))
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
            if (GUILayout.Button(setting.GetData("FPS") + ": " + FPS,DebugStyle.Button, DebugStyle.MinGUIWindow))
            {
                isExtend = false;
            }

            GUI.contentColor = debugType == DebugType.Console ? Color.white : Color.gray;
            if (GUILayout.Button(setting.GetData("Console"),DebugStyle.Button,  DebugStyle.MinHeight))
            {
                debugType = DebugType.Console;
            }

            GUI.contentColor = debugType == DebugType.Scene ? Color.white : Color.gray;
            if (GUILayout.Button(setting.GetData("Scene"), DebugStyle.Button,  DebugStyle.MinHeight))
            {
                debugType = DebugType.Scene;
                scene.RefreshGameObject();
                scene.RefreshComponent();
            }

            GUI.contentColor = debugType == DebugType.Memory ? Color.white : Color.gray;
            if (GUILayout.Button(setting.GetData("Memory"), DebugStyle.Button,  DebugStyle.MinHeight))
            {
                debugType = DebugType.Memory;
            }

            GUI.contentColor = debugType == DebugType.DrawCall ? Color.white : Color.gray;
            if (GUILayout.Button(setting.GetData("DrawCall"), DebugStyle.Button,  DebugStyle.MinHeight))
            {
                debugType = DebugType.DrawCall;
            }

            GUI.contentColor = debugType == DebugType.System ? Color.white : Color.gray;
            if (GUILayout.Button(setting.GetData("System"), DebugStyle.Button,  DebugStyle.MinHeight))
            {
                debugType = DebugType.System;
            }

            GUI.contentColor = debugType == DebugType.Screen ? Color.white : Color.gray;
            if (GUILayout.Button(setting.GetData("Screen"), DebugStyle.Button,  DebugStyle.MinHeight))
            {
                debugType = DebugType.Screen;
            }

            GUI.contentColor = debugType == DebugType.Time ? Color.white : Color.gray;
            if (GUILayout.Button(setting.GetData("Time"), DebugStyle.Button,  DebugStyle.MinHeight))
            {
                debugType = DebugType.Time;
            }

            GUI.contentColor = debugType == DebugType.Environment ? Color.white : Color.gray;
            if (GUILayout.Button(setting.GetData("Environment"), DebugStyle.Button, DebugStyle.MinHeight))
            {
                debugType = DebugType.Environment;
            }

            GUI.contentColor = Color.white;
            GUILayout.EndHorizontal();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Application.logMessageReceived -= console.LogMessageReceived;
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