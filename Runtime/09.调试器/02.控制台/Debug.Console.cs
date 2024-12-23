// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-18 20:12:46
// # Recently: 2024-12-22 20:12:41
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace JFramework
{
    internal partial class DebugManager
    {
        private readonly Dictionary<LogType, LogInfo> logInfos = new Dictionary<LogType, LogInfo>
        {
            { LogType.Log, new LogInfo(LogType.Log, Color.white) },
            { LogType.Warning, new LogInfo(LogType.Warning, Color.yellow) },
            { LogType.Exception, new LogInfo(LogType.Exception, Color.magenta) },
            { LogType.Error, new LogInfo(LogType.Error, Color.red) },
            { LogType.Assert, new LogInfo(LogType.Assert, Color.green) },
        };

        private readonly List<LogMessage> logMessages = new List<LogMessage>();
        private Vector2 consoleView = Vector2.zero;
        private Vector2 messageView = Vector2.zero;

        private int selectMessage = -1;

        private void ConsoleWindow()
        {
            ConsoleButton();
            ConsoleOption();
            ConsoleScroll();
        }

        private void LogMessageReceived(string message, string stackTrace, LogType logType)
        {
            if (logInfos.TryGetValue(logType, out var logInfo))
            {
                if (logMessages.Count >= 100)
                {
                    if (logInfos.TryGetValue(logMessages[0].logType, out var removeInfo))
                    {
                        removeInfo.count--;
                    }

                    logMessages.RemoveAt(0);
                }

                logInfo.count++;
                var logData = logInfos.Values.Reverse().ToList();
                foreach (var data in logData)
                {
                    if (data.count > 0 && data.logType != LogType.Log)
                    {
                        windowColor = data.color;
                        break;
                    }
                }

                logMessages.Add(new LogMessage(message, stackTrace, logType));
            }
        }

        private void ConsoleButton()
        {
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Clear", BoxWidth, Height30))
            {
                selectMessage = -1;
                foreach (var logData in logInfos.Values)
                {
                    logData.count = 0;
                }

                logMessages.Clear();
                windowColor = Color.white;
            }

            if (GUILayout.Button("Report", Height30))
            {
                var mailBody = new StringBuilder(1024);
                foreach (var debugData in logMessages)
                {
                    mailBody.Append(debugData + "\n\n" + debugData.stackTrace + "\n\n");
                }

                var mailData = new Service.Mail.MailData
                {
                    smtpServer = GlobalSetting.Instance.smtpServer,
                    smtpPort = GlobalSetting.Instance.smtpPort,
                    senderName = "JFramework",
                    senderAddress = GlobalSetting.Instance.smtpUsername,
                    senderPassword = GlobalSetting.Instance.smtpPassword,
                    targetAddress = GlobalSetting.Instance.smtpUsername,
                    mailName = "来自《JFramework》的调试日志:",
                    mailBody = mailBody.ToString()
                };

                Service.Mail.Send(mailData);
            }

            GUILayout.EndHorizontal();
        }

        private void ConsoleOption()
        {
            GUILayout.BeginHorizontal();
            foreach (var logInfo in logInfos.Values)
            {
                GUI.contentColor = logInfo.status ? Color.white : Color.gray;
                if (GUILayout.Button(Service.Text.Format("{0} [{1}]", logInfo.logType, logInfo.count), Height30))
                {
                    logInfo.status = !logInfo.status;
                }
            }

            GUILayout.EndHorizontal();
        }

        private void ConsoleScroll()
        {
            consoleView = GUILayout.BeginScrollView(consoleView, "Box", ScrollHeight);

            for (var i = 0; i < logMessages.Count; i++)
            {
                if (logInfos.TryGetValue(logMessages[i].logType, out var logInfo) && logInfo.status)
                {
                    GUILayout.BeginHorizontal();
                    GUI.contentColor = logInfo.color;
                    if (GUILayout.Toggle(selectMessage == i, logMessages[i].ToString(), Height20))
                    {
                        selectMessage = i;
                    }

                    GUI.contentColor = Color.white;
                    GUILayout.EndHorizontal();
                }
            }

            GUILayout.EndScrollView();

            messageView = GUILayout.BeginScrollView(messageView, "Box");

            if (selectMessage != -1)
            {
                GUILayout.Label(logMessages[selectMessage].message + "\n\n" + logMessages[selectMessage].stackTrace);
            }

            GUILayout.EndScrollView();
        }
    }
}