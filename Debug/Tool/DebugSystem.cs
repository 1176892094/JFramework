using UnityEngine;

namespace JFramework.Logger
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
            GUILayout.Label(debugData.GetData("System Information"), GUIStyles.Label, GUIStyles.MinHeight);
            GUILayout.EndHorizontal();
            scrollSystemView = GUILayout.BeginScrollView(scrollSystemView, GUIStyles.Box);
            GUILayout.Label(debugData.GetData("Operating System") + ": " + SystemInfo.operatingSystem, GUIStyles.Label);
            GUILayout.Label(debugData.GetData("System Memory") + ": " + SystemInfo.systemMemorySize + "MB", GUIStyles.Label);
            GUILayout.Label(debugData.GetData("Processor") + ": " + SystemInfo.processorType, GUIStyles.Label);
            GUILayout.Label(debugData.GetData("Number Of Processor") + ": " + SystemInfo.processorCount, GUIStyles.Label);
            GUILayout.Label(debugData.GetData("Graphics Device Name") + ": " + SystemInfo.graphicsDeviceName, GUIStyles.Label);
            GUILayout.Label(debugData.GetData("Graphics Device Type") + ": " + SystemInfo.graphicsDeviceType, GUIStyles.Label);
            GUILayout.Label(debugData.GetData("Graphics Memory") + ": " + SystemInfo.graphicsMemorySize + "MB", GUIStyles.Label);
            GUILayout.Label(debugData.GetData("Graphics DeviceID") + ": " + SystemInfo.graphicsDeviceID, GUIStyles.Label);
            GUILayout.Label(debugData.GetData("Graphics Device Vendor") + ": " + SystemInfo.graphicsDeviceVendor, GUIStyles.Label);
            GUILayout.Label(debugData.GetData("Graphics Device Vendor ID") + ": " + SystemInfo.graphicsDeviceVendorID, GUIStyles.Label);
            GUILayout.Label(debugData.GetData("Device Model") + ": " + SystemInfo.deviceModel, GUIStyles.Label);
            GUILayout.Label(debugData.GetData("Device Name") + ": " + SystemInfo.deviceName, GUIStyles.Label);
            GUILayout.Label(debugData.GetData("Device Type") + ": " + SystemInfo.deviceType, GUIStyles.Label);
            GUILayout.Label(debugData.GetData("Device Unique Identifier") + ": " + SystemInfo.deviceUniqueIdentifier, GUIStyles.Label);
            GUILayout.EndScrollView();
        }
    }
}