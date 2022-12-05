using System;
using System.Collections.Generic;
using UnityEngine;

namespace JFramework
{
    internal class DebugConsole
    {
        public Color colorFPS = Color.white;
        private readonly DebugData debugData;
        private readonly List<LogData> logList = new List<LogData>();
        private int logIndex = -1;
        private int logInfo;
        private int logWarn;
        private int logError;
        private int logFatal;
        private int logException;
        private bool showLogInfo = true;
        private bool showLogWarn = true;
        private bool showLogError = true;
        private bool showLogFatal = true;
        private bool showLogException = true;
        private Vector2 scrollLogView = Vector2.zero;
        private Vector2 currentLogView = Vector2.zero;

        public DebugConsole(DebugData debugData) => this.debugData = debugData;

        public void LogMessageReceived(string message, string stackTrace, LogType type)
        {
            LogData log = new LogData
            {
                dateTime = DateTime.Now.ToString("HH:mm:ss"),
                message = message,
                stackTrace = stackTrace
            };
            switch (type)
            {
                case LogType.Assert:
                    log.type = "Fatal";
                    logFatal += 1;
                    break;
                case LogType.Error:
                    log.type = "Error";
                    logError += 1;
                    break;
                case LogType.Warning:
                    log.type = "Warning";
                    logWarn += 1;
                    break;
                case LogType.Exception:
                    log.type = "Exception";
                    logException += 1;
                    break;
                case LogType.Log:
                    log.type = "Log";
                    logInfo += 1;
                    break;
            }

            log.showName = "[" + log.dateTime + "] [" + log.type + "] " + log.message;
            logList.Add(log);

            if (logFatal > 0)
            {
                colorFPS = new Color(1f, 0.5f, 0);
            }
            else if (logError > 0)
            {
                colorFPS = Color.red;
            }
            else if (logException > 0)
            {
                colorFPS = Color.magenta;
            }
            else if (logWarn > 0)
            {
                colorFPS = Color.yellow;
            }
        }

        public void ExtendConsoleGUI()
        {
            GUILayout.BeginHorizontal();
            GUI.contentColor = Color.white;
            if (GUILayout.Button(debugData.GetData("Clear"), DebugStyle.Button, DebugStyle.MinHeight))
            {
                logList.Clear();
                logIndex = -1;
                logInfo = 0;
                logWarn = 0;
                logError = 0;
                logFatal = 0;
                logException = 0;
                colorFPS = Color.white;
            }

            GUI.contentColor = showLogInfo ? Color.white : Color.gray;
            if (GUILayout.Button(debugData.GetData("Log") + " [" + logInfo + "]", DebugStyle.Button,
                    DebugStyle.MinHeight))
            {
                showLogInfo = !showLogInfo;
            }

            GUI.contentColor = showLogWarn ? Color.white : Color.gray;
            if (GUILayout.Button(debugData.GetData("Warning") + " [" + logWarn + "]", DebugStyle.Button,
                    DebugStyle.MinHeight))
            {
                showLogWarn = !showLogWarn;
            }

            GUI.contentColor = showLogException ? Color.white : Color.gray;
            if (GUILayout.Button(debugData.GetData("Exception") + " [" + logException + "]", DebugStyle.Button,
                    DebugStyle.MinHeight))
            {
                showLogException = !showLogException;
            }

            GUI.contentColor = showLogError ? Color.white : Color.gray;
            if (GUILayout.Button(debugData.GetData("Error") + " [" + logError + "]", DebugStyle.Button,
                    DebugStyle.MinHeight))
            {
                showLogError = !showLogError;
            }

            GUI.contentColor = showLogFatal ? Color.white : Color.gray;
            if (GUILayout.Button(debugData.GetData("Fatal") + " [" + logFatal + "]", DebugStyle.Button,
                    DebugStyle.MinHeight))
            {
                showLogFatal = !showLogFatal;
            }

            GUI.contentColor = Color.white;
            GUILayout.EndHorizontal();
            scrollLogView = GUILayout.BeginScrollView(scrollLogView, DebugStyle.Box, GUILayout.Height(Screen.height * 0.5f));
            for (int i = 0; i < logList.Count; i++)
            {
                bool show = false;
                Color color = Color.white;
                switch (logList[i].type)
                {
                    case "Fatal":
                        show = showLogFatal;
                        color = new Color(1, 0.5f, 0);
                        break;
                    case "Exception":
                        show = showLogException;
                        color = Color.magenta;
                        break;
                    case "Error":
                        show = showLogError;
                        color = Color.red;
                        break;
                    case "Log":
                        show = showLogInfo;
                        color = Color.white;
                        break;
                    case "Warning":
                        show = showLogWarn;
                        color = Color.yellow;
                        break;
                }

                if (show)
                {
                    GUILayout.BeginHorizontal();
                    GUI.contentColor = color;
                    if (GUILayout.Toggle(logIndex == i, logList[i].showName, DebugStyle.Label))
                    {
                        logIndex = i;
                    }

                    GUI.contentColor = Color.white;
                    GUILayout.EndHorizontal();
                }
            }

            GUILayout.EndScrollView();

            currentLogView = GUILayout.BeginScrollView(currentLogView, DebugStyle.Box);
            if (logIndex != -1)
            {
                GUILayout.Label(logList[logIndex].message + "\r\n\r\n" + logList[logIndex].stackTrace,
                    DebugStyle.Label);
            }

            GUILayout.EndScrollView();
        }
    }
}