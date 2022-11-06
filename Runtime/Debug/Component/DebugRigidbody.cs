using UnityEngine;

namespace JYJFramework
{
    [Debug(typeof(Rigidbody))]
    public class DebugRigidbody : Debugger
    {
        private Rigidbody target;

        public override void OnInit()
        {
            target = Target as Rigidbody;
        }

        public override void OnDebuggerGUI()
        {
            GUILayout.BeginHorizontal();
            target.useGravity = GUILayout.Toggle(target.useGravity, "Use Gravity","Button");
            target.isKinematic = GUILayout.Toggle(target.isKinematic, "IsKinematic","Button");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Velocity:", GUILayout.Width(130));
            target.velocity = GUILayoutExtend.Vector3Field(target.velocity);
            GUILayout.EndHorizontal();
        }
    }
}
