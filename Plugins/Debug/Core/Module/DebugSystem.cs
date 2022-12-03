using UnityEngine;

namespace JFramework
{
    internal class DebugSystem
    {
        private readonly DebugData debugData;
        private Vector2 scrollSystemView = Vector2.zero;
        public DebugSystem(DebugData debugData) => this.debugData = debugData;
        
        public void ExtendSystemGUI()
        {
            GUILayout.BeginHorizontal();
            GUI.contentColor = Color.white;
            GUILayout.Label(debugData.GetData("System Information"), DebugStyle.Label, DebugStyle.MinHeight);
            GUILayout.EndHorizontal();
            scrollSystemView = GUILayout.BeginScrollView(scrollSystemView, DebugStyle.Box);
            GUILayout.Label(debugData.GetData("Operating System") + ": " + SystemInfo.operatingSystem, DebugStyle.Label);
            GUILayout.Label(debugData.GetData("System Memory") + ": " + SystemInfo.systemMemorySize + "MB", DebugStyle.Label);
            GUILayout.Label(debugData.GetData("Processor") + ": " + SystemInfo.processorType, DebugStyle.Label);
            GUILayout.Label(debugData.GetData("Number Of Processor") + ": " + SystemInfo.processorCount, DebugStyle.Label);
            GUILayout.Label(debugData.GetData("Graphics Device Name") + ": " + SystemInfo.graphicsDeviceName, DebugStyle.Label);
            GUILayout.Label(debugData.GetData("Graphics Device Type") + ": " + SystemInfo.graphicsDeviceType, DebugStyle.Label);
            GUILayout.Label(debugData.GetData("Graphics Memory") + ": " + SystemInfo.graphicsMemorySize + "MB", DebugStyle.Label);
            GUILayout.Label(debugData.GetData("Graphics DeviceID") + ": " + SystemInfo.graphicsDeviceID, DebugStyle.Label);
            GUILayout.Label(debugData.GetData("Graphics Device Vendor") + ": " + SystemInfo.graphicsDeviceVendor, DebugStyle.Label);
            GUILayout.Label(debugData.GetData("Graphics Device Vendor ID") + ": " + SystemInfo.graphicsDeviceVendorID, DebugStyle.Label);
            GUILayout.Label(debugData.GetData("Device Model") + ": " + SystemInfo.deviceModel, DebugStyle.Label);
            GUILayout.Label(debugData.GetData("Device Name") + ": " + SystemInfo.deviceName, DebugStyle.Label);
            GUILayout.Label(debugData.GetData("Device Type") + ": " + SystemInfo.deviceType, DebugStyle.Label);
            GUILayout.Label(debugData.GetData("Device Unique Identifier") + ": " + SystemInfo.deviceUniqueIdentifier, DebugStyle.Label);
            GUILayout.EndScrollView();
        }
    }
}