using System;
using UnityEngine;

namespace JFramework
{
    internal class DebugTime
    {
        private readonly DebugSetting setting;
        public DebugTime(DebugSetting setting) => this.setting = setting;
        
        public void ExtendTimeGUI()
        {
            GUILayout.BeginHorizontal();
            GUI.contentColor = Color.white;
            GUILayout.Label(setting.GetData("Time Information"), DebugStyle.Label, DebugStyle.MinHeight);
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical(DebugStyle.Box, GUILayout.Height(setting.MaxHeight - 260));
            GUILayout.Label(setting.GetData("Current Time") + ": " + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"), DebugStyle.Label);
            GUILayout.Label(setting.GetData("Elapse Time") + ": " + (int)Time.realtimeSinceStartup, DebugStyle.Label);
            GUILayout.Label(setting.GetData("Time Scale") + ": " + Time.timeScale, DebugStyle.Label);
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("0.1 " + setting.GetData("Multiple"), DebugStyle.Button, DebugStyle.MinHeight))
            {
                Time.timeScale = 0.1f;
            }

            if (GUILayout.Button("0.2 " + setting.GetData("Multiple"), DebugStyle.Button, DebugStyle.MinHeight))
            {
                Time.timeScale = 0.2f;
            }

            if (GUILayout.Button("0.5 " + setting.GetData("Multiple"), DebugStyle.Button, DebugStyle.MinHeight))
            {
                Time.timeScale = 0.5f;
            }

            if (GUILayout.Button("1 " + setting.GetData("Multiple"), DebugStyle.Button, DebugStyle.MinHeight))
            {
                Time.timeScale = 1;
            }

            if (GUILayout.Button("2 " + setting.GetData("Multiple"), DebugStyle.Button, DebugStyle.MinHeight))
            {
                Time.timeScale = 2;
            }

            if (GUILayout.Button("5 " + setting.GetData("Multiple"), DebugStyle.Button, DebugStyle.MinHeight))
            {
                Time.timeScale = 5;
            }

            if (GUILayout.Button("10 " + setting.GetData("Multiple"), DebugStyle.Button, DebugStyle.MinHeight))
            {
                Time.timeScale = 10;
            }

            GUILayout.EndHorizontal();
        }
    }
}