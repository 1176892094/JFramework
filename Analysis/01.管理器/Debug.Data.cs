// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-18 21:12:35
// # Recently: 2024-12-22 20:12:36
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using UnityEngine;

namespace JFramework
{
    public partial class DebugManager
    {
        private static Vector2 ScreenRate = new Vector2(2560, 1440);
        private static Rect WindowRect = new Rect(10, 20, 108, 69);
        private static float Width => Screen.width / Rate;
        private static float Height => Screen.height / Rate;
        private static float Rate => Screen.width / ScreenRate.x + Screen.height / ScreenRate.y;
        private static Vector3 Scale => new Vector3(Rate, Rate, 1f);

        private static Rect MaxBox
        {
            get
            {
                var width = Width - WindowRect.x * 2;
                var height = Height - WindowRect.y / 2 * 3;
                return new Rect(WindowRect.x, WindowRect.y, width, height);
            }
        }

        private static Rect MinBox
        {
            get
            {
                var width = WindowRect.width - WindowRect.x * 2;
                var height = WindowRect.height - WindowRect.y / 2 * 3;
                return new Rect(WindowRect.x, WindowRect.y, width, height);
            }
        }

        private static GUILayoutOption ScrollHeight => GUILayout.Height(Height * 0.4f);
        private static GUILayoutOption BoxWidth => GUILayout.Width((Width - 30f) / 2);
        private static GUILayoutOption PoolWidth => GUILayout.Width((Width - 50f) / 2);
        private static GUILayoutOption Width80 => GUILayout.Width(80f);
        private static GUILayoutOption Width160 => GUILayout.Width(160f);
        private static GUILayoutOption Height20 => GUILayout.Height(20f);
        private static GUILayoutOption Height25 => GUILayout.Height(25f);
        private static GUILayoutOption Height30 => GUILayout.Height(30f);

        private enum Window
        {
            Console,
            Scene,
            Reference,
            System,
            Project,
            Memory,
            Screen,
            Time,
        }

        [Flags]
        private enum Status
        {
            Common,
            Expand = 1 << 0,
            Freeze = 1 << 1,
            Ping = 1 << 2,
            Window = 1 << 3,
        }

        private enum Pool
        {
            Heap,
            Event,
            Pool,
        }

        [Serializable]
        private class LogData
        {
            public int count;
            public bool status;
            public Color color;

            public LogData(Color color)
            {
                this.color = color;
                status = true;
            }
        }

        [Serializable]
        private struct LogMessage
        {
            public string message;
            public string stackTrace;
            public LogType logType;
            public DateTime dateTime;

            public LogMessage(string message, string stackTrace, LogType logType)
            {
                this.logType = logType;
                this.message = message;
                this.stackTrace = stackTrace;
                dateTime = DateTime.Now;
            }

            public override string ToString()
            {
                return Service.Text.Format("[{0}] [{1}] {2}", dateTime.ToString("HH:mm:ss"), logType, message);
            }
        }

        private class MemoryData
        {
            private readonly Func<long> func;
            private float maxValue;
            private float minValue;

            public MemoryData(Func<long> func)
            {
                this.func = func;
                minValue = 1024;
            }

            public string GetString()
            {
                var value = func.Invoke() / 1024F / 1024F;

                if (value > maxValue)
                {
                    maxValue = value;
                }
                else if (value < minValue)
                {
                    minValue = value;
                }

                return Service.Text.Format("{0:F2} MB\t\t[ 最小值: {1:F2}]\t最大值: {2:F2}", value, minValue, maxValue);
            }
        }
    }
}