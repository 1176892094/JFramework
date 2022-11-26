using UnityEngine;

namespace JFramework.Logger
{
    [Debug(typeof(Transform))]
    internal class DebugTransform : DebugComponent
    {
        private Transform target;

        public override void OnInit() => target = (Transform)Target;

        public override void OnDebugGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Position:", GUIStyles.Label,GUIStyles.Component);
            target.localPosition = GUIStyles.Vector3Field(target.localPosition);
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("Rotation:", GUIStyles.Label,GUIStyles.Component);
            target.localRotation = Quaternion.Euler(GUIStyles.Vector3Field(target.localRotation.eulerAngles));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Scale:", GUIStyles.Label,GUIStyles.Component);
            target.localScale = GUIStyles.Vector3Field(target.localScale);
            GUILayout.EndHorizontal();
        }
    }
}
