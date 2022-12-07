using System;
using System.Collections;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Logger = JFramework.Basic.Logger;

namespace JFramework
{
    internal class DebugScreen
    {
        private readonly DebugSetting setting;
        public DebugScreen(DebugSetting setting) => this.setting = setting;

        public void ExtendScreenGUI()
        {
            GUILayout.BeginHorizontal();
            GUI.contentColor = Color.white;
            GUILayout.Label(setting.GetData("Screen Information"), DebugStyle.Label, DebugStyle.MinHeight);
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical(DebugStyle.Box, GUILayout.Height(250), GUILayout.Height(setting.MaxHeight - 260));
            GUILayout.Label(setting.GetData("DPI") + ": " + Screen.dpi, DebugStyle.Label);
            GUILayout.Label(setting.GetData("Resolution") + ": " + Screen.width + " x " + Screen.height, DebugStyle.Label);
            GUILayout.Label(setting.GetData("Device Resolution") + ": " + Screen.currentResolution, DebugStyle.Label);
            GUILayout.Label(setting.GetData("Device Sleep") + ": " + (Screen.sleepTimeout == SleepTimeout.NeverSleep ? setting.GetData("Never Sleep") : setting.GetData("System Setting")), DebugStyle.Label);
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button(setting.GetData("Device Sleep"), DebugStyle.Button, DebugStyle.MinHeight))
            {
                Screen.sleepTimeout = Screen.sleepTimeout == SleepTimeout.NeverSleep ? SleepTimeout.SystemSetting : SleepTimeout.NeverSleep;
            }

            if (GUILayout.Button(setting.GetData("Screen Capture"), DebugStyle.Button, DebugStyle.MinHeight))
            {
                MonoManager.Instance.StartCoroutine(ScreenShot());
            }

            GUILayout.EndHorizontal();
        }

        private IEnumerator ScreenShot()
        {
#if UNITY_EDITOR
            setting.IsDebug = false;
            yield return new WaitForEndOfFrame();
            if (!Directory.Exists(setting.ScreenPath)) Directory.CreateDirectory(setting.ScreenPath);
            Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            texture.Apply();
            string title = DateTime.Now.ToString("yyyyMMddhhmmss") + ".png";
            byte[] bytes = texture.EncodeToPNG();
            setting.IsDebug = true;
            yield return File.WriteAllBytesAsync(setting.ScreenPath + title, bytes);
            AssetDatabase.Refresh();
#else
            Logger.LogWarning("当前平台不支持截屏！");
#endif
        }
    }
}