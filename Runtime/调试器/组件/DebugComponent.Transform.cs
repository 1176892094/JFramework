using UnityEngine;

namespace JFramework
{
    [Debug(typeof(Transform))]
    internal class DebugComponentTransform : DebugComponent<Transform>
    {
        protected override void OnDebugScene()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Position:", DebugData.Label, DebugData.Component);
            target.localPosition = DebugData.Vector3Field(target.localPosition);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Rotation:", DebugData.Label, DebugData.Component);
            target.localRotation = Quaternion.Euler(DebugData.Vector3Field(target.localRotation.eulerAngles));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Scale:", DebugData.Label, DebugData.Component);
            target.localScale = DebugData.Vector3Field(target.localScale);
            GUILayout.EndHorizontal();
        }
    }
}