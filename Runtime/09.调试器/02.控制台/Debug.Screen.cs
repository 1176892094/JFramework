// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-19 02:12:55
// # Recently: 2024-12-22 20:12:37
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace JFramework
{
    internal partial class DebugManager
    {
        private Vector2 scrollScreenView = Vector2.zero;

        private void ScreenWindow()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(" 屏幕信息", Height25);
            GUILayout.EndHorizontal();

            scrollScreenView = GUILayout.BeginScrollView(scrollScreenView, "Box");

            GUILayout.Label("像素密度: " + Screen.dpi);
            GUILayout.Label("启用全屏: " + Screen.fullScreen);
            GUILayout.Label("屏幕模式: " + Screen.fullScreenMode);
            GUILayout.Label("程序分辨率: " + Screen.width + " x " + Screen.height);
            GUILayout.Label("设备分辨率: " + Screen.currentResolution);
            GUILayout.Label("显示区域: " + Screen.safeArea);
            GUILayout.Label("质量等级: " + QualitySettings.names[QualitySettings.GetQualityLevel()]);


            GUILayout.EndScrollView();

            GUILayout.BeginHorizontal();

            for (var i = 0; i < QualitySettings.names.Length; i++)
            {
                var label = QualitySettings.names[i];
                if (GUILayout.Button(label, Height30))
                {
                    QualitySettings.SetQualityLevel(i);
                }
            }
#if UNITY_EDITOR
            if (GUILayout.Button("Screen Shot", Height30))
            {
                ScreenShot();
            }
#endif
            GUILayout.EndHorizontal();
        }

#if UNITY_EDITOR
        private async void ScreenShot()
        {
            status |= Status.Freeze;
            await Task.Yield();
            var fileName = Service.Text.Format("{0:yyyyMMddhhmmss}.png", DateTime.Now);
            var filePath = Service.Text.Format("{0}/{1}", Application.dataPath, fileName);
            Debug.Log(Service.Text.Format("截图保存路径: {0}", filePath));
            ScreenCapture.CaptureScreenshot(filePath);
            await Task.Yield();
            AssetDatabase.Refresh();
            status &= ~Status.Freeze;
        }
#endif
    }
}