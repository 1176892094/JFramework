using UnityEngine;

namespace JYJFramework
{
    [Debug(typeof(Transform))]
    public class DebugTransform : Debugger
    {
        private Transform target;

        public override void OnInit()
        {
            target = Target as Transform;
        }

        public override void OnDebuggerGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Position:", GUILayout.Width(130));
            target.localPosition = GUILayoutExtend.Vector3Field(target.localPosition);
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("Rotation:", GUILayout.Width(130));
            target.localRotation = Quaternion.Euler(GUILayoutExtend.Vector3Field(target.localRotation.eulerAngles));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Scale:", GUILayout.Width(130));
            target.localScale = GUILayoutExtend.Vector3Field(target.localScale);
            GUILayout.EndHorizontal();
        }
    }
}
