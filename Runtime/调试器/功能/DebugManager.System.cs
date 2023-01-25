using UnityEngine;

namespace JFramework.Core
{
    internal partial class DebugManager
    {
        private Vector2 scrollSystemView = Vector2.zero;

        private void DebugSystem()
        {
            GUILayout.BeginHorizontal();
            GUI.contentColor = Color.white;
            GUILayout.Label(DebugConst.SystemInformation, DebugData.Label, DebugData.HeightLow);
            GUILayout.EndHorizontal();
            scrollSystemView = GUILayout.BeginScrollView(scrollSystemView, DebugData.Box);
            GUILayout.Label(DebugConst.OperatingSystem + ": " + SystemInfo.operatingSystem, DebugData.Label);
            GUILayout.Label(DebugConst.SystemMemory + ": " + SystemInfo.systemMemorySize + "MB", DebugData.Label);
            GUILayout.Label(DebugConst.Processor + ": " + SystemInfo.processorType, DebugData.Label);
            GUILayout.Label(DebugConst.NumberOfProcessor + ": " + SystemInfo.processorCount, DebugData.Label);
            GUILayout.Label(DebugConst.GraphicsDeviceName + ": " + SystemInfo.graphicsDeviceName, DebugData.Label);
            GUILayout.Label(DebugConst.GraphicsDeviceType + ": " + SystemInfo.graphicsDeviceType, DebugData.Label);
            GUILayout.Label(DebugConst.GraphicsMemory + ": " + SystemInfo.graphicsMemorySize + "MB", DebugData.Label);
            GUILayout.Label(DebugConst.GraphicsDeviceID + ": " + SystemInfo.graphicsDeviceID, DebugData.Label);
            GUILayout.Label(DebugConst.GraphicsDeviceVendor + ": " + SystemInfo.graphicsDeviceVendor, DebugData.Label);
            GUILayout.Label(DebugConst.GraphicsDeviceVendorID + ": " + SystemInfo.graphicsDeviceVendorID, DebugData.Label);
            GUILayout.Label(DebugConst.DeviceModel + ": " + SystemInfo.deviceModel, DebugData.Label);
            GUILayout.Label(DebugConst.DeviceName + ": " + SystemInfo.deviceName, DebugData.Label);
            GUILayout.Label(DebugConst.DeviceType + ": " + SystemInfo.deviceType, DebugData.Label);
            GUILayout.Label(DebugConst.DeviceUniqueIdentifier + ": " + SystemInfo.deviceUniqueIdentifier, DebugData.Label);
            GUILayout.EndScrollView();
        }
    }
}