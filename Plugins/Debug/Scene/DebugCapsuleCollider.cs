using UnityEngine;

namespace JFramework
{
    [Debug(typeof(CapsuleCollider))]
    internal class DebugCapsuleCollider : DebugComponent
    {
        private CapsuleCollider target;

        public override void OnInit() => target = (CapsuleCollider)Target;


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
            GUILayout.Label("Direction: ", DebugStyle.Label, DebugStyle.Component);
            target.direction = DebugStyle.IntField(target.direction);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Height: ", DebugStyle.Label, DebugStyle.Component);
            target.height = DebugStyle.FloatField(target.height);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Radius: ", DebugStyle.Label, DebugStyle.Component);
            target.radius = DebugStyle.FloatField(target.radius);
            GUILayout.EndHorizontal();
        }
    }
}