using UnityEngine;

namespace JFramework
{
    [Debug(typeof(BoxCollider))]
    internal class DebugComponentBoxCollider : DebugComponent<BoxCollider>
    {
        protected override void OnDebugScene()
        {
            GUILayout.BeginHorizontal();
            GUI.contentColor = target.enabled ? Color.white : Color.gray;
            target.enabled = GUILayout.Toggle(target.enabled, "IsEnabled", DebugData.Button);
            target.isTrigger = GUILayout.Toggle(target.isTrigger, "IsTrigger", DebugData.Button);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Center: ", DebugData.Label, DebugData.Component);
            target.center = DebugData.Vector3Field(target.center);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Size: ", DebugData.Label, DebugData.Component);
            target.size = DebugData.Vector3Field(target.size);
            GUILayout.EndHorizontal();
        }
    }
}