using UnityEngine;

namespace JFramework.Debug
{
    [Debug(typeof(Transform))]
    internal class DebugTransform : DebugComponent
    {
        private Transform target;

        public override void OnInit() => target = (Transform)Target;

        public override void OnDebugGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Position:", DebugStyle.Label, DebugStyle.Component);
            target.localPosition = DebugStyle.Vector3Field(target.localPosition);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Rotation:", DebugStyle.Label, DebugStyle.Component);
            target.localRotation = Quaternion.Euler(DebugStyle.Vector3Field(target.localRotation.eulerAngles));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Scale:", DebugStyle.Label, DebugStyle.Component);
            target.localScale = DebugStyle.Vector3Field(target.localScale);
            GUILayout.EndHorizontal();
        }
    }
}