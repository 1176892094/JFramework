using UnityEngine;

namespace JFramework.Logger
{
    [Debug(typeof(Rigidbody))]
    internal class DebugRigidbody : DebugComponent
    {
        private Rigidbody target;

        public override void OnInit() => target = (Rigidbody)Target;

        public override void OnDebugGUI()
        {
            GUILayout.BeginHorizontal();
            target.useGravity = GUILayout.Toggle(target.useGravity, "Use Gravity",GUIStyles.Button);
            target.isKinematic = GUILayout.Toggle(target.isKinematic, "IsKinematic",GUIStyles.Button);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Velocity:", GUIStyles.Label,GUIStyles.Component);
            target.velocity = GUIStyles.Vector3Field(target.velocity);
            GUILayout.EndHorizontal();
        }
    }
}
