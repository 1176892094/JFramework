using UnityEngine;

namespace JFramework
{
    [Debug(typeof(Rigidbody))]
    internal class DebugComponentRigidbody : DebugComponent<Rigidbody>
    {
        protected override void OnDebugScene()
        {
            GUILayout.BeginHorizontal();
            target.useGravity = GUILayout.Toggle(target.useGravity, "Use Gravity", DebugData.Button);
            target.isKinematic = GUILayout.Toggle(target.isKinematic, "IsKinematic", DebugData.Button);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Velocity:", DebugData.Label, DebugData.Component);
            target.velocity = DebugData.Vector3Field(target.velocity);
            GUILayout.EndHorizontal();
        }
    }
}