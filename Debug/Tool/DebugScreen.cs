using System;
using System.IO;
using JFramework.Async;
using UnityEngine;

namespace JFramework.Logger
{
    internal class DebugScreen
    {
        private readonly DebugData debugData;
        public DebugScreen(DebugData debugData) => this.debugData = debugData;

        public void ExtendScreenGUI()
        {
            GUILayout.BeginHorizontal();
            GUI.contentColor = Color.white;
            GUILayout.Label(debugData.GetData("Screen Information"), GUIStyles.Label, GUIStyles.MinHeight);
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical(GUIStyles.Box, GUILayout.Height(250), GUILayout.Height(debugData.MaxHeight - 260));
            GUILayout.Label(debugData.GetData("DPI") + ": " + Screen.dpi, GUIStyles.Label);
            GUILayout.Label(debugData.GetData("Resolution") + ": " + Screen.width + " x " + Screen.height, GUIStyles.Label);
            GUILayout.Label(debugData.GetData("Device Resolution") + ": " + Screen.currentResolution, GUIStyles.Label);
            GUILayout.Label(debugData.GetData("Device Sleep") + ": " + (Screen.sleepTimeout == SleepTimeout.NeverSleep ? debugData.GetData("Never Sleep") : debugData.GetData("System Setting")),GUIStyles.Label);
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button(debugData.GetData("Device Sleep"), GUIStyles.Button, GUIStyles.MinHeight))
            {
                Screen.sleepTimeout = Screen.sleepTimeout == SleepTimeout.NeverSleep ? SleepTimeout.SystemSetting : SleepTimeout.NeverSleep;
            }

            if (GUILayout.Button(debugData.GetData("Screen Capture"), GUIStyles.Button, GUIStyles.MinHeight))
            {
                ScreenShot();
            }

            GUILayout.EndHorizontal();
        }
        
        private async void ScreenShot()
        {
            string path = "";
#if UNITY_EDITOR
            path = Application.dataPath + "/Screen/";
#endif
            if (path != "")
            {
                debugData.isDebug = false;
                await new WaitForEndOfFrame();
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
                texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
                texture.Apply();
                string title = DateTime.Now.ToString("yyyyMMddhhmmss") + ".png";
                byte[] bytes = texture.EncodeToPNG();
                debugData.isDebug = true;
                await File.WriteAllBytesAsync(path + title, bytes);
            }
            else
            {
                Debug.LogWarning("当前平台不支持截屏！");
            }
        }
    }
}