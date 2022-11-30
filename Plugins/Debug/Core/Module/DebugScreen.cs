using System;
using System.Collections;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using JFramework.Basic;
using Logger = JFramework.Basic.Logger;

namespace JFramework.Debug
{
    internal class DebugScreen
    {
        private readonly DebugData debugData;
        public DebugScreen(DebugData debugData) => this.debugData = debugData;

        public void ExtendScreenGUI()
        {
            GUILayout.BeginHorizontal();
            GUI.contentColor = Color.white;
            GUILayout.Label(debugData.GetData("Screen Information"), DebugStyle.Label, DebugStyle.MinHeight);
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical(DebugStyle.Box, GUILayout.Height(250), GUILayout.Height(debugData.MaxHeight - 260));
            GUILayout.Label(debugData.GetData("DPI") + ": " + Screen.dpi, DebugStyle.Label);
            GUILayout.Label(debugData.GetData("Resolution") + ": " + Screen.width + " x " + Screen.height, DebugStyle.Label);
            GUILayout.Label(debugData.GetData("Device Resolution") + ": " + Screen.currentResolution, DebugStyle.Label);
            GUILayout.Label(debugData.GetData("Device Sleep") + ": " + (Screen.sleepTimeout == SleepTimeout.NeverSleep ? debugData.GetData("Never Sleep") : debugData.GetData("System Setting")), DebugStyle.Label);
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button(debugData.GetData("Device Sleep"), DebugStyle.Button, DebugStyle.MinHeight))
            {
                Screen.sleepTimeout = Screen.sleepTimeout == SleepTimeout.NeverSleep ? SleepTimeout.SystemSetting : SleepTimeout.NeverSleep;
            }

            if (GUILayout.Button(debugData.GetData("Screen Capture"), DebugStyle.Button, DebugStyle.MinHeight))
            {
                MonoManager.Instance.StartCoroutine(ScreenShot());
            }

            GUILayout.EndHorizontal();
        }

        private IEnumerator ScreenShot()
        {
#if UNITY_EDITOR
            debugData.isDebug = false;
            yield return new WaitForEndOfFrame();
            if (!Directory.Exists(debugData.screenPath)) Directory.CreateDirectory(debugData.screenPath);
            Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            texture.Apply();
            string title = DateTime.Now.ToString("yyyyMMddhhmmss") + ".png";
            byte[] bytes = texture.EncodeToPNG();
            debugData.isDebug = true;
            File.WriteAllBytes(debugData.screenPath + title, bytes);
            AssetDatabase.Refresh();
#else
            Logger.LogWarning("当前平台不支持截屏！");
#endif
            yield return null;
        }
    }
}