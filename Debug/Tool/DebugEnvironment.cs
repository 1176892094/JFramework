using UnityEngine;

namespace JFramework.Logger
{
    internal class DebugEnvironment
    {
        private readonly DebugData debugData;
        public DebugEnvironment(DebugData debugData) => this.debugData = debugData;

        public void ExtendEnvironmentGUI()
        {
            GUILayout.BeginHorizontal();
            GUI.contentColor = Color.white;
            GUILayout.Label(debugData.GetData("Environment Information"), GUIStyles.Label, GUIStyles.MinHeight);
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical(GUIStyles.Box, GUILayout.Height(debugData.MaxHeight - 260));
            GUILayout.Label(debugData.GetData("Product Name") + ": " + Application.productName, GUIStyles.Label);
            GUILayout.Label(debugData.GetData("Product Identifier") + ": " + Application.identifier, GUIStyles.Label);
            GUILayout.Label(debugData.GetData("Product Version") + ": " + Application.version, GUIStyles.Label);
            GUILayout.Label(debugData.GetData("Product DataPath") + ": " + Application.dataPath, GUIStyles.Label);
            GUILayout.Label(debugData.GetData("Company Name") + ": " + Application.companyName, GUIStyles.Label);
            GUILayout.Label(debugData.GetData("Unity Version") + ": " + Application.unityVersion, GUIStyles.Label);
            GUILayout.Label(debugData.GetData("Has Pro License") + ": " + Application.HasProLicense(), GUIStyles.Label);
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

            GUILayout.Label(debugData.GetData("Internet State") + ": " + internetState,GUIStyles.Label);
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button(debugData.GetData("Quit"), GUIStyles.Button, GUIStyles.MinHeight))
            {
                Application.Quit();
            }

            GUILayout.EndHorizontal();
        }
    }
}