using UnityEngine;

namespace JFramework
{
    [Debug(typeof(SphereCollider))]
    internal class DebugComponentSphereCollider : DebugComponent<SphereCollider>
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
            GUILayout.Label("Radius: ", DebugData.Label, DebugData.Component);
            target.radius = DebugData.FloatField(target.radius);
            GUILayout.EndHorizontal();
        }
    }
}