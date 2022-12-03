using UnityEngine;

namespace JFramework
{
    [Debug(typeof(BoxCollider))]
    internal class DebugBoxCollider : DebugComponent
    {
        private BoxCollider target;

        public override void OnInit() => target = (BoxCollider)Target;

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
            GUILayout.Label("Size: ", DebugStyle.Label, DebugStyle.Component);
            target.size = DebugStyle.Vector3Field(target.size);
            GUILayout.EndHorizontal();
        }
    }
}