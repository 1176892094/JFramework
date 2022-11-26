using UnityEngine;

namespace JFramework.Logger
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
            target.enabled = GUILayout.Toggle(target.enabled, "IsEnabled",GUIStyles.Button);
            target.isTrigger = GUILayout.Toggle(target.isTrigger, "IsTrigger",GUIStyles.Button);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Center: ", GUIStyles.Label,GUIStyles.Component);
            target.center = GUIStyles.Vector3Field(target.center);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Height: ", GUIStyles.Label,GUIStyles.Component);
            target.height = GUIStyles.FloatField(target.height);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Radius: ", GUIStyles.Label,GUIStyles.Component);
            target.radius = GUIStyles.FloatField(target.radius);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("IsGround: " + target.isGrounded,GUIStyles.Label);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Velocity: " + target.velocity,GUIStyles.Label);
            GUILayout.EndHorizontal();
        }
    }
}
