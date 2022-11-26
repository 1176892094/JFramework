using UnityEngine;

namespace JFramework.Logger
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
            target.enabled = GUILayout.Toggle(target.enabled, "IsEnabled",GUIStyles.Button);
            target.isTrigger = GUILayout.Toggle(target.isTrigger, "IsTrigger",GUIStyles.Button);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Center: ", GUIStyles.Label,GUIStyles.Component);
            target.center = GUIStyles.Vector3Field(target.center);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Size: ", GUIStyles.Label,GUIStyles.Component);
            target.size = GUIStyles.Vector3Field(target.size);
            GUILayout.EndHorizontal();
        }
    }
}
