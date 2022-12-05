using UnityEngine;

namespace JFramework
{
    [Debug(typeof(CharacterController))]
    internal class DebugCharacterController : DebugComponent
    {
        private CharacterController target;

        public override void OnInit() => target = (CharacterController)Target;

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
            GUILayout.Label("Height: ", DebugStyle.Label, DebugStyle.Component);
            target.height = DebugStyle.FloatField(target.height);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Radius: ", DebugStyle.Label, DebugStyle.Component);
            target.radius = DebugStyle.FloatField(target.radius);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("IsGround: " + target.isGrounded, DebugStyle.Label);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Velocity: " + target.velocity, DebugStyle.Label);
            GUILayout.EndHorizontal();
        }
    }
}