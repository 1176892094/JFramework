using UnityEngine;

namespace JFramework
{
    [Debug(typeof(Rigidbody))]
    internal class DebugRigidbody : DebugComponent
    {
        private Rigidbody target;

        public override void OnInit() => target = (Rigidbody)Target;

        public override void OnDebugGUI()
        {
            GUILayout.BeginHorizontal();
            target.useGravity = GUILayout.Toggle(target.useGravity, "Use Gravity", DebugStyle.Button);
            target.isKinematic = GUILayout.Toggle(target.isKinematic, "IsKinematic", DebugStyle.Button);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Velocity:", DebugStyle.Label, DebugStyle.Component);
            target.velocity = DebugStyle.Vector3Field(target.velocity);
            GUILayout.EndHorizontal();
        }
    }
}