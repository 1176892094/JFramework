using UnityEngine;

namespace JFramework.Debug
{
    [Debug(typeof(SphereCollider))]
    internal class DebugSphereCollider : DebugComponent
    {
        private SphereCollider target;

        public override void OnInit() => target = (SphereCollider)Target;

        public override void OnDebugGUI()
        {
            GUILayout.BeginHorizontal();
            GUI.contentColor = target.enabled ? Color.white : Color.gray;
            target.enabled = GUILayout.Toggle(target.enabled, "IsEnabled", DebugStyle.Button);
            target.isTrigger = GUILayout.Toggle(target.isTrigger, "IsTrigger", DebugStyle.Button);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Center: ", DebugStyle.Label, DebugStyle.Component);
            target.center = DebugStyle.Vector3Field(target.center);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Radius: ", DebugStyle.Label, DebugStyle.Component);
            target.radius = DebugStyle.FloatField(target.radius);
            GUILayout.EndHorizontal();
        }
    }
}