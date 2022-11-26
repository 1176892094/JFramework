using UnityEngine;

namespace JFramework.Logger
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
            target.enabled = GUILayout.Toggle(target.enabled, "IsEnabled",GUIStyles.Button);
            target.isTrigger = GUILayout.Toggle(target.isTrigger, "IsTrigger",GUIStyles.Button);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Center: ", GUIStyles.Label,GUIStyles.Component);
            target.center = GUIStyles.Vector3Field(target.center);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Direction: ", GUIStyles.Label,GUIStyles.Component);
            target.direction = GUIStyles.IntField(target.direction);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Height: ", GUIStyles.Label,GUIStyles.Component);
            target.height = GUIStyles.FloatField(target.height);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Radius: ", GUIStyles.Label,GUIStyles.Component);
            target.radius = GUIStyles.FloatField(target.radius);
            GUILayout.EndHorizontal();
        }
    }
}
