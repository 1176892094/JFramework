using UnityEngine;

namespace JYJFramework.Logger
{
    [Debug(typeof(CharacterController))]
    public class DebugCharacterController : Debugger
    {
        private CharacterController target;

        public override void OnInit()
        {
            target = Target as CharacterController;
        }

        public override void OnDebuggerGUI()
        {
            GUILayout.BeginHorizontal();
            GUI.contentColor = target.enabled ? Color.white : Color.gray;
            target.enabled = GUILayout.Toggle(target.enabled, "IsEnabled","Button");
            target.isTrigger = GUILayout.Toggle(target.isTrigger, "IsTrigger","Button");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Center: ", GUILayout.Width(130));
            target.center = GUILayoutExtend.Vector3Field(target.center);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Height: ", GUILayout.Width(130));
            target.height = GUILayoutExtend.FloatField(target.height);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Radius: ", GUILayout.Width(130));
            target.radius = GUILayoutExtend.FloatField(target.radius);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("IsGround: " + target.isGrounded);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Velocity: " + target.velocity);
            GUILayout.EndHorizontal();
        }
    }
}
