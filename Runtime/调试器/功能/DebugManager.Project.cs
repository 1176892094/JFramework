using UnityEngine;

namespace JFramework.Core
{
    internal partial class DebugManager
    {
        private void DebugProject()
        {
            GUILayout.BeginHorizontal();
            GUI.contentColor = Color.white;
            GUILayout.Label(DebugConst.EnvironmentInformation, DebugData.Label, DebugData.HeightLow);
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical(DebugData.Box, DebugData.WindowBox);
            GUILayout.Label(DebugConst.ProductName + ": " + Application.productName, DebugData.Label);
            GUILayout.Label(DebugConst.ProductIdentifier + ": " + Application.identifier, DebugData.Label);
            GUILayout.Label(DebugConst.ProductVersion + ": " + Application.version, DebugData.Label);
            GUILayout.Label(DebugConst.ProductDataPath + ": " + Application.dataPath, DebugData.Label);
            GUILayout.Label(DebugConst.CompanyName + ": " + Application.companyName, DebugData.Label);
            GUILayout.Label(DebugConst.UnityVersion + ": " + Application.unityVersion, DebugData.Label);
            GUILayout.Label(DebugConst.HasProLicense + ": " + Application.HasProLicense(), DebugData.Label);

            string internetState = DebugConst.NotReachable;
            switch (Application.internetReachability)
            {
                case NetworkReachability.NotReachable:
                    internetState = DebugConst.NotReachable;
                    break;
                case NetworkReachability.ReachableViaLocalAreaNetwork:
                    internetState = DebugConst.ReachableViaLocalAreaNetwork;
                    break;
                case NetworkReachability.ReachableViaCarrierDataNetwork:
                    internetState = DebugConst.ReachableViaCarrierDataNetwork;
                    break;
            }

            GUILayout.Label(DebugConst.InternetState + ": " + internetState, DebugData.Label);
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button(DebugConst.Quit, DebugData.Button, DebugData.Height))
            {
                Application.Quit();
            }

            GUILayout.EndHorizontal();
        }
    }
}