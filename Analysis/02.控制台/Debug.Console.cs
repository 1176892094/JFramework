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
    public partial class DebugManager
    {
        private readonly Dictionary<LogType, LogData> logData = new Dictionary<LogType, LogData>
        {
            { LogType.Log, new LogData(Color.white) },
            { LogType.Warning, new LogData(Color.yellow) },
            { LogType.Exception, new LogData(Color.magenta) },
            { LogType.Error, new LogData(Color.red) },
            { LogType.Assert, new LogData(Color.green) }
        };

        private readonly List<LogMessage> messages = new List<LogMessage>();
        private int selectMessage = -1;

        private void ConsoleWindow()
        {
            ConsoleButton();
            ConsoleOption();
            ConsoleScroll();
        }

        private void LogMessageReceived(string message, string stackTrace, LogType logType)
        {
            if (!logData.TryGetValue(logType, out var data))
            {
                return;
            }

            if (messages.Count >= 100)
            {
                if (logData.TryGetValue(messages[0].logType, out var log))
                {
                    log.count--;
                }

                messages.RemoveAt(0);
            }

            messages.Add(new LogMessage(message, stackTrace, logType));

            data.count++;
            var logs = logData.Values.Reverse();
            foreach (var log in logs)
            {
                if (log.count > 0)
                {
                    windowColor = log.color;
                    break;
                }
            }
        }

        private void ConsoleButton()
        {
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Clear", BoxWidth, Height30))
            {
                selectMessage = -1;
                foreach (var data in logData.Values)
                {
                    data.count = 0;
                }

                messages.Clear();
                windowColor = Color.white;
            }

            if (GUILayout.Button("Report", Height30))
            {
                var mailBody = new StringBuilder(1024);
                foreach (var message in messages)
                {
                    mailBody.Append(message + "\n\n" + message.stackTrace + "\n\n");
                }

                Service.Mail.Send(GlobalSetting.Instance.SendMail(mailBody.ToString()));
            }

            GUILayout.EndHorizontal();
        }

        private void ConsoleOption()
        {
            GUILayout.BeginHorizontal();
            foreach (var type in logData.Keys)
            {
                if (logData.TryGetValue(type, out var data))
                {
                    GUI.contentColor = data.status ? Color.white : Color.gray;
                    if (GUILayout.Button(Service.Text.Format("{0} [{1}]", type, data.count), Height30))
                    {
                        data.status = !data.status;
                    }
                }
            }

            GUILayout.EndHorizontal();
        }

        private void ConsoleScroll()
        {
            consoleView = GUILayout.BeginScrollView(consoleView, "Box", ScrollHeight);

            for (var i = 0; i < messages.Count; i++)
            {
                if (logData.TryGetValue(messages[i].logType, out var data) && data.status)
                {
                    GUILayout.BeginHorizontal();
                    GUI.contentColor = data.color;
                    if (GUILayout.Toggle(selectMessage == i, messages[i].ToString(), Height20))
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
                GUILayout.Label(messages[selectMessage].message + "\n\n" + messages[selectMessage].stackTrace);
            }

            GUILayout.EndScrollView();
        }
    }
}