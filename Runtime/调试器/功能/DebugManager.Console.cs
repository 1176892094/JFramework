using System;
using System.Collections.Generic;
using UnityEngine;

namespace JFramework.Core
{
    internal sealed partial class DebugManager
    {
        private readonly List<LogData> logList = new List<LogData>();
        private int logIndex = -1;
        private int logInfo;
        private int logWarn;
        private int logError;
        private int logAssert;
        private int logException;
        private bool showInfo = true;
        private bool showWarn = true;
        private bool showError = true;
        private bool showFatal = true;
        private bool showException = true;
        private Vector2 maxLogView = Vector2.zero;
        private Vector2 curLogView = Vector2.zero;

        private void LogMessageReceived(string message, string stackTrace, LogType type)
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
                    log.type = DebugConst.Assert;
                    logAssert += 1;
                    break;
                case LogType.Error:
                    log.type = DebugConst.Error;
                    logError += 1;
                    break;
                case LogType.Warning:
                    log.type = DebugConst.Warning;
                    logWarn += 1;
                    break;
                case LogType.Exception:
                    log.type = DebugConst.Exception;
                    logException += 1;
                    break;
                case LogType.Log:
                    log.type = DebugConst.Log;
                    logInfo += 1;
                    break;
            }

            log.showTitle = "[" + log.dateTime + "] [" + log.type + "] " + log.message;
            logList.Add(log);

            if (logAssert > 0)
            {
                FPSColor = new Color(1f, 0.5f, 0);
            }
            else if (logError > 0)
            {
                FPSColor = Color.red;
            }
            else if (logException > 0)
            {
                FPSColor = Color.magenta;
            }
            else if (logWarn > 0)
            {
                FPSColor = Color.yellow;
            }
            else
            {
                FPSColor = Color.white;
            }
        }

        private void DebugConsole()
        {
            GUILayout.BeginHorizontal();
            GUI.contentColor = Color.white;
            if (GUILayout.Button(DebugConst.Clear, DebugData.Button, DebugData.Height))
            {
                logList.Clear();
                logIndex = -1;
                logInfo = 0;
                logWarn = 0;
                logError = 0;
                logAssert = 0;
                logException = 0;
                FPSColor = Color.white;
            }

            GUI.contentColor = showInfo ? Color.white : Color.gray;
            if (GUILayout.Button($"{DebugConst.Log} [{logInfo}]", DebugData.Button, DebugData.Height))
            {
                showInfo = !showInfo;
            }

            GUI.contentColor = showWarn ? Color.white : Color.gray;
            if (GUILayout.Button($"{DebugConst.Warning} [{logWarn}]", DebugData.Button, DebugData.Height))
            {
                showWarn = !showWarn;
            }

            GUI.contentColor = showException ? Color.white : Color.gray;
            if (GUILayout.Button($"{DebugConst.Exception} [{logException}]", DebugData.Button, DebugData.Height))
            {
                showException = !showException;
            }

            GUI.contentColor = showError ? Color.white : Color.gray;
            if (GUILayout.Button($"{DebugConst.Error} [{logError}]", DebugData.Button, DebugData.Height))
            {
                showError = !showError;
            }

            GUI.contentColor = showFatal ? Color.white : Color.gray;
            if (GUILayout.Button($"{DebugConst.Assert} [{logAssert}]", DebugData.Button, DebugData.Height))
            {
                showFatal = !showFatal;
            }

            GUI.contentColor = Color.white;
            GUILayout.EndHorizontal();
            maxLogView = GUILayout.BeginScrollView(maxLogView, DebugData.Box, DebugData.HeightLog);
            for (int i = 0; i < logList.Count; i++)
            {
                bool show = false;
                Color color = Color.white;
                switch (logList[i].type)
                {
                    case DebugConst.Assert:
                        show = showFatal;
                        color = new Color(1, 0.5f, 0);
                        break;
                    case DebugConst.Exception:
                        show = showException;
                        color = Color.magenta;
                        break;
                    case DebugConst.Error:
                        show = showError;
                        color = Color.red;
                        break;
                    case DebugConst.Log:
                        show = showInfo;
                        color = Color.white;
                        break;
                    case DebugConst.Warning:
                        show = showWarn;
                        color = Color.yellow;
                        break;
                }

                if (show)
                {
                    GUILayout.BeginHorizontal();
                    GUI.contentColor = color;
                    if (GUILayout.Toggle(logIndex == i, logList[i].showTitle, DebugData.Label))
                    {
                        logIndex = i;
                    }

                    GUI.contentColor = Color.white;
                    GUILayout.EndHorizontal();
                }
            }

            GUILayout.EndScrollView();

            curLogView = GUILayout.BeginScrollView(curLogView, DebugData.Box);
            if (logIndex != -1)
            {
                GUILayout.Label(logList[logIndex].message + "\r\n\r\n" + logList[logIndex].stackTrace, DebugData.Label);
            }

            GUILayout.EndScrollView();
        }
    }
}