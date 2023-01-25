using UnityEngine;

namespace JFramework
{
    [Debug(typeof(CharacterController))]
    internal class DebugComponentCharacterController : DebugComponent<CharacterController>
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
            GUILayout.Label("Height: ", DebugData.Label, DebugData.Component);
            target.height = DebugData.FloatField(target.height);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Radius: ", DebugData.Label, DebugData.Component);
            target.radius = DebugData.FloatField(target.radius);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("IsGround: " + target.isGrounded, DebugData.Label);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Velocity: " + target.velocity, DebugData.Label);
            GUILayout.EndHorizontal();
        }
    }
}