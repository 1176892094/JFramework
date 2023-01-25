using System;
using UnityEngine;

namespace JFramework.Core
{
    internal partial class DebugManager
    {
        private void DebugTime()
        {
            GUILayout.BeginHorizontal();
            GUI.contentColor = Color.white;
            GUILayout.Label(DebugConst.TimeInformation, DebugData.Label, DebugData.HeightLow);
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical(DebugData.Box, DebugData.WindowBox);
            GUILayout.Label(DebugConst.CurrentTime + ": " + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"), DebugData.Label);
            GUILayout.Label(DebugConst.ElapseTime + ": " + (int)Time.realtimeSinceStartup, DebugData.Label);
            GUILayout.Label(DebugConst.TimeScale + ": " + Time.timeScale, DebugData.Label);
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("0.1 " + DebugConst.Multiple, DebugData.Button, DebugData.Height))
            {
                Time.timeScale = 0.1f;
            }

            if (GUILayout.Button("0.2 " + DebugConst.Multiple, DebugData.Button, DebugData.Height))
            {
                Time.timeScale = 0.2f;
            }

            if (GUILayout.Button("0.5 " + DebugConst.Multiple, DebugData.Button, DebugData.Height))
            {
                Time.timeScale = 0.5f;
            }

            if (GUILayout.Button("1 " + DebugConst.Multiple, DebugData.Button, DebugData.Height))
            {
                Time.timeScale = 1;
            }

            if (GUILayout.Button("2 " + DebugConst.Multiple, DebugData.Button, DebugData.Height))
            {
                Time.timeScale = 2;
            }

            if (GUILayout.Button("5 " + DebugConst.Multiple, DebugData.Button, DebugData.Height))
            {
                Time.timeScale = 5;
            }

            if (GUILayout.Button("10 " + DebugConst.Multiple, DebugData.Button, DebugData.Height))
            {
                Time.timeScale = 10;
            }

            GUILayout.EndHorizontal();
        }
    }
}