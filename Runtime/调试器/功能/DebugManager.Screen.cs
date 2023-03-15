using System;
using System.Collections;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;


namespace JFramework.Core
{
    internal sealed partial class DebugManager
    {
        private void DebugScreen()
        {
            GUILayout.BeginHorizontal();
            GUI.contentColor = Color.white;
            GUILayout.Label(DebugConst.ScreenInformation, DebugData.Label, DebugData.HeightLow);
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical(DebugData.Box, DebugData.WindowBox);
            GUILayout.Label(DebugConst.DPI + ": " + Screen.dpi, DebugData.Label);
            GUILayout.Label(DebugConst.Resolution + ": " + Screen.width + " x " + Screen.height, DebugData.Label);
            GUILayout.Label(DebugConst.DeviceResolution + ": " + Screen.currentResolution, DebugData.Label);
            GUILayout.Label(DebugConst.DeviceSleep + ": " + (Screen.sleepTimeout == SleepTimeout.NeverSleep ? DebugConst.NeverSleep : DebugConst.SystemSetting), DebugData.Label);
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button(DebugConst.DeviceSleep, DebugData.Button, DebugData.Height))
            {
                Screen.sleepTimeout = Screen.sleepTimeout == SleepTimeout.NeverSleep ? SleepTimeout.SystemSetting : SleepTimeout.NeverSleep;
            }

            if (GUILayout.Button(DebugConst.ScreenCapture, DebugData.Button, DebugData.Height))
            {
                StartCoroutine(ScreenShot());
            }

            GUILayout.EndHorizontal();
        }

        private IEnumerator ScreenShot()
        {
#if UNITY_EDITOR
            GlobalManager.Instance.IsDebugPool = false;
            yield return new WaitForEndOfFrame();
            string screenPath = "Assets/Screen/";
            if (!Directory.Exists(screenPath)) Directory.CreateDirectory(screenPath);
            Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            texture.Apply();
            string title = DateTime.Now.ToString("yyyyMMddhhmmss") + ".png";
            byte[] bytes = texture.EncodeToPNG();
            GlobalManager.Instance.IsDebugPool = true;
            yield return File.WriteAllBytesAsync(screenPath + title, bytes);
            AssetDatabase.Refresh();
#else
            Debug.LogWarning("当前平台不支持截屏！");
            yield return null;
#endif
        }
    }
}