using System;
using UnityEngine;

namespace JFramework.Logger
{
    internal class DebugTime
    {
        private readonly DebugData debugData;
        public DebugTime(DebugData debugData) => this.debugData = debugData;
        
        public void ExtendTimeGUI()
        {
            GUILayout.BeginHorizontal();
            GUI.contentColor = Color.white;
            GUILayout.Label(debugData.GetData("Time Information"), GUIStyles.Label, GUIStyles.MinHeight);
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical(GUIStyles.Box, GUILayout.Height(debugData.MaxHeight - 260));
            GUILayout.Label(debugData.GetData("Current Time") + ": " + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"), GUIStyles.Label);
            GUILayout.Label(debugData.GetData("Elapse Time") + ": " + (int)Time.realtimeSinceStartup, GUIStyles.Label);
            GUILayout.Label(debugData.GetData("Time Scale") + ": " + Time.timeScale, GUIStyles.Label);
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("0.1 " + debugData.GetData("Multiple"), GUIStyles.Button, GUIStyles.MinHeight))
            {
                Time.timeScale = 0.1f;
            }

            if (GUILayout.Button("0.2 " + debugData.GetData("Multiple"), GUIStyles.Button, GUIStyles.MinHeight))
            {
                Time.timeScale = 0.2f;
            }

            if (GUILayout.Button("0.5 " + debugData.GetData("Multiple"), GUIStyles.Button, GUIStyles.MinHeight))
            {
                Time.timeScale = 0.5f;
            }

            if (GUILayout.Button("1 " + debugData.GetData("Multiple"), GUIStyles.Button, GUIStyles.MinHeight))
            {
                Time.timeScale = 1;
            }

            if (GUILayout.Button("2 " + debugData.GetData("Multiple"), GUIStyles.Button, GUIStyles.MinHeight))
            {
                Time.timeScale = 2;
            }

            if (GUILayout.Button("5 " + debugData.GetData("Multiple"), GUIStyles.Button, GUIStyles.MinHeight))
            {
                Time.timeScale = 5;
            }

            if (GUILayout.Button("10 " + debugData.GetData("Multiple"), GUIStyles.Button, GUIStyles.MinHeight))
            {
                Time.timeScale = 10;
            }

            GUILayout.EndHorizontal();
        }
    }
}