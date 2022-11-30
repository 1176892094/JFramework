using JFramework.Debug;
using UnityEngine;

namespace JFramework.Debug
{
    internal class DebugEnvironment
    {
        private readonly DebugData debugData;
        public DebugEnvironment(DebugData debugData) => this.debugData = debugData;

        public void ExtendEnvironmentGUI()
        {
            GUILayout.BeginHorizontal();
            GUI.contentColor = Color.white;
            GUILayout.Label(debugData.GetData("Environment Information"), DebugStyle.Label, DebugStyle.MinHeight);
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical(DebugStyle.Box, GUILayout.Height(debugData.MaxHeight - 260));
            GUILayout.Label(debugData.GetData("Product Name") + ": " + Application.productName, DebugStyle.Label);
            GUILayout.Label(debugData.GetData("Product Identifier") + ": " + Application.identifier, DebugStyle.Label);
            GUILayout.Label(debugData.GetData("Product Version") + ": " + Application.version, DebugStyle.Label);
            GUILayout.Label(debugData.GetData("Product DataPath") + ": " + Application.dataPath, DebugStyle.Label);
            GUILayout.Label(debugData.GetData("Company Name") + ": " + Application.companyName, DebugStyle.Label);
            GUILayout.Label(debugData.GetData("Unity Version") + ": " + Application.unityVersion, DebugStyle.Label);
            GUILayout.Label(debugData.GetData("Has Pro License") + ": " + Application.HasProLicense(),
                DebugStyle.Label);
            string internetState = debugData.GetData("NotReachable");
            switch (Application.internetReachability)
            {
                case NetworkReachability.NotReachable:
                    internetState = debugData.GetData("NotReachable");
                    break;
                case NetworkReachability.ReachableViaLocalAreaNetwork:
                    internetState = debugData.GetData("ReachableViaLocalAreaNetwork");
                    break;
                case NetworkReachability.ReachableViaCarrierDataNetwork:
                    internetState = debugData.GetData("ReachableViaCarrierDataNetwork");
                    break;
            }

            GUILayout.Label(debugData.GetData("Internet State") + ": " + internetState, DebugStyle.Label);
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button(debugData.GetData("Quit"), DebugStyle.Button, DebugStyle.MinHeight))
            {
                Application.Quit();
            }

            GUILayout.EndHorizontal();
        }
    }
}