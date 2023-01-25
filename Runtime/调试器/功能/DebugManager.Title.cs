using UnityEngine;

namespace JFramework.Core
{
    internal partial class DebugManager
    {
        private void DebugTitle()
        {
            GUILayout.BeginHorizontal();
            GUI.contentColor = FPSColor;
            if (GUILayout.Button($"{DebugConst.FPS}: {FPS}", DebugData.Button, DebugData.Width, DebugData.Height))
            {
                IsExtend = false;
            }

            GUI.contentColor = debugType == DebugType.Console ? Color.white : Color.gray;
            if (GUILayout.Button(DebugConst.Console, DebugData.Button, DebugData.Height))
            {
                debugType = DebugType.Console;
            }

            GUI.contentColor = debugType == DebugType.Scene ? Color.white : Color.gray;
            if (GUILayout.Button(DebugConst.Scene, DebugData.Button, DebugData.Height))
            {
                debugType = DebugType.Scene;
                RefreshGameObject();
                RefreshComponent();
            }

            GUI.contentColor = debugType == DebugType.Memory ? Color.white : Color.gray;
            if (GUILayout.Button(DebugConst.Memory, DebugData.Button, DebugData.Height))
            {
                debugType = DebugType.Memory;
            }

            GUI.contentColor = debugType == DebugType.DrawCall ? Color.white : Color.gray;
            if (GUILayout.Button(DebugConst.DrawCall, DebugData.Button, DebugData.Height))
            {
                debugType = DebugType.DrawCall;
            }

            GUI.contentColor = debugType == DebugType.System ? Color.white : Color.gray;
            if (GUILayout.Button(DebugConst.System, DebugData.Button, DebugData.Height))
            {
                debugType = DebugType.System;
            }

            GUI.contentColor = debugType == DebugType.Screen ? Color.white : Color.gray;
            if (GUILayout.Button(DebugConst.Screen, DebugData.Button, DebugData.Height))
            {
                debugType = DebugType.Screen;
            }

            GUI.contentColor = debugType == DebugType.Time ? Color.white : Color.gray;
            if (GUILayout.Button(DebugConst.Time, DebugData.Button, DebugData.Height))
            {
                debugType = DebugType.Time;
            }

            GUI.contentColor = debugType == DebugType.Environment ? Color.white : Color.gray;
            if (GUILayout.Button(DebugConst.Environment, DebugData.Button, DebugData.Height))
            {
                debugType = DebugType.Environment;
            }

            GUI.contentColor = Color.white;
            GUILayout.EndHorizontal();
        }
    }
}