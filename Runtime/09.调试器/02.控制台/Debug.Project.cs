// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-08-24 17:08:14
// # Recently: 2024-12-22 20:12:40
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using UnityEngine;

namespace JFramework
{
    internal partial class DebugManager
    {
        private Vector2 scrollProjectView = Vector2.zero;

        private void ProjectWindow()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(" 环境配置", Height25);
            GUILayout.EndHorizontal();

            scrollProjectView = GUILayout.BeginScrollView(scrollProjectView, "Box");
            GUILayout.Label("项目名称: " + Application.productName);
            GUILayout.Label("项目标识: " + Application.identifier);
            GUILayout.Label("项目版本: " + Application.version);
            GUILayout.Label("运行平台: " + Application.platform);
            GUILayout.Label("公司名称: " + Application.companyName);
            GUILayout.Label("Unity版本: " + Application.unityVersion);
            GUILayout.Label("Unity专业版: " + Application.HasProLicense());

            var message = "";
            switch (Application.internetReachability)
            {
                case NetworkReachability.NotReachable:
                    message = "当前设备无法访问互联网";
                    break;
                case NetworkReachability.ReachableViaCarrierDataNetwork:
                    message = "当前设备通过 蜂窝移动网络 连接到互联网";
                    break;
                case NetworkReachability.ReachableViaLocalAreaNetwork:
                    message = "当前设备通过 WiFi 或有线网络连接到互联网";
                    break;
            }

            GUILayout.Label("网络状态: " + message);

            GUILayout.EndScrollView();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("退出游戏", Height30))
            {
                Application.Quit();
            }

            GUILayout.EndHorizontal();
        }
    }
}