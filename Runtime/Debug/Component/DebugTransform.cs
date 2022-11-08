using UnityEngine;

namespace JYJFramework.Logger
{
    [Debug(typeof(Transform))]
    public class DebugTransform : Debugger
    {
        private Transform target;

        public override void OnInit()
        {
            target = Target as Transform;
        }

        public override void OnDebugGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Position:", GUILayout.Width(130));
            target.localPosition = GUIExtensions.Vector3Field(target.localPosition);
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("Rotation:", GUILayout.Width(130));
            target.localRotation = Quaternion.Euler(GUIExtensions.Vector3Field(target.localRotation.eulerAngles));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Scale:", GUILayout.Width(130));
            target.localScale = GUIExtensions.Vector3Field(target.localScale);
            GUILayout.EndHorizontal();
        }
    }
}
