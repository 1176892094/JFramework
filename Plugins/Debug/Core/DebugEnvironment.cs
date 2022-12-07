using UnityEngine;

namespace JFramework
{
    internal class DebugEnvironment
    {
        private readonly DebugSetting setting;
        public DebugEnvironment(DebugSetting setting) => this.setting = setting;

        public void ExtendEnvironmentGUI()
        {
            GUILayout.BeginHorizontal();
            GUI.contentColor = Color.white;
            GUILayout.Label(setting.GetData("Environment Information"), DebugStyle.Label, DebugStyle.MinHeight);
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical(DebugStyle.Box, GUILayout.Height(setting.MaxHeight - 260));
            GUILayout.Label(setting.GetData("Product Name") + ": " + Application.productName, DebugStyle.Label);
            GUILayout.Label(setting.GetData("Product Identifier") + ": " + Application.identifier, DebugStyle.Label);
            GUILayout.Label(setting.GetData("Product Version") + ": " + Application.version, DebugStyle.Label);
            GUILayout.Label(setting.GetData("Product DataPath") + ": " + Application.dataPath, DebugStyle.Label);
            GUILayout.Label(setting.GetData("Company Name") + ": " + Application.companyName, DebugStyle.Label);
            GUILayout.Label(setting.GetData("Unity Version") + ": " + Application.unityVersion, DebugStyle.Label);
            GUILayout.Label(setting.GetData("Has Pro License") + ": " + Application.HasProLicense(),
                DebugStyle.Label);
            string internetState = setting.GetData("NotReachable");
            switch (Application.internetReachability)
            {
                case NetworkReachability.NotReachable:
                    internetState = setting.GetData("NotReachable");
                    break;
                case NetworkReachability.ReachableViaLocalAreaNetwork:
                    internetState = setting.GetData("ReachableViaLocalAreaNetwork");
                    break;
                case NetworkReachability.ReachableViaCarrierDataNetwork:
                    internetState = setting.GetData("ReachableViaCarrierDataNetwork");
                    break;
            }

            GUILayout.Label(setting.GetData("Internet State") + ": " + internetState, DebugStyle.Label);
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button(setting.GetData("Quit"), DebugStyle.Button, DebugStyle.MinHeight))
            {
                Application.Quit();
            }

            GUILayout.EndHorizontal();
        }
    }
}