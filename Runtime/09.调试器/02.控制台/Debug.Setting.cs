// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-18 14:12:36
// # Recently: 2024-12-22 20:12:36
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using UnityEngine;

namespace JFramework
{
    internal partial class DebugManager
    {
        private Vector2 scrollPathView = Vector2.zero;

        private void SettingWindow()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(" 路径配置", Height25);
            GUILayout.EndHorizontal();

            scrollPathView = GUILayout.BeginScrollView(scrollPathView, "Box");
            GUILayout.Label("项目路径: " + Application.dataPath);
            GUILayout.Label("存储路径: " + Application.persistentDataPath);
            GUILayout.Label("流动资源路径: " + Application.streamingAssetsPath);
            GUILayout.Label("临时缓存路径: " + Application.temporaryCachePath);
            GUILayout.EndScrollView();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("0.5x", Height30))
            {
                ScreenRate = new Vector2(3200, 1800);
            }

            if (GUILayout.Button("1.0x", Height30))
            {
                ScreenRate = new Vector2(2560, 1440);
            }

            if (GUILayout.Button("1.5x", Height30))
            {
                ScreenRate = new Vector2(1920, 1080);
            }


            if (GUILayout.Button("2.0x", Height30))
            {
                ScreenRate = new Vector2(1280, 720);
            }

            GUILayout.EndHorizontal();
        }
    }
}